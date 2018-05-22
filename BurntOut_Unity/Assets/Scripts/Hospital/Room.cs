using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public int scenario;



    // detect if player is within interact range
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "Player")
        {
            dialogueManager.StartScenario(GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative.scenarios[scenario]);
        }

    }
}
