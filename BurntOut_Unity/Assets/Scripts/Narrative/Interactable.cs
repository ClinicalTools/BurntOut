using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        private List<Task> events;
        public List<Task> Events
        {
            get
            {
                if (events == null)
                    events = new List<Task>();

                return events;
            }
        }


        // for clicking mechanics
        private void OnMouseUpAsButton()
        {
            if (!Main_GameManager.Instance.isCurrentlyExamine)
                DialogueManager.Instance.ObjectInteract(this);
        }
    }
}