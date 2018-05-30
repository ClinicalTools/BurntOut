using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetY : MonoBehaviour {
    public float y;
	// Update is called once per frame
	void Update () {
        var pos = transform.position;
        pos.y = y;
        transform.position = pos;
	}
}
