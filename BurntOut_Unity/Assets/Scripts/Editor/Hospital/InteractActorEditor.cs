using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractActor))]
[CanEditMultipleObjects]
public class InteractActorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        var patient = (InteractActor)target;

        var sceneNarrative = GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative;
        var scenario = sceneNarrative.GetScenario(patient.GetComponentInParent<Room>().scenarioId);

        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(target, "Patient change");

        if (scenario != null)
        {
            var actorIndex = EditorGUILayout.Popup("Patient", scenario.ActorIndex(patient.actorId), scenario.ActorNames());
            if (actorIndex >= 0)
                patient.actorId = scenario.Actors[actorIndex].id;
        }
    }
}