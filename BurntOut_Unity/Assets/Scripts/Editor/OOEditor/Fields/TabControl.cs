using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A control that allows selecting a string by index through a tab interface.
    /// </summary>
    public class TabControl
    {
        public List<ToggleButton> tabs = new List<ToggleButton>();
        private int value;
        /// <summary>
        /// Currently selected tab index.
        /// </summary>
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

        /// <summary>
        /// A control that allows selecting a string by index through a tab interface.
        /// </summary>
        /// <param name="value">Initially selected tab.</param>
        /// <param name="tabNames">Names of tabs to display and select from.</param>
        public TabControl(int value, IEnumerable<string> tabNames)
        {
            foreach (var tabName in tabNames)
                AddTab(tabName);
            tabs[Value].Value = true;
            Value = value;
        }

        /// <summary>
        /// Adds a new tab the user can select.
        /// </summary>
        /// <param name="tabName">Name of the new tab.</param>
        public void AddTab(string tabName)
        {
            var newTab = new ToggleButton(false, tabName);
            int tabNum = tabs.Count;
            newTab.Pressed += (o, e) =>
            {
                ((ToggleButton)o).Value = true;
                if (Value != tabNum)
                {
                    Value = tabNum;
                    Changed?.Invoke(this, new ControlChangedArgs<int>(Value, tabNum));
                }
            };
            tabs.Add(newTab);
        }

        /// <summary>
        /// Sets a tab's name at a given index.
        /// </summary>
        /// <param name="index">Index where name should be set.</param>
        /// <param name="tabName">New name to set.</param>
        public void SetTabName(int index, string tabName)
        {
            if (index < 0 || index >= tabs.Count)
            {
                Debug.LogError("Invalid tab index");
                return;
            }

            tabs[index].Content.text = tabName;
        }

        /// <summary>
        /// Removes a tab at the passed index.
        /// </summary>
        /// <param name="index">Index of the tab to remove.</param>
        public void RemoveTab(int index)
        {
            if (index < 0 || index >= tabs.Count)
            {
                Debug.LogError("Invalid tab index");
                return;
            }

            tabs.RemoveAt(index);
            Value = Value;
        }

        /// <summary>
        /// Draws the tab control.
        /// 
        ///   <para>
        ///   If currently in a toolbar, they'll be drawn in the current toolbar. 
        ///   Otherwise, a new toolbar will be created.
        ///   </para>
        /// </summary>
        public void Draw()
        {
            var drawToolbar = !OOEditorManager.InToolbar;
            if (drawToolbar)
                Toolbar.Draw();

            foreach (var tab in tabs)
                tab.Draw();

            if (drawToolbar)
                Toolbar.EndDraw();
        }
        /// <summary>
        /// Updates the control's value and then draws it.
        /// </summary>
        /// <param name="value">Updated value for the control.</param>
        public void Draw(int value)
        {
            Value = value;
            Draw();
        }
    }
}