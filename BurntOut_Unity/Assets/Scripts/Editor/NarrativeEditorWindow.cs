using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.IO;

/// <summary>
/// Manages editing the scene's scenarios.
/// </summary>
public class NarrativeEditorWindow : EditorWindow
{
    private readonly List<ScenarioEditor> scenarioEditors = new List<ScenarioEditor>();
    private int selectedScenario = -1;
    private Vector2 scrollPosition = new Vector2();

    // Add menu named "Scene Manager" to the Window menu
    [MenuItem("Window/Narrative Manager")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        NarrativeEditorWindow window = GetWindow<NarrativeEditorWindow>("Narrative");
        window.Show();
    }

    void OnGUI()
    {
        // If I don't reload this often, the editor will become disconnected from the object after a test play.
        GameObject sceneManagerObj = GameObject.Find("NarrativeManager");

        // Initalize if null
        if (sceneManagerObj == null)
        {
            sceneManagerObj = new GameObject("NarrativeManager");
            sceneManagerObj.AddComponent<NarrativeManager>();
        }
        NarrativeManager sceneManager = sceneManagerObj.GetComponent<NarrativeManager>();
        if (sceneManager == null)
        {
            sceneManagerObj.AddComponent<NarrativeManager>();
            sceneManager = sceneManagerObj.GetComponent<NarrativeManager>();
        }

        // Allows the scene to save changes and 'undo' to be possible
        Undo.RecordObject(sceneManager, "NarrativeManager change");

        // Ensure there's at least one scenario
        if (sceneManager.sceneNarrative.scenarios.Count == 0)
            sceneManager.sceneNarrative.scenarios.Add(new Scenario());

        // Ensure scenarios in the editor match the scenarios in the scene
        if (scenarioEditors.Count != sceneManager.sceneNarrative.scenarios.Count ||
            scenarioEditors[0].scenario != sceneManager.sceneNarrative.scenarios[0])
        {
            scenarioEditors.Clear();
            foreach (Scenario scenario in sceneManager.sceneNarrative.scenarios)
                scenarioEditors.Add(new ScenarioEditor(scenario));
        }

        // Draw the toolbar for scenario management
        using (new EditorHorizontal(EditorStyles.toolbar))
        {
            EditorStyles.toolbarButton.fontSize = 12;
            if (GUILayout.Toggle(selectedScenario == -1, "SCENE", EditorStyles.toolbarButton))
            {
                selectedScenario = -1;
            }

            for (int i = 0; i < sceneManager.sceneNarrative.scenarios.Count; i++)
            {
                if (GUILayout.Toggle(selectedScenario == i,
                    "Scenario " + (i + 1) + " - " + sceneManager.sceneNarrative.scenarios[i].Name, EditorStyles.toolbarButton))
                {
                    selectedScenario = i;
                }
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("+", EditorStyles.toolbarButton))
            {
                Scenario scenario = new Scenario();
                sceneManager.sceneNarrative.scenarios.Add(scenario);
                scenarioEditors.Add(new ScenarioEditor(scenario));
            }
            if (GUILayout.Button("-", EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Remove Scenario",
                    "Are you sure you want to delete this scenario?", "Delete", "Cancel"))
                {
                    sceneManager.sceneNarrative.scenarios.RemoveAt(selectedScenario);
                    scenarioEditors.RemoveAt(selectedScenario);
                    if (selectedScenario > 0)
                        selectedScenario--;

                    // Ensure there's at least one scenario
                    if (sceneManager.sceneNarrative.scenarios.Count == 0)
                    {
                        Scenario scenario = new Scenario();
                        sceneManager.sceneNarrative.scenarios.Add(scenario);
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
                    string json = JsonUtility.ToJson(sceneManager.sceneNarrative.scenarios[selectedScenario]);
                    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    path += "\\BurntOut\\Narrative\\" + SceneManager.GetActiveScene().name + "\\Scenario";
                    Directory.CreateDirectory(path);

                    // Name.yyyy.MM.dd.letter.json
                    string fileName = sceneManager.sceneNarrative.scenarios[selectedScenario].Name + DateTime.Now.ToString("'.'yyyy'.'MM'.'dd");

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
                        sceneManager.sceneNarrative.scenarios[selectedScenario] = scenario;
                        scenarioEditors[selectedScenario] = new ScenarioEditor(scenario);
                    }
                }
            }
            EditorStyles.toolbarButton.fontStyle = FontStyle.BoldAndItalic;
            if (GUILayout.Button("Save All", EditorStyles.toolbarButton))
            {
                // Get the folder to save the scene in
                string json = JsonUtility.ToJson(sceneManager.sceneNarrative);
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

                    var sceneNarrative = JsonUtility.FromJson<SceneNarrative>(json);
                    sceneManager.sceneNarrative = sceneNarrative;
                }
            }
            EditorStyles.toolbarButton.fontStyle = FontStyle.Normal;
            EditorStyles.toolbarButton.fontSize = 0;
        }


        using (new EditorScrollView(ref scrollPosition))
        {
            // Edit basic scene info
            if (selectedScenario == -1)
            {
                EditorStyles.textField.wordWrap = true;
                EditorGUILayout.LabelField("Start Narration:");
                sceneManager.sceneNarrative.startNarration = EditorGUILayout.TextArea(sceneManager.sceneNarrative.startNarration);

                EditorGUILayout.LabelField("End Narration:");
                sceneManager.sceneNarrative.endNarration = EditorGUILayout.TextArea(sceneManager.sceneNarrative.endNarration);
            }
            // Edit selected scenario
            else
                scenarioEditors[selectedScenario].Edit();
        }
    }
}