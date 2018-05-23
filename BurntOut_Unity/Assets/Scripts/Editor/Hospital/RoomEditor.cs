using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room))]
[CanEditMultipleObjects]
public class RoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var room = (Room)target;


        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(target, "Room change");

        room.dialogueManager = (DialogueManager) EditorGUILayout.ObjectField(room.dialogueManager, typeof(DialogueManager), true);
        var sceneNarrative = GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative;

        var scenarioIndex = EditorGUILayout.Popup(sceneNarrative.ScenarioIndex(room.scenarioId), sceneNarrative.ScenarioNames());
        if (scenarioIndex >= 0)
            room.scenarioId = sceneNarrative.scenarios[scenarioIndex].id;
    }
}