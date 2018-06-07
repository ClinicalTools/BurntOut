using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractiveNode))]
[CanEditMultipleObjects]

public class InteractiveNodeEditor : Editor {

    public override void OnInspectorGUI() {

        // Show default inspector property editor
        DrawDefaultInspector();

        var node = (InteractiveNode)target;

        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(target, "Next Station");

        if (node.fx_moveStation) {
            node.nextStation = (StationaryMovementNode) EditorGUILayout.ObjectField("Next Station", node.nextStation, typeof(StationaryMovementNode),true);
        }
            
    }
}
