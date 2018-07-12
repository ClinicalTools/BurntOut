using System;
using UnityEngine;

namespace Narrative.Vars
{
    [Serializable]
    public class NarrativeVarCheck
    {
        private NarrativeVar var;
        public NarrativeVar Var
        {
            get
            {
                if (var == null)
                {
                    //ScenarioManager.Instance.Scenario.
                }
                return var;
            }
            set
            {
                var = value;
                varId = var.id;
            }
        }

        [SerializeField]
        public int varId;

        [SerializeField]
        private BoolCheck boolCheck;
        public BoolCheck BoolCheck
        {
            get
            {
                if (boolCheck != null)
                    boolCheck = new BoolCheck();

                return boolCheck;
            }
        }
        [SerializeField]
        private IntCheck intCheck;
        public IntCheck IntCheck
        {
            get
            {
                if (intCheck != null)
                    intCheck = new IntCheck();

                return intCheck;
            }
        }

        public bool Check()
        {
            switch (Var.type)
            {
                case VarType.Bool:
                    return BoolCheck.Check(Var.boolVal);
                case VarType.Int:
                    return IntCheck.Check(var.intVal);
                default:
                    return false;
            }
        }
    }
}