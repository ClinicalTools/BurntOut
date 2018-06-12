using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float speed;

    public void Update() {
  
        transform.Rotate(Input.GetAxis("Vertical") * speed * Time.deltaTime, Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);
       
    }
}
