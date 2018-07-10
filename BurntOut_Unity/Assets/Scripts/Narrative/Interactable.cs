using UnityEngine;

namespace Narrative
{
    public class Interactable : MonoBehaviour
    {
        // for clicking mechanics
        private void OnMouseUpAsButton()
        {
            if (!Main_GameManager.Instance.isCurrentlyExamine)
                ScenarioDialogueManager.Instance.ObjectInteract(this);
        }
    }
}