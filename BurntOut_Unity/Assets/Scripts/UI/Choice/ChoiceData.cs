using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceData : MonoBehaviour {

    public string text;
    public float result;

    public PlayerStats stats;
    public NarrativeManager narrativeManager;

    private Text textComponent;

    public void Start() {

        textComponent = GetComponentInChildren<Text>();
        UpdateText();

    }

    public void ApplyResult() {

        stats.currentHealth += result;

    }

    public void UpdateText() {

        textComponent.text = text;
        
    }
}
