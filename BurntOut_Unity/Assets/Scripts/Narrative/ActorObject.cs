using UnityEngine;

namespace Narrative
{
    public class ActorObject : MonoBehaviour
    {
        public Actor actor;

        // for clicking mechanics
        private void OnMouseUpAsButton()
        {
            if (!Main_GameManager.Instance.isCurrentlyExamine)
                ScenarioDialogueManager.Instance.ActorInteract(this);
        }
    }
}