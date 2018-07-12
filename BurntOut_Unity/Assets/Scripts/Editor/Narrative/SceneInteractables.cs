using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public static class SceneInteractables
    {
        private static void Init()
        {
            EditorApplication.hierarchyChanged += ResetInteractables;

            ResetInteractables();
        }

        private static void ResetInteractables()
        {
            var curInteractables = Object.FindObjectsOfType<Interactable>();
            // Use the previous array reference when possible
            if (interactables == null || interactables.Length != curInteractables.Length)
                interactables = curInteractables;
            else
                for (var i = 0; i < curInteractables.Length; i++)
                    interactables[i] = curInteractables[i];

            if (interactableNames == null || interactableNames.Length != interactables.Length)
                interactableNames = new string[interactables.Length];

            for (int i = 0; i < interactables.Length; i++)
                interactableNames[i] = interactables[i].name;
        }

        private static Interactable[] interactables;
        public static Interactable[] Interactables
        {
            get
            {
                if (interactables == null || (interactables.Length > 0 && interactables[0] == null))
                    Init();

                return interactables;
            }
        }

        public static Interactable GetInteractable(string name)
        {
            return Interactables.FirstOrDefault(n => n.name == name);
        }

        private static string[] interactableNames;
        public static string[] Names
        {
            get
            {
                if (interactableNames == null || (interactables.Length > 0 && interactables[0] == null))
                    Init();

                return interactableNames;
            }
        }

        public static int GetIndex(Interactable interactable)
        {
            var index = -1;
            for (int i = 0; i < Interactables.Length; i++)
                if (Interactables[i] == interactable)
                    index = i;

            return index;
        }
    }
}