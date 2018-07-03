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

        // General scenario drawer
        private SceneGeneralEditor scenarioGeneralEditor;

        // Choice drawer
        private FoldoutList<Choice, ChoiceEditor> choiceList;

        // Actors drawer
        private SceneActorsEditor sceneActorsEditor;

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
            var oldManger = scenarioManager;
            // If I don't reload this often, the editor will become disconnected from the object after a test play.
            scenarioManager = FindObjectOfType<ScenarioManager>();

            if (oldManger != scenarioManager)
                InitScenarioManager();
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
            }
            else
            {
                scenarioManager = scenarioManagerObj.GetComponent<ScenarioManager>();
            }

            if (scenarioManager == null)
                scenarioManager = scenarioManagerObj.AddComponent<ScenarioManager>();

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

            scenarioGeneralEditor = new SceneGeneralEditor(scenarioManager.scenario);
            scenarioEditor = new ScenarioEditor(scenarioManager.scenario);
            choiceList = new FoldoutList<Choice, ChoiceEditor>(scenarioManager.scenario.Choices);
            sceneActorsEditor = new SceneActorsEditor(scenarioManager.scenario);
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
                if (scenarioManager == null || sceneActorsEditor == null)
                    ResetScenarioManager();

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
                        scenarioGeneralEditor.Draw(scenarioManager.scenario);
                    // Edit scenario
                    else if (tabs.Value == 1)
                        sceneActorsEditor.Draw(scenarioManager.scenario);
                    else if (tabs.Value == 2)
                        choiceList.Draw(scenarioManager.scenario.Choices);
                }
            }
        }
    }
}