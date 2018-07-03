using OOEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative.Inspector
{
    public class ActorPrefabDrawer : FoldoutClassDrawer<Actor>
    {
        private readonly TextField actorName;
        private readonly List<Tuple<LabelField, SpriteField>> spriteTuples
            = new List<Tuple<LabelField, SpriteField>>();

        protected override Foldout Foldout { get; }

        public ActorPrefabDrawer(Actor value) : base(value)
        {
            if (value.icon != null)
                Foldout = new Foldout(value.name, null, value.icon.texture);
            else
                Foldout = new Foldout(value.name, null);
            Foldout.Style.FontStyle = FontStyle.Bold;

            actorName = new TextField(Value.name, "Name:");
            actorName.Changed += (sender, e) =>
            {
                Value.name = e.Value;
                Foldout.Content.text = Value.name;
            };

            var spriteLabel = new LabelField("Icon:");
            var spriteField = new SpriteField(value.icon);
            spriteField.Changed += (sender, e) =>
            {
                Value.icon = e.Value;
                if (Value.icon != null)
                    Foldout.Content.image = Value.icon?.texture;
            };
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteLabel = new LabelField("Neutral:");
            spriteField = new SpriteField(value.neutral);
            spriteField.Changed += (sender, e) =>
            {
                Value.neutral = e.Value;
            };
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteLabel = new LabelField("Happy:");
            spriteField = new SpriteField(Value.happy);
            spriteField.Changed += (sender, e) =>
            {
                Value.happy = e.Value;
            };
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteLabel = new LabelField("Sad:");
            spriteField = new SpriteField(Value.sad);
            spriteField.Changed += (sender, e) =>
            {
                Value.sad = e.Value;
            };
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteLabel = new LabelField("Angry:");
            spriteField = new SpriteField(Value.angry);
            spriteField.Changed += (sender, e) =>
            {
                Value.angry = e.Value;
            };
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));

            spriteLabel = new LabelField("Scared:");
            spriteField = new SpriteField(Value.scared);
            spriteField.Changed += (sender, e) =>
            {
                Value.scared = e.Value;
            };
            spriteTuples.Add(new Tuple<LabelField, SpriteField>(spriteLabel, spriteField));
        }

        protected override void Display()
        {
            actorName.Draw(Value.name);

            //using (GUIContainer.Draw())
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
        }
    }
}