using OOEditor;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public class SceneGeneralEditor : ClassDrawer<Scenario>
    {
        private readonly Toggle startNarrativeToggle, endNarrativeToggle;
        private readonly TextArea startNarrativeField, endNarrativeField;

        private readonly ObjectField<SceneAsset> sceneField;
        private readonly Toggle autoChangeSceneToggle, sceneChangeToggle;
        private readonly Foldout triggersFoldout;
        private readonly ReorderableList<Trigger, TriggerDrawer> triggerList;

        public SceneGeneralEditor(Scenario value) : base(value)
        {
            startNarrativeToggle = new Toggle(Value.hasStartNarrative, "Start Narrative:");
            startNarrativeToggle.Changed += (sstarter, e) =>
            {
                Value.hasStartNarrative = e.Value;
            };
            startNarrativeField = new TextArea(Value.startNarrative);
            startNarrativeField.Changed += (sstarter, e) =>
            {
                Value.startNarrative = e.Value;
            };

            endNarrativeToggle = new Toggle(Value.hasEndNarrative, "End Narrative:");
            endNarrativeToggle.Changed += (sender, e) =>
            {
                Value.hasEndNarrative = e.Value;
            };
            endNarrativeField = new TextArea(Value.endNarrative);
            endNarrativeField.Changed += (sender, e) =>
            {
                Value.endNarrative = e.Value;
            };

            sceneChangeToggle = new Toggle(Value.sceneChange, "Scene Change:", "Allows a next scene to be set as part of the narrative");
            sceneChangeToggle.Changed += (sender, e) =>
            {
                Value.sceneChange = e.Value;
            };
            sceneField = new ObjectField<SceneAsset>(
                AssetDatabase.LoadAssetAtPath<SceneAsset>(Value.scenePath), false, "Scene:");
            sceneField.Changed += (sender, e) =>
            {
                Value.scenePath = AssetDatabase.GetAssetPath(e.Value);
            };
            autoChangeSceneToggle = new Toggle(Value.autoChangeScene, "Automatically Transition:",
                    "Scene transitions immediately after the narrative ends");
            autoChangeSceneToggle.Changed += (sender, e) =>
            {
                Value.autoChangeScene = e.Value;
            };
            triggersFoldout = new Foldout("Triggers");
            triggersFoldout.Style.FontStyle = FontStyle.Bold;
            triggerList = new ReorderableList<Trigger, TriggerDrawer>(Value.SceneChangeTriggers);
        }

        protected override void Display()
        {
            startNarrativeToggle.Draw(Value.hasStartNarrative);
            if (Value.hasStartNarrative)
                using (Indent.Draw())
                    startNarrativeField.Draw(Value.startNarrative);

            endNarrativeToggle.Draw(Value.hasEndNarrative);
            if (Value.hasEndNarrative)
                using (Indent.Draw())
                    endNarrativeField.Draw(Value.endNarrative);

            sceneChangeToggle.Draw(Value.sceneChange);
            if (Value.sceneChange)
            {
                using (Indent.Draw())
                {
                    sceneField.Draw(AssetDatabase.LoadAssetAtPath<SceneAsset>(Value.scenePath));

                    autoChangeSceneToggle.Draw(Value.autoChangeScene);
                    if (!Value.autoChangeScene)
                    {
                        triggersFoldout.Draw();
                        if (triggersFoldout.Value)
                            using (Indent.Draw())
                                triggerList.Draw(Value.SceneChangeTriggers);
                    }
                }
            }
        }
    }
}