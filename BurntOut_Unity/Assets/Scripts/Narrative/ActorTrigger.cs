using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    public class ActorTrigger : MonoBehaviour
    {
        public ActorObject actor;

        // for clicking mechanics
        private void OnMouseUpAsButton()
        {
            if (!Main_GameManager.Instance.isCurrentlyExamine)
                DialogueManager.Instance.ActorInteract(actor);
        }
    }
}