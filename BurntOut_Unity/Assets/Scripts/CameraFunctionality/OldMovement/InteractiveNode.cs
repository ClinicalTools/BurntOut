using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveNode : MonoBehaviour {

    public int id;

    public Material fx_MS_Material;
    public Material fx_MT_Material;


    public bool fx_moveStation;
    public bool fx_moveTo;

    [HideInInspector]
    public StationaryMovementNode nextStation;

    private Camera playerCamera;
    private PlayerRotateToTarget myRotateTo;
    private PlayerMoveToTarget myMoveTo;
    private Animator myAnimator;

    public bool mouseHovered;

    void Start() {

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        myMoveTo = playerCamera.GetComponent<PlayerMoveToTarget>();
        myAnimator = gameObject.GetComponent<Animator>();

        if (fx_moveStation)
            this.gameObject.GetComponent<MeshRenderer>().material = fx_MS_Material;
        if (fx_moveTo)
            this.gameObject.GetComponent<MeshRenderer>().material = fx_MT_Material;
        
    }

    public void MoveTo() {
        myMoveTo.target = this.gameObject;
    }

    public void OnMouseOver() {
        myAnimator.enabled = true;
        mouseHovered = true;
    }
    public void OnMouseExit() {
        myAnimator.enabled = false;
        mouseHovered = false;
    }


}
