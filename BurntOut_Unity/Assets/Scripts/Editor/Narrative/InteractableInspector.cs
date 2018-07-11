using OOEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    [CustomEditor(typeof(Interactable))]
    public class InteractableInspector : Editor
    {
        private Interactable interactable;
        private ReorderableList<Task, TaskDrawer> tasks;

        public void OnEnable()
        {
            interactable = (Interactable)target;

            tasks = new ReorderableList<Task, TaskDrawer>(interactable.Events);
        }

        public override void OnInspectorGUI()
        {
            tasks.Draw();
        }
    }
}