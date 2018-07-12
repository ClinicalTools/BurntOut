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
        private ReorderableList<Task, TaskEditor> tasks;

        public void OnEnable()
        {
            interactable = (Interactable)target;

            tasks = new ReorderableList<Task, TaskEditor>(interactable.Events);
        }

        public override void OnInspectorGUI()
        {
            tasks.Draw();
        }
    }
}