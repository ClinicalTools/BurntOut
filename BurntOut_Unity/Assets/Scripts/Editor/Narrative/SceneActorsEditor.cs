using OOEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a scenario.
    /// </summary>
    public class SceneActorsEditor : ClassDrawer<Scenario>
    {
        private readonly FoldoutList<Actor, ActorPrefabDrawer> actorPrefabs;

        private readonly List<Foldout> actorFoldouts;
        private ActorPrefabDrawer sceneActorDrawer;
        private readonly Popup actorPopup;
        private readonly Button createActorBtn;
        private GameObject actorTemplate;
        private List<Actor> actorPrefabsList = new List<Actor>();

        public SceneActorsEditor(Scenario value) : base(value)
        {
            var resourceObjs = Resources.FindObjectsOfTypeAll<ActorObject>();
            actorTemplate = resourceObjs.First(a => a.actor?.id == 0).gameObject;
            var sceneObjs = UnityEngine.Object.FindObjectsOfType<ActorObject>();

            foreach (ActorObject obj in resourceObjs.Where(
                a => a.actor != null && a.actor.id != 0 && !sceneObjs.Contains(a)))
            {
                actorPrefabsList.Add(obj.actor);
            }

            actorPrefabs = new FoldoutList<Actor, ActorPrefabDrawer>(actorPrefabsList,
                false, false, false);
            actorPrefabs.Changed += (sender, e) =>
            {
                AssetDatabase.SaveAssets();
            };

            createActorBtn = new Button("New Actor")
            {
                MaxWidth = 120
            };
            createActorBtn.Pressed += CreateActorBtn_Pressed;
        }

        private void CreateActorBtn_Pressed(object sender, EventArgs e)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources"))
                AssetDatabase.CreateFolder("Assets/Prefabs", "Resources");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources/Actors"))
                AssetDatabase.CreateFolder("Assets/Prefabs/Resources", "Actors");

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

            var actorObj = UnityEngine.Object.Instantiate(actorTemplate);
            var prefab = PrefabUtility.CreatePrefab(path, actorObj);
            var prefabActorObj = prefab.GetComponent<ActorObject>();
            prefabActorObj.actor = new Actor(actorPrefabsList.ToArray());

            actorPrefabsList.Add(prefabActorObj.actor);
            UnityEngine.Object.DestroyImmediate(actorObj);

            AssetDatabase.SaveAssets();
        }

        protected override void Display()
        {
            actorPrefabs.Draw(actorPrefabsList);

            using (Indent.Draw())
                createActorBtn.Draw();
        }
    }
}