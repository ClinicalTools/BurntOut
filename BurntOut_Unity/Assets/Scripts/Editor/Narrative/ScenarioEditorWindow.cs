using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using OOEditor;
using System;
using Narrative.Vars;
using Narrative.Vars.Inspector;

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

        private enum ScenarioTab
        {
            General, Variables, Actors, Interactables, Events
        }

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

        // Vars drawer
        private ReorderableList<NarrativeVar, NarrativeVarEditor> narrativeVars;

        // Actors drawer
        private SceneActorsEditor sceneActorsEditor;

        // Interactables drawer
        private List<Interactable> interactables;
        private FoldoutList<Interactable, InteractableEditor> interactablesList;

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
                scenarioManager.Scenario = new Scenario();

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

            EditorApplication.playModeStateChanged += (stateChange) =>
            {
                scenarioManager = FindObjectOfType<ScenarioManager>();
                InitScenarioControls();
            };
        }

        private void InitScenarioControls()
        {
            if (scenarioManager?.Scenario == null)
            {
                //Debug.LogError("Scenario Manager and its scenario should not be null.");
                return;
            }

            tabs = new TabControl(0, Enum.GetNames(typeof(ScenarioTab)));

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

            narrativeVars = new ReorderableList<NarrativeVar, NarrativeVarEditor>(scenarioManager.Scenario.Vars);

            var interactables = new List<Interactable>(SceneInteractables.Interactables);
            interactablesList = new FoldoutList<Interactable, InteractableEditor>(interactables, false, false, false);
        }

        private void SaveBtn_Pressed(object sender, EventArgs e)
        {
            NarrativeFileManager.SaveScenario(scenarioManager.Scenario);
        }

        private void LoadBtn_Pressed(object sender, EventArgs e)
        {
            var scenario = NarrativeFileManager.LoadScenario();
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

                if (scenarioManager == null || !scenarioManager.isActiveAndEnabled)
                {
                    initButton.Draw();
                    return;
                }

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
                    switch ((ScenarioTab)tabs.Value)
                    {
                        case ScenarioTab.General:
                            scenarioGeneralEditor.Draw(scenarioManager.Scenario);
                            break;
                        case ScenarioTab.Variables:
                            narrativeVars.Draw(scenarioManager.Scenario.Vars);
                            break;
                        case ScenarioTab.Actors:
                            sceneActorsEditor.Draw(scenarioManager.Scenario);
                            break;
                        case ScenarioTab.Interactables:
                            var interactables = new List<Interactable>(SceneInteractables.Interactables);
                            interactablesList.Draw(interactables);
                            break;
                        case ScenarioTab.Events:
                            choiceList.Draw(scenarioManager.Scenario.Choices);
                            break;

                    }
                }
            }
        }
    }
}