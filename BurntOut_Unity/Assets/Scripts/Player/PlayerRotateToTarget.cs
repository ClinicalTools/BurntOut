using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateToTarget : MonoBehaviour {

    public GameObject target;
    public float speed;

	void Update () {

        Vector3 targetDirection = target.transform.position - transform.position;

        float step = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 3.0f);
       
        transform.rotation = Quaternion.LookRotation(newDirection);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);

    }

    
}
