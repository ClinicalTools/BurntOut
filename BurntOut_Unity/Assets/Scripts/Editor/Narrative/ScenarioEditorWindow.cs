using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using OOEditor;
using System;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages editing the scene's scenarios.
    /// </summary>
    public class ScenarioEditorWindow : EditorWindow
    {
        // Scene controls
        private ScenarioEditor scenarioEditor;
        private ScrollView scrollView = new ScrollView();

        private readonly string gameObjectName = "ScenarioManager";

        private Button initButton;
        // Toolbar controls
        private TabControl tabs;
        private IntSlider fontSizeSlider;
        private Button loadBtn, saveBtn;
        private readonly OverrideTextStyle toolbarTextStyle = new OverrideTextStyle(12);
        private readonly OverrideTextStyle textStyle = new OverrideTextStyle();
        private DropdownMenuButton actorSelectButton;

        // Actor drawer
        private ActorPopupWindow actorDrawer;

        private static ScenarioEditorWindow window;
        // Add menu named "Scene Manager" to the Window menu
        [MenuItem("Window/Scenario Manager")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            window = GetWindow<ScenarioEditorWindow>("Scenario");
            window.Show();
        }

        private ScenarioManager scenarioManager;

        private void ResetScenarioManager()
        {
            // If I don't reload this often, the editor will become disconnected from the object after a test play.
            scenarioManager = FindObjectOfType<ScenarioManager>();
        }

        private void InitScenarioManager()
        {
            // If I don't reload this often, the editor will become disconnected from the object after a test play.
            GameObject scenarioManagerObj = GameObject.Find(gameObjectName);

            // Initalize if null
            if (scenarioManagerObj == null)
            {
                scenarioManagerObj = new GameObject(gameObjectName);
                Undo.RegisterCreatedObjectUndo(scenarioManagerObj, "Created scenario manager");
                scenarioManager = null;
                scenarioManagerObj.AddComponent<ScenarioManager>();
            }
            else
            {
                scenarioManager = scenarioManagerObj.GetComponent<ScenarioManager>();
            }
            if (scenarioManager == null)
            {
                scenarioManagerObj.AddComponent<ScenarioManager>();
                scenarioManager = scenarioManagerObj.GetComponent<ScenarioManager>();
            }

            if (scenarioManager.scenario == null)
                scenarioManager.scenario = new Scenario(new Scenario[0]);

            InitScenarioControls();
        }

        void OnInspectorUpdate()
        {
            ResetScenarioManager();
            Repaint();
        }

        public void OnEnable()
        {
            initButton = new Button("Create Scenario Manager", "Creates a scenario manager for the current scene");
            initButton.Pressed += (o, sender) =>
            {
                InitScenarioManager();
            };

        }

        private void test()
        {
        }

        private void InitScenarioControls()
        {
            if (scenarioManager?.scenario == null)
            {
                Debug.LogError("Scenario Manager and its scenario should not be null.");
                return;
            }

            string[] tabNames = { "General", "Actors", "Events" };
            tabs = new TabControl(0, tabNames);

            fontSizeSlider = new IntSlider(11, 10, 20)
            {
                MaxWidth = 150
            };
            fontSizeSlider.Changed += (sender, e) =>
            {
                textStyle.FontSize = e.Value;
            };

            saveBtn = new Button("Save");
            saveBtn.Style.FontStyle = FontStyle.Bold;
            saveBtn.Pressed += SaveBtn_Pressed;

            loadBtn = new Button("Load");
            loadBtn.Style.FontStyle = FontStyle.Bold;
            loadBtn.Pressed += LoadBtn_Pressed;

            scenarioEditor = new ScenarioEditor(scenarioManager.scenario);

            actorDrawer = new ActorPopupWindow(scenarioManager.scenario.Actors);
            actorSelectButton = new DropdownMenuButton(
                ActorPopupWindow.ActorMenu(scenarioManager.scenario.Actors), "Actor stuff");
        }

        private void SaveBtn_Pressed(object sender, EventArgs e)
        {
            NarrativeFileManager.SaveScenario(scenarioManager.scenario);
        }

        private void LoadBtn_Pressed(object sender, EventArgs e)
        {
            var scenario = NarrativeFileManager.LoadScenario(new List<Scenario>());
            if (scenario == null)
                return;

            scenarioManager.scenario = scenario;
        }

        void OnGUI()
        {
            if (scenarioManager?.scenario == null)
            {
                initButton.Draw();
            }
            else
            {
                if (scenarioEditor == null)
                    InitScenarioControls();

                // Allows the scene to save changes and 'undo' to be possible
                Undo.RecordObject(scenarioManager, "ScenarioManager change");

                // Draw the toolbar for scenario management
                using (Toolbar.Draw())
                using (toolbarTextStyle.Draw())
                {
                    tabs.Draw();

                    FlexibleSpace.Draw();

                    fontSizeSlider.Draw();

                    saveBtn.Draw();
                    loadBtn.Draw();
                }

                using (textStyle.Draw())
                using (scrollView.Draw())
                {
                    // Edit basic scene info
                    if (tabs.Value == 0)
                        scenarioEditor.Draw(scenarioManager.scenario);
                    // Edit scenario
                    else
                        actorDrawer.Draw(scenarioManager.scenario.Actors);
                        //actorSelectButton.Draw();
                }
            }
        }
    }
}