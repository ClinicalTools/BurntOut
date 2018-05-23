using UnityEngine;

public class Room : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public int scenarioId;

    // detect if player is within interact range
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            dialogueManager.StartScenario(
                GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().
                sceneNarrative.GetScenario(scenarioId));
        }

    }
}
