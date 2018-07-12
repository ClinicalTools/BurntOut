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
        // Folder where scene data is saved to and loaded from
        // Must end with a '\'
        private const string DEFAULT_PATH = 
            @"P:\CTIInternal\Active\BurntOut\BurntOut - WriteUps\Scenarios\SceneData\";

        public static bool SaveScenario(Scenario scenario)
        {
            // Get the folder to save the scenario in
            string json = JsonUtility.ToJson(scenario);
            var path = DEFAULT_PATH + SceneManager.GetActiveScene().name;
            Directory.CreateDirectory(path);

            // Name.yyyy.MM.dd.letter.json
            string fileName = SceneManager.GetActiveScene().name + 
                DateTime.Now.ToString("'.'yyyy'.'MM'.'dd");

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

        public static Scenario LoadScenario()
        {
            // Get the folder to load the scenario from
            var path = DEFAULT_PATH + SceneManager.GetActiveScene().name;

            path = EditorUtility.OpenFilePanel("Load Scenario", path, "json");

            if (!String.IsNullOrEmpty(path))
            {
                string json = File.ReadAllText(path);

                return JsonUtility.FromJson<Scenario>(json);
            }

            return null;
        }
    }
}