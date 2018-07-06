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
        private readonly FoldoutList<ActorObject, ActorPrefabDrawer> actorPrefabs;
        private readonly Button createActorBtn;
        private GameObject actorTemplate;
        private List<ActorObject> actorPrefabsList = new List<ActorObject>();

        public SceneActorsEditor(Scenario value) : base(value)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources"))
                AssetDatabase.CreateFolder("Assets/Prefabs", "Resources");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources/Actors"))
                AssetDatabase.CreateFolder("Assets/Prefabs/Resources", "Actors");

            var actorResources = Resources.LoadAll("Actors", typeof(ActorObject));
            foreach (var obj in actorResources)
            {
                var actor = (ActorObject)obj;
                if (actor.actor?.id == 0)
                    actorTemplate = actor.gameObject;
                else if (actor.actor != null)
                    actorPrefabsList.Add(actor);
            }


            actorPrefabs = new FoldoutList<ActorObject, ActorPrefabDrawer>(actorPrefabsList,
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

            var actorsArr = new Actor[actorPrefabsList.Count];
            for (var i = 0; i < actorPrefabsList.Count; i++)
                actorsArr[i] = actorPrefabsList[i].actor;
            prefabActorObj.actor = new Actor(actorsArr);

            actorPrefabsList.Add(prefabActorObj);

            AssetDatabase.SaveAssets();
        }

        protected override void Display()
        {
            actorPrefabsList.Clear();
            var actorResources = Resources.LoadAll("Actors", typeof(ActorObject));
            foreach (var obj in actorResources)
            {
                var actor = (ActorObject)obj;
                if (actor.actor?.id == 0)
                    actorTemplate = actor.gameObject;
                else if (actor.actor != null)
                    actorPrefabsList.Add(actor);
            }

            actorPrefabs.Draw(actorPrefabsList);

            using (Indent.Draw())
                createActorBtn.Draw();
        }
    }
}