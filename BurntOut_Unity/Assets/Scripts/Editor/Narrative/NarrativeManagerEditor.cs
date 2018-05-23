using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NarrativeManager))]
public class NarrativeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Narrative Manager Editor"))
        {
            NarrativeEditorWindow.Init();
        }
    }
}