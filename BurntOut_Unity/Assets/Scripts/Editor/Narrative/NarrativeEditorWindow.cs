using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using CtiEditor;

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

    // Add menu named "Scene Manager" to the Window menu
    [MenuItem("Window/Narrative Manager")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        NarrativeEditorWindow window = GetWindow<NarrativeEditorWindow>("Narrative");
        window.Show();
    }

    NarrativeManager sceneManager;
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
    }

    void OnInspectorUpdate()
    {
        ResetSceneManager();
    }


    void OnGUI()
    {
        if (sceneManager == null)
            ResetSceneManager();

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
        using (CtiEditorGUI.Toolbar())
        {
            var oldFontSize = EditorStyles.toolbarButton.fontSize;
            EditorStyles.toolbarButton.fontSize = 12;
            if (GUILayout.Toggle(selectedScenario == -1, "SCENE", EditorStyles.toolbarButton))
            {
                selectedScenario = -1;
            }

            for (int i = 0; i < sceneNarrative.scenarios.Count; i++)
            {
                if (GUILayout.Toggle(selectedScenario == i,
                    "Scenario " + (i + 1) + " - " + sceneNarrative.scenarios[i].name, EditorStyles.toolbarButton))
                {
                    selectedScenario = i;
                }
            }

            GUILayout.FlexibleSpace();

            scale = Mathf.Floor(EditorGUILayout.Slider(scale, 10, 20, GUILayout.MaxWidth(150)));

            if (GUILayout.Button("+", EditorStyles.toolbarButton))
            {
                Scenario scenario = new Scenario(sceneNarrative.scenarios.ToArray());
                sceneNarrative.scenarios.Add(scenario);
                scenarioEditors.Add(new ScenarioEditor(scenario));
            }
            if (GUILayout.Button("-", EditorStyles.toolbarButton))
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

            EditorStyles.toolbarButton.fontStyle = FontStyle.Italic;
            if (selectedScenario > -1)
            {

                if (GUILayout.Button("Save Scenario", EditorStyles.toolbarButton))
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
                if (GUILayout.Button("Load Scenario", EditorStyles.toolbarButton))
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
            EditorStyles.toolbarButton.fontStyle = FontStyle.BoldAndItalic;
            if (GUILayout.Button("Save All", EditorStyles.toolbarButton))
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
            if (GUILayout.Button("Load All", EditorStyles.toolbarButton))
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
            EditorStyles.toolbarButton.fontStyle = FontStyle.Normal;
            EditorStyles.toolbarButton.fontSize = oldFontSize;
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