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
    }
}