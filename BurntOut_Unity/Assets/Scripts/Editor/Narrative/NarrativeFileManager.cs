using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Narrative.Inspector
{
    public static class NarrativeFileManager
    {
        public static bool SaveScenario(Scenario scenario)
        {
            // Get the folder to save the scenario in
            string json = JsonUtility.ToJson(scenario);
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

            path = EditorUtility.SaveFilePanel("Save Scenario", path, fileName, "json");

            if (!String.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, json);
                return true;
            }
            return false;
        }

        public static Scenario LoadScenario(List<Scenario> scenarios)
        {
            // Get the folder to load the scenario from
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += "\\BurntOut\\Narrative\\" + SceneManager.GetActiveScene().name + "\\Scenario";

            path = EditorUtility.OpenFilePanel("Load Scenario", path, "json");

            if (!String.IsNullOrEmpty(path))
            {
                string json = File.ReadAllText(path);

                var scenario = JsonUtility.FromJson<Scenario>(json);
                for (int i = 0; i < scenarios.Count; i++)
                    scenario.ResetHash(scenarios.ToArray());

                return scenario;
            }

            return null;
        }
    }
}