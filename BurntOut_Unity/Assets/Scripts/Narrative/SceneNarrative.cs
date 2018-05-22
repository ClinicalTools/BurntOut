using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneNarrative
{
    [SerializeField]
    public List<Scenario> scenarios = new List<Scenario>();

    [SerializeField]
    public string startNarration;
    [SerializeField]
    public string endNarration;
}
