using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Interact_Patient))]
[CanEditMultipleObjects]
public class InteractPatientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var patient = (Interact_Patient)target;

        var sceneNarrative = GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative;
        var scenario = sceneNarrative.GetScenario(patient.GetComponentInParent<Room>().scenarioId);

        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(target, "Patient change");

        var actorIndex = EditorGUILayout.Popup("Patient", scenario.ActorIndex(patient.patientId), scenario.ActorNames());
        if (actorIndex >= 0)
            patient.patientId = scenario.Actors[actorIndex].id;

        patient.pressButton = (Text)EditorGUILayout.ObjectField("Press Button", patient.pressButton, typeof(Text), true);
        patient.player = (GameObject)EditorGUILayout.ObjectField("Player", patient.player, typeof(GameObject), true);
        patient.gameManager = (Main_GameManager)EditorGUILayout.ObjectField("Game Manager", patient.gameManager, typeof(Main_GameManager), true);
        patient.dialogueManager = (DialogueManager)EditorGUILayout.ObjectField("Dialogue Manage:", patient.dialogueManager, typeof(DialogueManager), true);

        patient.maxAngle = EditorGUILayout.FloatField("Max Angle", patient.maxAngle);
    }
}