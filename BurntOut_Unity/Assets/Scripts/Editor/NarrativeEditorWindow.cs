using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Manages editing the scene's scenarios.
/// </summary>
public class NarrativeEditorWindow : EditorWindow
{
    private readonly List<ScenarioEditor> scenarioEditors = new List<ScenarioEditor>();
    private int selectedScenario = 0;

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
        if (sceneManager.scenarios.Count == 0)
            sceneManager.scenarios.Add(new Scenario());

        // Ensure scenarios in the editor match the scenarios in the scene
        if (scenarioEditors.Count != sceneManager.scenarios.Count || scenarioEditors[0].scenario != sceneManager.scenarios[0])
        {
            scenarioEditors.Clear();
            foreach (Scenario scenario in sceneManager.scenarios)
                scenarioEditors.Add(new ScenarioEditor(scenario));
        }

        // Draw the toolbar for scenario management
        using (new EditorHorizontal(EditorStyles.toolbar))
        {
            EditorStyles.toolbarButton.fontSize = 12;
            for (int i = 0; i < sceneManager.scenarios.Count; i++)
            {
                if (GUILayout.Toggle(i == selectedScenario, "Scenario " + (i + 1) + " - " + sceneManager.scenarios[i].Name, EditorStyles.toolbarButton))
                {
                    selectedScenario = i;
                }
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("+", EditorStyles.toolbarButton))
            {
                Scenario scenario = new Scenario();
                sceneManager.scenarios.Add(scenario);
                scenarioEditors.Add(new ScenarioEditor(scenario));
            }
            if (GUILayout.Button("-", EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Remove Scenario",
                    "Are you sure you want to delete this scenario?", "Delete", "Cancel"))
                {
                    sceneManager.scenarios.RemoveAt(selectedScenario);
                    scenarioEditors.RemoveAt(selectedScenario);
                    selectedScenario--;
                }
            }

            EditorStyles.toolbarButton.fontStyle = FontStyle.Italic;
            GUILayout.Button("Save Scenario", EditorStyles.toolbarButton);
            GUILayout.Button("Load Scenario", EditorStyles.toolbarButton);
            EditorStyles.toolbarButton.fontStyle = FontStyle.BoldAndItalic;
            GUILayout.Button("Save All", EditorStyles.toolbarButton);
            GUILayout.Button("Load All", EditorStyles.toolbarButton);
            EditorStyles.toolbarButton.fontStyle = FontStyle.Normal;
            EditorStyles.toolbarButton.fontSize = 0;
        }

        // Draw the selected scenario
        using (new EditorScrollView())
        {
            scenarioEditors[selectedScenario].Edit();
        }

    }
}