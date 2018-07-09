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
            var curPositions = Object.FindObjectsOfType<PositionNode>();
            // Use the previous array reference when possible
            if (positions == null || positions.Length != curPositions.Length)
                positions = curPositions;
            else
                for (var i = 0; i < curPositions.Length; i++)
                    positions[i] = curPositions[i];

            if (positionNames == null || positionNames.Length != positions.Length)
                positionNames = new string[positions.Length];

            for (int i = 0; i < positions.Length; i++)
                positionNames[i] = positions[i].name;
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

        public static PositionNode GetPositions(string positionName)
        {
            return Positions.FirstOrDefault(n => n.name == positionName);
        }

        private static string[] positionNames;
        public static string[] PositionNames
        {
            get
            {
                if (positionNames == null)
                    Init();

                return positionNames;
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