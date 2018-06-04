using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Used by GUI elements that contain a label.
    /// </summary>
    internal interface IField
    {
        GUIContent Content { get; set; }
    }
}