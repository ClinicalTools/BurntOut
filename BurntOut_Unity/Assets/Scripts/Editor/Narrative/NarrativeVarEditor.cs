using OOEditor;

namespace Narrative.Vars.Inspector
{
    public class NarrativeVarEditor : ClassDrawer<NarrativeVar>
    {
        private readonly TextField nameField;

        private readonly EnumPopup typePopup;
        private readonly Toggle boolValToggle;
        private readonly IntField intValField;

        private readonly Toggle globalToggle;

        public NarrativeVarEditor(NarrativeVar value) : base(value)
        {
            nameField = new TextField(value.name);
            nameField.Changed += (sender, e) =>
            {
                Value.name = e.Value;
            };

            typePopup = new EnumPopup(Value.type)
            {
                FitWidth = true
            };
            typePopup.Changed += (sender, e) =>
            {
                Value.type = (VarType)e.Value;
            };

            boolValToggle = new Toggle(value.boolVal)
            {
                FitWidth = true
            };
            boolValToggle.Changed += (sender, e) =>
            {
                Value.boolVal = e.Value;
            };
            intValField = new IntField(value.intVal)
            {
                MaxWidth = 50
            };
            intValField.Changed += (sender, e) =>
            {
                Value.intVal = e.Value;
            };
        }

        protected override void Display()
        {
            nameField.Draw();
            typePopup.Draw();

            switch (Value.type)
            {
                case VarType.Bool:
                    boolValToggle.Draw();
                    break;
                case VarType.Int:
                    intValField.Draw();
                    break;
            }
        }
    }
}