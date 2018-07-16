using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public static class ScenePositions
    {
        private static void Init()
        {
            EditorApplication.hierarchyChanged += ResetPositions;

            ResetPositions();
        }

        private static void ResetPositions()
        {
            var curPositions = SceneObjects.FindObjectsOfType<PositionNode>();
            // Use the previous array reference when possible
            if (positions == null || positions.Length != curPositions.Length)
                positions = curPositions;
            else
                for (var i = 0; i < curPositions.Length; i++)
                    positions[i] = curPositions[i];

            if (names == null || names.Length != positions.Length)
                names = new string[positions.Length];

            for (int i = 0; i < positions.Length; i++)
                names[i] = positions[i].name;
        }

        private static PositionNode[] positions;
        public static PositionNode[] Positions
        {
            get
            {
                if (positions == null)
                    Init();

                return positions;
            }
        }

        public static PositionNode GetPosition(string name)
        {
            return Positions.FirstOrDefault(n => n.name == name);
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

        public static int GetIndex(PositionNode position)
        {
            var index = -1;
            for (int i = 0; i < Positions.Length; i++)
                if (Positions[i] == position)
                    index = i;

            return index;
        }
    }
}