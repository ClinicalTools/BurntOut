using UnityEngine;

public class Room : MonoBehaviour
{
    public DialogueManager dialogueManager;
    [HideInInspector]
    public int scenarioId;

    // detect if player is within interact range
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            dialogueManager.StartScenario(
                FindObjectOfType<NarrativeManager>().
                sceneNarrative.GetScenario(scenarioId));
        }

    }
}
