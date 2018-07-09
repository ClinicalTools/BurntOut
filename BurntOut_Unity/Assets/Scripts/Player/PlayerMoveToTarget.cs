using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveToTarget : MonoBehaviour {

    public GameObject target;
    public float speed;

    void Update() {
        
        if (target != null) {
            //float step = speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

    }
}
