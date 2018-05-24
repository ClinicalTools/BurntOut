using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(InteractPatient))]
[CanEditMultipleObjects]
public class InteractPatientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var patient = (InteractPatient)target;

        var sceneNarrative = GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative;
        var scenario = sceneNarrative.GetScenario(patient.GetComponentInParent<Room>().scenarioId);

        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(target, "Patient change");

        var actorIndex = EditorGUILayout.Popup("Patient", scenario.ActorIndex(patient.patientId), scenario.ActorNames());
        if (actorIndex >= 0)
            patient.patientId = scenario.Actors[actorIndex].id;

        patient.interactPrompt = (Text)EditorGUILayout.ObjectField("Interact Prompt", patient.interactPrompt, typeof(Text), true);
        patient.player = (GameObject)EditorGUILayout.ObjectField("Player", patient.player, typeof(GameObject), true);
        patient.gameManager = (Main_GameManager)EditorGUILayout.ObjectField("Game Manager", patient.gameManager, typeof(Main_GameManager), true);
        patient.dialogueManager = (DialogueManager)EditorGUILayout.ObjectField("Dialogue Manager", patient.dialogueManager, typeof(DialogueManager), true);
        patient.playerRotateToTarget = (PlayerRotateToTarget)EditorGUILayout.ObjectField("Player Rotate", patient.playerRotateToTarget, typeof(PlayerRotateToTarget), true);

        patient.maxAngle = EditorGUILayout.FloatField("Max Angle", patient.maxAngle);
    }
}