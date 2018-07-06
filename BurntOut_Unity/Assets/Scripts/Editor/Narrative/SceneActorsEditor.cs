using OOEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a scenario.
    /// </summary>
    public class SceneActorsEditor : ClassDrawer<Scenario>
    {
        private readonly Button editActorsBtn;

        public SceneActorsEditor(Scenario value) : base(value)
        {
            editActorsBtn = new Button("Edit Actor Prefabs");
            editActorsBtn.Style.FontSize = 14;
            editActorsBtn.Style.FontStyle = FontStyle.Bold;
            editActorsBtn.Pressed += (sender, e) =>
            {
                ActorPrefabsEditorWindow.Init();
            };
        }

        protected override void Display()
        {
            editActorsBtn.Draw();
        }
    }
}