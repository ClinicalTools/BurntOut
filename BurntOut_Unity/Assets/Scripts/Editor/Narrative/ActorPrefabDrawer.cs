using OOEditor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public class ActorPrefabDrawer : FoldoutClassDrawer<ActorObject>
    {
        private readonly DelayedTextField actorNameField;
        private readonly List<Tuple<LabelField, SpriteField>> spriteTuples
            = new List<Tuple<LabelField, SpriteField>>();
        private readonly Button deleteButton;

        protected override Foldout Foldout { get; }

        public ActorPrefabDrawer(ActorObject value) : base(value)
        {
            if (Value.actor.icon != null)
                Foldout = new Foldout(Value.actor.name, null, Value.actor.icon.texture);
            else
                Foldout = new Foldout(Value.actor.name, null);
            Foldout.Style.FontStyle = FontStyle.Bold;

            actorNameField = new DelayedTextField(Value.actor.name, "Name:");
            actorNameField.Changed += (sender, e) =>
            {
                Value.actor.name = e.Value;
                Foldout.Content.text = Value.actor.name;

                EditorUtility.SetDirty(Value);
                AssetDatabase.SaveAssets();
            };

            deleteButton = new Button("Delete Actor Prefab");
            deleteButton.Pressed += DeleteButton_Pressed;

            var spriteField = new SpriteField(Value.actor.icon);
            spriteField.Changed += (sender, e) =>
            {
                Value.actor.icon = e.Value;
                if (Value.actor.icon != null)
                    Foldout.Content.image = Value.actor.icon?.texture;

                EditorUtility.SetDirty(Value);
                AssetDatabase.SaveAssets();
            };
            var spriteLabel = new LabelField(spriteField, "Icon:");
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteField = new SpriteField(Value.actor.neutral);
            spriteField.Changed += (sender, e) =>
            {
                Value.actor.neutral = e.Value;

                EditorUtility.SetDirty(Value);
                AssetDatabase.SaveAssets();
            };
            spriteLabel = new LabelField(spriteField, "Neutral:");
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteField = new SpriteField(Value.actor.happy);
            spriteField.Changed += (sender, e) =>
            {
                Value.actor.happy = e.Value;

                EditorUtility.SetDirty(Value);
                AssetDatabase.SaveAssets();
            };
            spriteLabel = new LabelField(spriteField, "Happy:");
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteField = new SpriteField(Value.actor.sad);
            spriteField.Changed += (sender, e) =>
            {
                Value.actor.sad = e.Value;

                EditorUtility.SetDirty(Value);
                AssetDatabase.SaveAssets();
            };
            spriteLabel = new LabelField(spriteField, "Sad:");
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteField = new SpriteField(Value.actor.angry);
            spriteField.Changed += (sender, e) =>
            {
                Value.actor.angry = e.Value;

                EditorUtility.SetDirty(Value);
                AssetDatabase.SaveAssets();
            };
            spriteLabel = new LabelField(spriteField, "Angry:");
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteField = new SpriteField(Value.actor.scared);
            spriteField.Changed += (sender, e) =>
            {
                Value.actor.scared = e.Value;

                EditorUtility.SetDirty(Value);
                AssetDatabase.SaveAssets();
            };
            spriteLabel = new LabelField(spriteField, "Scared:");
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));
        }

        private void DeleteButton_Pressed(object sender, EventArgs e)
        {
            if (EditorUtility.DisplayDialog("Delete Actor",
                $"Are you sure you want to delete the prefab for {Value.actor.name} ({Value.name})?",
                "Delete", "Cancel"))
            {
                var path = AssetDatabase.GetAssetPath(Value.gameObject);
                AssetDatabase.DeleteAsset(path);
            }
        }

        protected override void Display()
        {
            actorNameField.Draw(Value.actor.name);

            using (Horizontal.Draw())
            {
                foreach (var spriteTuple in spriteTuples)
                {
                    using (GUIContainer.Draw())
                    {
                        spriteTuple.Item1.Draw();
                        spriteTuple.Item2.Draw();
                    }
                }
            }

            deleteButton.Draw();
        }
    }
}