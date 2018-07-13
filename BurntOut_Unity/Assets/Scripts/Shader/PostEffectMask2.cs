// This version works with Post Processing Stack Debug Views and Ambient Occlusion
// drawback: transparent objects disappear where the post effect has been masked

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class PostEffectMask2 : MonoBehaviour {

    public List<MeshFilter> maskObjects;
    public bool invert;

    private Material m_stencilWriteMaterial;
    private Material m_stencilBlitMaterial;

    private Camera m_camera;
    private CommandBuffer m_beforePostFX;
    private RenderTexture m_initial;
    private RenderTexture m_unprocessed;

    private void Awake() {
        m_camera = GetComponent<Camera>();
    }

    private void OnEnable() {
        CreateResources();
        AddAsFirstCommandBuffer(m_camera, CameraEvent.BeforeImageEffectsOpaque, m_beforePostFX);
    }

    private void OnDisable() {
        if (m_beforePostFX != null)
            m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, m_beforePostFX);
        DisposeResources();
    }

    private void CreateResources() {
        m_beforePostFX = new CommandBuffer();
        m_beforePostFX.name = "PostEffectMask2";

        // create the stencil materials
        var stencilShader = Shader.Find("Unlit/Stencil");

        m_stencilWriteMaterial = new Material(stencilShader);
        m_stencilWriteMaterial.SetFloat("_StencilOp", (float)StencilOp.Replace);
        m_stencilWriteMaterial.SetFloat("_DepthComp", (float)CompareFunction.LessEqual); // normal z test for the mask objects
        m_stencilWriteMaterial.SetFloat("_ColorMask", 0); // write only to the stencil buffer

        m_stencilBlitMaterial = new Material(stencilShader); // stencil comparison is set in OnRenderImage
    }

    private void DisposeResources() {
        DestroyImmediate(m_stencilWriteMaterial);
        DestroyImmediate(m_stencilBlitMaterial);

        if (m_unprocessed != null)
            RenderTexture.ReleaseTemporary(m_unprocessed);
        m_unprocessed = null;

        m_beforePostFX.Dispose();
        m_beforePostFX = null;
    }

    private void OnPreRender() {
        // prepare to copy the unprocessed image before post effects

        if (m_unprocessed != null)
            RenderTexture.ReleaseTemporary(m_unprocessed);
        m_unprocessed = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
        m_beforePostFX.Clear();
        m_beforePostFX.Blit(BuiltinRenderTextureType.CurrentActive, m_unprocessed);
    }

    private void OnPostRender() {
        // write the mask objects on top of the scene render with a stencil write shader
        if (maskObjects != null && maskObjects.Count > 0) {
            m_stencilWriteMaterial.SetPass(0);
            foreach (var o in maskObjects) {
                if (o == null) continue;
                Graphics.DrawMeshNow(o.sharedMesh, o.transform.localToWorldMatrix);
            }
        }

        // save a reference to the initial rendertexture where the stencil values have been written
        m_initial = RenderTexture.active;
        // NOTE: if some script renders to this rendertexture before our OnRenderImage call, this effect can break. e.g. if the first ImageEffect script writes to the source in OnRenderImage
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (m_unprocessed != source && m_initial != null) {
            int stencilComp = invert ? (int)CompareFunction.NotEqual : (int)CompareFunction.Equal;
            m_stencilBlitMaterial.SetFloat("_StencilComp", stencilComp);

            // render the processed image (source) on top of the unprocessed image with a stencil test shader
            // use the depth/stencil buffer we saved in OnPostRender
            BlitCopy(m_unprocessed.colorBuffer, m_initial.depthBuffer, source, m_stencilBlitMaterial);
            source = m_unprocessed;
        }

        Graphics.Blit(source, destination);
    }

    #region Utility functions
    private static Material defaultBlitMaterial;

    private static void BlitCopy(RenderBuffer targetColor, RenderBuffer targetDepth, Texture sourceTex) {
        if (defaultBlitMaterial == null) defaultBlitMaterial = new Material(Shader.Find("Hidden/BlitCopy"));
        BlitCopy(targetColor, targetDepth, sourceTex, defaultBlitMaterial);
    }

    // renders sourceTex using specified target buffers
    private static void BlitCopy(RenderBuffer targetColor, RenderBuffer targetDepth, Texture sourceTex, Material mat) {
        Graphics.SetRenderTarget(targetColor, targetDepth);
        mat.SetTexture("_MainTex", sourceTex);

        GL.PushMatrix();
        GL.LoadOrtho();

        // activate the first shader pass (in this case we know it is the only pass)
        mat.SetPass(0);
        // draw a quad over whole screen
        GL.Begin(GL.QUADS);
        GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 0);
        GL.TexCoord2(1, 0); GL.Vertex3(1, 0, 0);
        GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 0);
        GL.TexCoord2(0, 1); GL.Vertex3(0, 1, 0);
        GL.End();

        GL.PopMatrix();
    }

    private static void AddAsFirstCommandBuffer(Camera cam, CameraEvent evt, CommandBuffer buffer) {
        var buffers = cam.GetCommandBuffers(evt);
        cam.RemoveCommandBuffers(evt);
        cam.AddCommandBuffer(evt, buffer);
        foreach (var b in buffers) {
            cam.AddCommandBuffer(evt, b);
        }
    }
    #endregion
}
