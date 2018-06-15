using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using OOEditor;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages editing the scene's scenarios.
    /// </summary>
    public class NarrativeEditorWindow : EditorWindow
    {
        // Scene controls
        private SceneNarrationEditor sceneEditor;
        private readonly List<ScenarioEditor> scenarioEditors = new List<ScenarioEditor>();
        private ScrollView scrollView = new ScrollView();

        private readonly string gameObjectName = "NarrativeManager";

        // Toolbar controls
        private TabControl tabs;
        private IntSlider fontSizeSlider;
        private Button addScenarioBtn, delScenarioBtn,
            saveScenarioBtn, loadScenarioBtn, saveAllBtn, loadAllBtn;
        private readonly OverrideTextStyle toolbarTextStyle = new OverrideTextStyle(12);
        private readonly OverrideTextStyle textStyle = new OverrideTextStyle();

        private int ScenarioNum => tabs.Value - 1;

        public void OnEnable()
        {
            if (sceneNarrative == null)
                ResetSceneManager();

            string[] tabNames = new string[sceneNarrative.scenarios.Count + 1];
            tabNames[0] = "Scene";
            for (int i = 0; i < sceneNarrative.scenarios.Count; i++)
            {
                tabNames[i + 1] = "Scenario " + (i + 1) + " - " + sceneNarrative.scenarios[i].name;
            }
            tabs = new TabControl(0, tabNames);

            fontSizeSlider = new IntSlider(11, 10, 20)
            {
                MaxWidth = 150
            };
            fontSizeSlider.Changed += (object sender, ControlChangedArgs<int> e) =>
            {
                textStyle.FontSize = e.Value;
            };

            addScenarioBtn = new Button("+");
            addScenarioBtn.Pressed += AddScenarioBtn_Pressed;
            delScenarioBtn = new Button("-");
            delScenarioBtn.Pressed += DelScenarioBtn_Pressed;

            saveScenarioBtn = new Button("Save Scenario");
            saveScenarioBtn.Style.FontStyle = FontStyle.Italic;
            saveScenarioBtn.Pressed += SaveScenarioBtn_Pressed;

            loadScenarioBtn = new Button("Load Scenario");
            loadScenarioBtn.Style.FontStyle = FontStyle.Italic;
            loadScenarioBtn.Pressed += LoadScenarioBtn_Pressed;

            saveAllBtn = new Button("Save All");
            saveAllBtn.Style.FontStyle = FontStyle.BoldAndItalic;
            saveAllBtn.Pressed += SaveAllBtn_Pressed;

            loadAllBtn = new Button("Load All");
            loadAllBtn.Style.FontStyle = FontStyle.BoldAndItalic;
            loadAllBtn.Pressed += LoadAllBtn_Pressed;

            sceneEditor = new SceneNarrationEditor(sceneNarrative);
        }

        private void AddScenarioBtn_Pressed(object sender, EventArgs e)
        {
            Scenario scenario = new Scenario(sceneNarrative.scenarios.ToArray());
            sceneNarrative.scenarios.Add(scenario);
            scenarioEditors.Add(new ScenarioEditor(scenario));
            tabs.AddTab("Scenario");
        }
        private void DelScenarioBtn_Pressed(object sender, EventArgs e)
        {
            if (EditorUtility.DisplayDialog("Remove Scenario",
                           "Are you sure you want to delete this scenario?", "Delete", "Cancel"))
            {
                sceneNarrative.scenarios.RemoveAt(ScenarioNum);
                scenarioEditors.RemoveAt(ScenarioNum);
                tabs.RemoveTab(tabs.Value);

                // Ensure there's at least one scenario
                if (sceneNarrative.scenarios.Count == 0)
                {
                    Scenario scenario = new Scenario(sceneNarrative.scenarios.ToArray());
                    sceneNarrative.scenarios.Add(scenario);
                    scenarioEditors.Add(new ScenarioEditor(scenario));
                }
            }
        }

        private void SaveScenarioBtn_Pressed(object sender, EventArgs e)
        {
            NarrativeFileManager.SaveScenario(sceneNarrative.scenarios[ScenarioNum]);
        }

        private void LoadScenarioBtn_Pressed(object sender, EventArgs e)
        {
            var scenario = NarrativeFileManager.LoadScenario(sceneNarrative);
            if (scenario == null)
                return;

            sceneNarrative.scenarios[ScenarioNum] = scenario;
            scenarioEditors[ScenarioNum] = new ScenarioEditor(scenario);
        }

        private void SaveAllBtn_Pressed(object sender, EventArgs e)
        {
            NarrativeFileManager.SaveSceneNarrative(sceneNarrative);
        }
        private void LoadAllBtn_Pressed(object sender, EventArgs e)
        {
            var narrative = NarrativeFileManager.LoadSceneNarrative();
            if (narrative == null)
                return;

            sceneManager.sceneNarrative = narrative;
            OnEnable();
        }

        // Add menu named "Scene Manager" to the Window menu
        [MenuItem("Window/Narrative Manager")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            NarrativeEditorWindow window = GetWindow<NarrativeEditorWindow>("Narrative");
            window.Show();
        }

        private NarrativeManager sceneManager;
        private SceneNarrative sceneNarrative;
        private void ResetSceneManager()
        {
            // If I don't reload this often, the editor will become disconnected from the object after a test play.
            GameObject sceneManagerObj = GameObject.Find(gameObjectName);

            // Initalize if null
            if (sceneManagerObj == null)
            {
                sceneManagerObj = new GameObject(gameObjectName);
                sceneManagerObj.AddComponent<NarrativeManager>();
            }

            sceneManager = sceneManagerObj.GetComponent<NarrativeManager>();
            if (sceneManager == null)
            {
                sceneManagerObj.AddComponent<NarrativeManager>();
                sceneManager = sceneManagerObj.GetComponent<NarrativeManager>();
            }
            sceneNarrative = sceneManager.sceneNarrative;
        }

        void OnInspectorUpdate()
        {
            var oldSceneNarrative = sceneNarrative;
            ResetSceneManager();
            if (oldSceneNarrative != sceneNarrative)
                OnEnable();
        }


        void OnGUI()
        {
            if (sceneNarrative == null)
            {
                ResetSceneManager();
                OnEnable();
            }

            // Allows the scene to save changes and 'undo' to be possible
            Undo.RecordObject(sceneManager, "NarrativeManager change");

            // Ensure there's at least one scenario
            if (sceneNarrative.scenarios.Count == 0)
            {
                tabs.AddTab("Scenario");
                sceneNarrative.scenarios.Add(
                    new Scenario(sceneNarrative.scenarios.ToArray()));
            }

            // Ensure scenarios in the editor match the scenarios in the scene
            if (scenarioEditors.Count != sceneNarrative.scenarios.Count)
            {
                scenarioEditors.Clear();
                foreach (Scenario scenario in sceneNarrative.scenarios)
                    scenarioEditors.Add(new ScenarioEditor(scenario));
            }

            /*
            // Draw the toolbar for scenario management
            using (Toolbar.Draw())
            using (toolbarTextStyle.Draw())
            {
                for (int i = 0; i < sceneNarrative.scenarios.Count; i++)
                    tabs.SetTabName(i + 1, "Scenario " + (i + 1) + " - " + sceneNarrative.scenarios[i].name);
                tabs.Draw();

                FlexibleSpace.Draw();

                fontSizeSlider.Draw();

                addScenarioBtn.Draw();
                delScenarioBtn.Draw();

                if (tabs.Value > 0)
                {
                    saveScenarioBtn.Draw();
                    loadScenarioBtn.Draw();
                }
                saveAllBtn.Draw();
                loadAllBtn.Draw();
            }

            using (textStyle.Draw())
            using (scrollView.Draw())//*/
            {
                // Edit basic scene info
                if (tabs.Value == 0)
                    sceneEditor.Draw(sceneNarrative);
                // Edit selected scenario
                else
                    scenarioEditors[ScenarioNum].Draw(sceneNarrative.scenarios[ScenarioNum]);
            }
        }
    }
}