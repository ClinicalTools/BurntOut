using OOEditor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class TabControl
    {
        public List<ToggleButton> tabs = new List<ToggleButton>();
        private int value;
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value < -1)
                    value = 0;
                if (value >= tabs.Count)
                    value = tabs.Count - 1;

                this.value = value;

                for (var i = 0; i < tabs.Count; i++)
                    tabs[i].Value = (value == i);
            }
        }

        public event EventHandler<ControlChangedArgs<int>> Changed;

        public TabControl(int value, string[] tabNames)
        {
            foreach (var tabName in tabNames)
                AddTab(tabName);
            tabs[Value].Value = true;
            Value = value;
        }

        public void AddTab(string tabName)
        {
            var newTab = new ToggleButton(false, tabName);
            int tabNum = tabs.Count;
            newTab.Changed += (object o, ControlChangedArgs<bool> e) =>
            {
                if (e.Value == false)
                    Changed?.Invoke(this, new ControlChangedArgs<int>(Value, tabNum));

                Value = tabNum;
            };
            tabs.Add(newTab);
        }

        public void SetTabName(int index, string tabName)
        {
            tabs[index].Content.text = tabName;
        }

        public void RemoveTab(int index)
        {
            tabs.RemoveAt(index);
            Value = Value;
        }

        public void Draw()
        {
            Toolbar toolbar = null;
            if (OOEditorManager.InToolbar == 0)
                toolbar = new Toolbar();

            foreach (var tab in tabs)
                tab.Draw();

            if (toolbar != null)
                toolbar.Dispose();
        }

    }
}