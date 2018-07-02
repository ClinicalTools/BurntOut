using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChoiceController : MonoBehaviour {

    public Animator myanim;

    private void OnEnable() {
        myanim.SetBool("Choice", true);
    }

    private void OnDisable() {
        myanim.SetBool("Choice", false);
    }

}
