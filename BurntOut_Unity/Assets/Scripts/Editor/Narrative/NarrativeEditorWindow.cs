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

    // Toolbar controls
    private ToggleButton sceneTab;
    private List<ToggleButton> scenarioTabs = new List<ToggleButton>();
    private IntSlider fontSizeSlider;
    private Button addScenarioBtn;
    private Button delScenarioBtn;

    private Button saveScenarioBtn;
    private Button loadScenarioBtn; 
    private Button saveAllBtn;
    private Button loadAllBtn;

    // Scene controls
    SceneNarrationEditor sceneEditor;

    public void OnEnable()
    {
        if (sceneNarrative == null)
            ResetSceneManager();

        sceneTab = new ToggleButton(selectedScenario == -1, "Scene");
        sceneTab.Changed += (object sender, EventArgs e) =>
        {
            var tab = (ToggleButton)sender;
            tab.Value = true;

            selectedScenario = -1;
            foreach (var scenarioTab in scenarioTabs)
                scenarioTab.Value = false;
        };

        scenarioTabs.Clear();
        for (int i = 0; i < sceneNarrative.scenarios.Count; i++)
        {
            scenarioTabs.Add(new ToggleButton(selectedScenario == i, "Scenario " + (i + 1) + " - " + sceneNarrative.scenarios[i].name));
            var tabNum = i;
            scenarioTabs[i].Changed += (object sender, EventArgs e) =>
            {
                var tab = (ToggleButton)sender;
                tab.Value = true;

                selectedScenario = tabNum;
                sceneTab.Value = false;
                foreach (var scenarioTab in scenarioTabs)
                    if (scenarioTab != scenarioTabs[tabNum])
                        scenarioTab.Value = false;
            };
        }

        fontSizeSlider = new IntSlider(11, 10, 20)
        {
            MaxWidth = 150
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
        loadScenarioBtn.Changed += LoadScenarioBtn_Pressed;

        saveAllBtn = new Button("Save All");
        saveAllBtn.Style.FontStyle = FontStyle.BoldAndItalic;
        saveAllBtn.Changed += SaveAllBtn_Pressed;

        loadAllBtn = new Button("Load All");
        loadAllBtn.Style.FontStyle = FontStyle.BoldAndItalic;
        loadAllBtn.Changed += LoadAllBtn_Pressed;

        sceneEditor = new SceneNarrationEditor(sceneNarrative);
    }

    private void AddScenarioBtn_Pressed(object sender, EventArgs e)
    {
        Scenario scenario = new Scenario(sceneNarrative.scenarios.ToArray());
        sceneNarrative.scenarios.Add(scenario);
        scenarioEditors.Add(new ScenarioEditor(scenario));
        var tabNum = scenarioTabs.Count;
        scenarioTabs.Add(new ToggleButton(false, "Scenario " + (tabNum) + " - " + sceneNarrative.scenarios[tabNum].name));
        scenarioTabs[tabNum].Changed += (object tabSender, EventArgs args) =>
        {
            var tab = (ToggleButton)sender;
            tab.Value = true;

            selectedScenario = tabNum;
            sceneTab.Value = false;
            foreach (var scenarioTab in scenarioTabs)
                if (scenarioTab != scenarioTabs[tabNum])
                    scenarioTab.Value = false;
        };
    }
    private void DelScenarioBtn_Pressed(object sender, EventArgs e)
    {
        if (EditorUtility.DisplayDialog("Remove Scenario",
                       "Are you sure you want to delete this scenario?", "Delete", "Cancel"))
        {
            sceneNarrative.scenarios.RemoveAt(selectedScenario);
            scenarioEditors.RemoveAt(selectedScenario);
            scenarioTabs.RemoveAt(selectedScenario);
            if (selectedScenario > 0)
                scenarioTabs[--selectedScenario].Value = true;

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

    private void LoadScenarioBtn_Pressed(object sender, EventArgs e)
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

        OnEnable();
    }

    private void SaveAllBtn_Pressed(object sender, EventArgs e)
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
    private void LoadAllBtn_Pressed(object sender, EventArgs e)
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
        using (new OverrideTextStyle(12))
        {
            sceneTab.Draw();

            for (int i = 0; i < sceneNarrative.scenarios.Count; i++)
            {
                var tab = scenarioTabs[i];
                tab.Content.text = "Scenario " + (i + 1) + " - " + sceneNarrative.scenarios[i].name;
                tab.Draw();
            }

            FlexibleSpace.Draw();

            fontSizeSlider.Draw();
            scale = fontSizeSlider.Value;

            addScenarioBtn.Draw();
            delScenarioBtn.Draw();

            if (selectedScenario > -1)
            {
                saveScenarioBtn.Draw();
                loadScenarioBtn.Draw();
            }
            saveAllBtn.Draw();
            loadAllBtn.Draw();
        }

        using (new OverrideTextStyle(fontSizeSlider.Value))
        using (new ScrollView(ref scrollPosition))
        {
            // Edit basic scene info
            if (selectedScenario == -1)
            {
                sceneEditor.Draw();
            }
            // Edit selected scenario
            else
            {
                scenarioEditors[selectedScenario].Edit();
            }
        }
    }
}