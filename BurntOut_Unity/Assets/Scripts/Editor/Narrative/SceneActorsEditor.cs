using OOEditor;
using System.Collections.Generic;
using System.Linq;
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

        public SceneActorsEditor(Scenario value) : base(value)
        {

            if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources"))
                AssetDatabase.CreateFolder("Assets/Prefabs", "Resources");
            if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Resources/Actors"))
                AssetDatabase.CreateFolder("Assets/Prefabs/Resources", "Actors");

            var resourceObjs = Resources.FindObjectsOfTypeAll<ActorObject>();
            var sceneObjs = Object.FindObjectsOfType<ActorObject>();

            var actorPrefabsList = new List<Actor>();
            foreach (ActorObject obj in resourceObjs.Where(a => !sceneObjs.Contains(a)))
                actorPrefabsList.Add(obj.actor);

            actorPrefabs = new FoldoutList<Actor, ActorPrefabDrawer>(actorPrefabsList, 
                false, false, false);
        }

        protected override void Display()
        {
            actorPrefabs.Draw();
        }
    }
}