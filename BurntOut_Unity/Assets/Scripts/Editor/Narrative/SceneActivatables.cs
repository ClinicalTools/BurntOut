using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public static class SceneActivatables
    {
        public const string ACTIVATABLE_TAG = "Activatable";

        private static void Init()
        {
            EditorApplication.hierarchyChanged += ResetActivatables;

            ResetActivatables();
        }

        private static void ResetActivatables()
        {
            var curActivatables = SceneObjects.FindGameObjectsWithTag(ACTIVATABLE_TAG);
            // Use the previous array reference when possible
            if (activatables == null || activatables.Length != curActivatables.Length)
                activatables = curActivatables;
            else
                for (var i = 0; i < curActivatables.Length; i++)
                    activatables[i] = curActivatables[i];

            if (names == null || names.Length != activatables.Length)
                names = new string[activatables.Length];

            for (int i = 0; i < activatables.Length; i++)
                names[i] = activatables[i].name;
        }

        private static GameObject[] activatables;
        public static GameObject[] Activatables
        {
            get
            {
                if (activatables == null)
                    Init();

                return activatables;
            }
        }

        public static GameObject GetActivatable(string objectName)
        {
            return Activatables.FirstOrDefault(o => o.name == objectName);
        }

        private static string[] names;
        public static string[] Names
        {
            get
            {
                if (names == null)
                    Init();

                return names;
            }
        }

        public static int GetIndex(GameObject activatable)
        {
            var index = -1;
            for (int i = 0; i < Activatables.Length; i++)
                if (Activatables[i] == activatable)
                    index = i;

            return index;
        }
    }
}