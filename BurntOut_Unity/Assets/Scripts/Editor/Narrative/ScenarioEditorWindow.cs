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
        private const int DEFAULT_FONT_SIZE = 11;
        private const int MIN_FONT_SIZE = 10;
        private const int MAX_FONT_SIZE = 20;
        private const int TOOLBAR_FONT_SIZE = 12;

        // Scene controls
        private ScrollView scrollView = new ScrollView();

        private readonly string gameObjectName = "ScenarioManager";

        private Button initButton;
        // Toolbar controls
        private TabControl tabs;
        private IntSlider fontSizeSlider;
        private Button loadBtn, saveBtn;
        private readonly OverrideTextStyle toolbarTextStyle = new OverrideTextStyle(TOOLBAR_FONT_SIZE);
        private readonly OverrideTextStyle textStyle = new OverrideTextStyle(DEFAULT_FONT_SIZE);

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

            if (scenarioManager.Scenario == null)
                scenarioManager.Scenario = new Scenario(new Scenario[0]);

            InitScenarioControls();
        }

        private void OnInspectorUpdate()
        {
            ResetScenarioManager();
            Repaint();
        }

        private void OnEnable()
        {
            initButton = new Button("Create Scenario Manager", "Creates a scenario manager for the current scene");
            initButton.Pressed += (o, sender) =>
            {
                InitScenarioManager();
            };
        }

        private void InitScenarioControls()
        {
            if (scenarioManager?.Scenario == null)
            {
                Debug.LogError("Scenario Manager and its scenario should not be null.");
                return;
            }

            string[] tabNames = { "General", "Actors", "Events" };
            tabs = new TabControl(0, tabNames);

            fontSizeSlider = new IntSlider(DEFAULT_FONT_SIZE, MIN_FONT_SIZE, MAX_FONT_SIZE)
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

            scenarioGeneralEditor = new SceneGeneralEditor(scenarioManager.Scenario);
            choiceList = new FoldoutList<Choice, ChoiceEditor>(scenarioManager.Scenario.Choices);
            sceneActorsEditor = new SceneActorsEditor(scenarioManager.Scenario);
        }

        private void SaveBtn_Pressed(object sender, EventArgs e)
        {
            NarrativeFileManager.SaveScenario(scenarioManager.Scenario);
        }

        private void LoadBtn_Pressed(object sender, EventArgs e)
        {
            var scenario = NarrativeFileManager.LoadScenario(new List<Scenario>());
            if (scenario == null)
                return;

            scenarioManager.Scenario = scenario;
        }

        void OnGUI()
        {
            if (scenarioManager?.Scenario == null)
            {
                initButton.Draw();
            }
            else
            {
                if (scenarioManager == null)
                    ResetScenarioManager();

                if (scenarioGeneralEditor == null || choiceList == null || sceneActorsEditor == null)
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
                        scenarioGeneralEditor.Draw(scenarioManager.Scenario);
                    // Edit scenario
                    else if (tabs.Value == 1)
                        sceneActorsEditor.Draw(scenarioManager.Scenario);
                    else if (tabs.Value == 2)
                        choiceList.Draw(scenarioManager.Scenario.Choices);
                }
            }
        }
    }
}