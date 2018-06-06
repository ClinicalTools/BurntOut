using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using CtiEditor;
using OOEditor;

/// <summary>
/// Manages editing the scene's scenarios.
/// </summary>
public class NarrativeEditorWindow : EditorWindow
{
    private readonly List<ScenarioEditor> scenarioEditors = new List<ScenarioEditor>();
    private int selectedScenario = -1;
    private Vector2 scrollPosition = new Vector2();
    private float scale = 11;
    private float sceneTextAreaWidth = 0;

    private readonly string gameObjectName = "NarrativeManager";
    private ToggleButton sceneTab;
    private List<ToggleButton> scenarioTabs = new List<ToggleButton>();
    private IntSlider fontSizeSlider = new IntSlider(11, 10, 20)
    {
        MaxWidth = 150
    };
    private Button addScenarioBtn = new Button("+");
    private Button delScenarioBtn = new Button("-");

    private Button saveScenarioBtn = new Button("Save Scenario");
    private Button loadScenarioBtn = new Button("Load Scenario");
    private Button saveAllBtn = new Button("Save All");
    private Button loadAllBtn = new Button("Load All");


    public void OnEnable()
    {
        if (sceneManager == null)
            ResetSceneManager();

        sceneTab = new ToggleButton(false, "Scene");
        sceneTab.Changed += (object sender, EventArgs e) =>
        {
            var tab = (ToggleButton)sender;
            tab.Value = true;

            selectedScenario = -1;
            foreach (var scenarioTab in scenarioTabs)
                scenarioTab.Value = false;
        };

        saveScenarioBtn.Style.FontStyle = FontStyle.Italic;
        loadScenarioBtn.Style.FontStyle = FontStyle.Italic;
        saveAllBtn.Style.FontStyle = FontStyle.BoldAndItalic;
        loadAllBtn.Style.FontStyle = FontStyle.BoldAndItalic;
    }

    private void AddScenarioBtn_Changed(object sender, EventArgs e)
    {
        throw new NotImplementedException();
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
        var sceneManager = sceneManagerObj.GetComponent<NarrativeManager>();
        if (sceneManager == null)
        {
            sceneManagerObj.AddComponent<NarrativeManager>();
            sceneManager = sceneManagerObj.GetComponent<NarrativeManager>();
        }
    }

    void OnInspectorUpdate()
    {
        var oldSceneManager = sceneManager;
        ResetSceneManager();
        if (oldSceneManager != sceneManager)
            OnEnable();
    }


    void OnGUI()
    {
        if (sceneManager == null)
        { 
            ResetSceneManager();
            OnEnable();
        }



        SceneNarrative sceneNarrative = sceneManager.sceneNarrative;

        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(sceneManager, "NarrativeManager change");

        // Ensure there's at least one scenario
        if (sceneNarrative.scenarios.Count == 0)
            sceneNarrative.scenarios.Add(
                new Scenario(sceneNarrative.scenarios.ToArray()));

        // Ensure scenarios in the editor match the scenarios in the scene
        if (scenarioEditors.Count != sceneNarrative.scenarios.Count ||
            scenarioEditors[0].scenario != sceneNarrative.scenarios[0])
        {
            scenarioEditors.Clear();
            foreach (Scenario scenario in sceneNarrative.scenarios)
                scenarioEditors.Add(new ScenarioEditor(scenario));
        }

        // Draw the toolbar for scenario management
        using (new Toolbar())
        using (var toolbarStyle = new OverrideTextStyle())
        {
            toolbarStyle.FontSize = 12;
            
            sceneTab.Draw();

            for (int i = 0; i < sceneNarrative.scenarios.Count; i++)
            {
                if (scenarioTabs.Count <= i)
                    scenarioTabs.Add(new ToggleButton(false, "Scenario " + (i + 1) + " - " + sceneNarrative.scenarios[i].name));

                var tab = scenarioTabs[i];
                tab.Content.text = "Scenario " + (i + 1) + " - " + sceneNarrative.scenarios[i].name;
                tab.Value = (selectedScenario == i);
                tab.Draw();
                if (tab.Value)
                    selectedScenario = i;
            }

            GUILayout.FlexibleSpace();

            fontSizeSlider.Draw();
            scale = fontSizeSlider.Value;

            addScenarioBtn.Draw();
            if (addScenarioBtn.Value)
            {
                Scenario scenario = new Scenario(sceneNarrative.scenarios.ToArray());
                sceneNarrative.scenarios.Add(scenario);
                scenarioEditors.Add(new ScenarioEditor(scenario));
            }

            delScenarioBtn.Draw();
            if (delScenarioBtn.Value)
            {
                if (EditorUtility.DisplayDialog("Remove Scenario",
                    "Are you sure you want to delete this scenario?", "Delete", "Cancel"))
                {
                    sceneNarrative.scenarios.RemoveAt(selectedScenario);
                    scenarioEditors.RemoveAt(selectedScenario);
                    if (selectedScenario > 0)
                        selectedScenario--;

                    // Ensure there's at least one scenario
                    if (sceneNarrative.scenarios.Count == 0)
                    {
                        Scenario scenario = new Scenario(sceneNarrative.scenarios.ToArray());
                        sceneNarrative.scenarios.Add(scenario);
                        scenarioEditors.Add(new ScenarioEditor(scenario));
                    }
                }
            }

            if (selectedScenario > -1)
            {
                saveScenarioBtn.Draw();
                if (saveScenarioBtn.Value)
                {
                    // Get the folder to save the scenario in
                    string json = JsonUtility.ToJson(sceneNarrative.scenarios[selectedScenario]);
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    path += "\\BurntOut\\Narrative\\" + SceneManager.GetActiveScene().name + "\\Scenario";
                    Directory.CreateDirectory(path);

                    // Name.yyyy.MM.dd.letter.json
                    string fileName = sceneNarrative.scenarios[selectedScenario].name + DateTime.Now.ToString("'.'yyyy'.'MM'.'dd");

                    // Alphabetical character based on number of similar files saved today
                    char lastLetter = 'a';
                    foreach (var file in Directory.GetFiles(path))
                    {
                        if (file.Contains(fileName) && file.EndsWith(".json"))
                        {
                            string str = Path.GetFileName(file);
                            str = str.Replace(fileName + '.', "");
                            if (str[0] >= lastLetter)
                                lastLetter = (char)(str[0] + 1);
                        }
                    }
                    fileName += "." + lastLetter + ".json";

                    path = EditorUtility.SaveFilePanel("Save Scenario", path, fileName, "json");

                    if (!String.IsNullOrEmpty(path))
                        File.WriteAllText(path, json);
                }
                loadScenarioBtn.Draw();
                if (loadScenarioBtn.Value)
                {
                    // Get the folder to load the scenario from
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    path += "\\BurntOut\\Narrative\\" + SceneManager.GetActiveScene().name + "\\Scenario";

                    path = EditorUtility.OpenFilePanel("Load Scenario", path, "json");

                    if (!String.IsNullOrEmpty(path))
                    {
                        string json = File.ReadAllText(path);

                        var scenario = JsonUtility.FromJson<Scenario>(json);
                        for (int i = 0; i < sceneNarrative.scenarios.Count; i++)
                            if (i != selectedScenario && scenario.id == sceneNarrative.scenarios[i].id)
                                scenario.ResetHash(sceneNarrative.scenarios.ToArray());

                        sceneNarrative.scenarios[selectedScenario] = scenario;
                        scenarioEditors[selectedScenario] = new ScenarioEditor(scenario);
                    }
                }
            }
            saveAllBtn.Draw();
            if (saveAllBtn.Value)
            {
                // Get the folder to save the scene in
                string json = JsonUtility.ToJson(sceneNarrative);
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path += "\\BurntOut\\Narrative\\" + SceneManager.GetActiveScene().name;
                Directory.CreateDirectory(path);

                // Name.yyyy.MM.dd.letter.json
                string fileName = SceneManager.GetActiveScene().name + DateTime.Now.ToString("'.'yyyy'.'MM'.'dd");

                // Alphabetical character based on number of similar files saved today
                char lastLetter = 'a';
                foreach (var file in Directory.GetFiles(path))
                {
                    if (file.Contains(fileName) && file.EndsWith(".json"))
                    {
                        string str = Path.GetFileName(file);
                        str = str.Replace(fileName + '.', "");
                        if (str[0] >= lastLetter)
                            lastLetter = (char)(str[0] + 1);
                    }
                }
                fileName += "." + lastLetter + ".json";

                path = EditorUtility.SaveFilePanel("Save Scene", path, fileName, "json");

                if (!String.IsNullOrEmpty(path))
                    File.WriteAllText(path, json);
            }
            loadAllBtn.Draw();
            if (loadAllBtn.Value)
            {
                // Get the folder to load the scene from
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path += "\\BurntOut\\Narrative\\" + SceneManager.GetActiveScene().name;

                path = EditorUtility.OpenFilePanel("Load Scene", path, "json");

                if (!String.IsNullOrEmpty(path))
                {
                    string json = File.ReadAllText(path);

                    var narrative = JsonUtility.FromJson<SceneNarrative>(json);
                    sceneManager.sceneNarrative = narrative;
                }
            }
        }

        using (CtiEditorGUI.FontSize((int)scale))
        {
            using (CtiEditorGUI.ScrollView(ref scrollPosition))
            {
                // Edit basic scene info
                if (selectedScenario == -1)
                {
                    using (CtiEditorGUI.LabelFontStyle(FontStyle.Bold))
                    {
                        CtiEditorGUI.LabelField("Start Narration:");
                        sceneNarrative.startNarration = CtiEditorGUI.TextArea(sceneNarrative.startNarration, ref sceneTextAreaWidth);

                        CtiEditorGUI.LabelField("");

                        CtiEditorGUI.LabelField("End Narration:");
                        sceneNarrative.endNarration = CtiEditorGUI.TextArea(sceneNarrative.endNarration, ref sceneTextAreaWidth);
                    }
                }
                // Edit selected scenario
                else
                {
                    scenarioEditors[selectedScenario].Edit();
                }
            }
        }
    }
}