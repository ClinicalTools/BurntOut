using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using Minigames.Reading;
/*
namespace Minigames.Inspector
{
    /// <summary>
    /// Manages editing the scene's scenarios.
    /// </summary>
    public class MinigamesEditorWindow : EditorWindow
    {
        //private readonly ReadingGameEditor() 
        private int selectedGame = -1;
        private Vector2 scrollPosition = new Vector2();
        private float scale = 11;

        private readonly string gameObjectName = "MinigameManager";

        // Add menu named "Minigame Manager" to the Window menu
        [MenuItem("Window/Minigame Manager")]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            MinigamesEditorWindow window = GetWindow<MinigamesEditorWindow>("Minigames");
            window.Show();
        }

        void OnGUI()
        {
            // If I don't reload this often, the editor will become disconnected from the object after a test play.
            var sceneManagerObj = GameObject.Find(gameObjectName);

            // Initalize if null
            if (sceneManagerObj == null)
            {
                sceneManagerObj = new GameObject(gameObjectName);
                sceneManagerObj.AddComponent<MinigameManager>();
            }
            var minigameManager = sceneManagerObj.GetComponent<MinigameManager>();
            if (minigameManager == null)
            {
                sceneManagerObj.AddComponent<MinigameManager>();
                minigameManager = sceneManagerObj.GetComponent<MinigameManager>();
            }

            // Reading game work
            sceneManagerObj.transform.Find("ReadingGame");

            SceneMinigames sceneMinigames = minigameManager.sceneMinigames;



            // Allows the scene to save changes and 'undo' to be possible
            Undo.RecordObject(minigameManager, "MinigamesManager change");

            // Draw the toolbar for scenario management
            using (new EditorHorizontal(EditorStyles.toolbar))
            {
                var oldFontSize = EditorStyles.toolbarButton.fontSize;
                EditorStyles.toolbarButton.fontSize = 12;
                if (GUILayout.Toggle(selectedGame == -1, "GAMES", EditorStyles.toolbarButton))
                    selectedGame = -1;
                if (GUILayout.Toggle(selectedGame == 0, "Reading", EditorStyles.toolbarButton))
                    selectedGame = 0;


                GUILayout.FlexibleSpace();

                scale = Mathf.Floor(EditorGUILayout.Slider(scale, 8, 20, GUILayout.MaxWidth(150)));

                EditorStyles.toolbarButton.fontSize = oldFontSize;
            }

            EditorStyles.textField.wordWrap = true;
            EditorStyles.textArea.wordWrap = true;
            using (new EditorSize((int)scale))
            {
                using (new EditorScrollView(ref scrollPosition))
                {
                    // Edit basic scene info
                    if (selectedGame == -1)
                    {
                        EditorStyles.textField.wordWrap = true;
                        
                        EditorStyles.label.fontStyle = FontStyle.Normal;
                    }
                    // Edit selected scenario
                    else
                    {
                        //minigameEditors[selectedGame].Edit();
                    }
                }
            }
        }
    }
}
*/