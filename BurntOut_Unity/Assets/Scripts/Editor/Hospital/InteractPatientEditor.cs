using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractPatient))]
[CanEditMultipleObjects]
public class InteractPatientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        var patient = (InteractPatient)target;

        var sceneNarrative = GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative;
        var scenario = sceneNarrative.GetScenario(patient.GetComponentInParent<Room>().scenarioId);

        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(target, "Patient change");

        var actorIndex = EditorGUILayout.Popup("Patient", scenario.ActorIndex(patient.patientId), scenario.ActorNames());
        if (actorIndex >= 0)
            patient.patientId = scenario.Actors[actorIndex].id;
    }
}