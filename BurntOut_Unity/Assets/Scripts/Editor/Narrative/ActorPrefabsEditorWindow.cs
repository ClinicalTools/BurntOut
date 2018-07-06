using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using OOEditor;
using System;
using System.Text.RegularExpressions;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages editing the scene's scenarios.
    /// </summary>
    public class ActorPrefabsEditorWindow : EditorWindow
    {
        private const int DEFAULT_FONT_SIZE = 11;
        private const int MIN_FONT_SIZE = 10;
        private const int MAX_FONT_SIZE = 20;
        private const int TOOLBAR_FONT_SIZE = 12;
        private const string WINDOW_TITLE = "Actors";
        private const string MENU_TITLE = "Window/Actor Prefabs Manager";

        // Toolbar controls
        private IntSlider fontSizeSlider;
        private readonly OverrideTextStyle toolbarTextStyle = new OverrideTextStyle(TOOLBAR_FONT_SIZE);
        private readonly OverrideTextStyle textStyle = new OverrideTextStyle(DEFAULT_FONT_SIZE);

        // Drawers for Actor Prefabs portion 
        private readonly ScrollView scrollView = new ScrollView();
        private FoldoutList<ActorObject, ActorPrefabDrawer> actorPrefabsList;
        private Button createActorBtn;
        private GameObject actorTemplate;
        private List<ActorObject> actorPrefabs = new List<ActorObject>();

        private static ActorPrefabsEditorWindow window;
        [MenuItem(MENU_TITLE)]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            window = GetWindow<ActorPrefabsEditorWindow>(WINDOW_TITLE);
            window.Show();
        }

        private void OnEnable()
        {
            fontSizeSlider = new IntSlider(DEFAULT_FONT_SIZE, MIN_FONT_SIZE, MAX_FONT_SIZE)
            {
                MaxWidth = 150
            };
            fontSizeSlider.Changed += (sender, e) =>
            {
                textStyle.FontSize = e.Value;
            };

            // Ensure the path is valid
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources"))
                AssetDatabase.CreateFolder("Assets/Prefabs", "Resources");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources/Actors"))
                AssetDatabase.CreateFolder("Assets/Prefabs/Resources", "Actors");

            // Find all the actors in that path
            var actorResources = Resources.LoadAll("Actors", typeof(ActorObject));
            foreach (var obj in actorResources)
            {
                var actor = (ActorObject)obj;
                // The only actor with a 0 id is considered the template
                if (actor.actor?.id == 0)
                    actorTemplate = actor.gameObject;
                else if (actor.actor != null)
                    actorPrefabs.Add(actor);
            }

            actorPrefabsList = new FoldoutList<ActorObject, ActorPrefabDrawer>(actorPrefabs,
                false, false, false);

            createActorBtn = new Button("New Actor")
            {
                MaxWidth = 120
            };
            createActorBtn.Pressed += CreateActorBtn_Pressed;
        }

        private void CreateActorBtn_Pressed(object sender, EventArgs e)
        {
            var path = EditorUtility.SaveFilePanel("New Actor", "Assets/Prefabs/Resources/Actors",
                "", "prefab");
            var regexPattern = @"/Assets/.*\.prefab";
            var pathMatch = Regex.Match(path, regexPattern, RegexOptions.IgnoreCase);
            if (!pathMatch.Success || !pathMatch.Value.ToLower().Contains("/resources/"))
            {
                EditorUtility.DisplayDialog("Error",
                    "Asset must be saved as a prefab in a \"Resources\" folder," +
                    " within the project's \"Assets\" folder.", "OK");

                return;
            }
            path = pathMatch.Value.Substring(1);

            var prefab = PrefabUtility.CreatePrefab(path, actorTemplate);

            var prefabActorObj = prefab.GetComponent<ActorObject>();

            var actorsArr = new Actor[actorPrefabs.Count];
            for (var i = 0; i < actorPrefabs.Count; i++)
                actorsArr[i] = actorPrefabs[i].actor;
            prefabActorObj.actor = new Actor(actorsArr);

            actorPrefabs.Add(prefabActorObj);

            AssetDatabase.SaveAssets();
        }

        void OnGUI()
        {
            // Draw the toolbar for scenario management
            using (Toolbar.Draw())
            using (toolbarTextStyle.Draw())
            {
                FlexibleSpace.Draw();

                fontSizeSlider.Draw();
            }

            using (textStyle.Draw())
            using (scrollView.Draw())
            {
                actorPrefabs.Clear();
                var actorResources = Resources.LoadAll("Actors", typeof(ActorObject));
                foreach (var obj in actorResources)
                {
                    var actor = (ActorObject)obj;
                    if (actor.actor?.id == 0)
                        actorTemplate = actor.gameObject;
                    else if (actor.actor != null)
                        actorPrefabs.Add(actor);
                }

                actorPrefabsList.Draw(actorPrefabs);

                using (Indent.Draw())
                    createActorBtn.Draw();
            }
        }
    }
}