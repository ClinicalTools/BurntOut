using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateToTarget : MonoBehaviour {

    public GameObject target;
    public float speed;

    private Main_GameManager gamemanager;

    private void Start() {
        gamemanager = FindObjectOfType<Main_GameManager>();

        if (gamemanager.scene.name == "Hospital_Patient_SingleRoom")
            target = Camera.main.gameObject;
    }

    void Update () {

        Vector3 targetDirection = target.transform.position - transform.position;
        Debug.DrawRay(transform.position, targetDirection, Color.red);

        float step = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0f);
       
        transform.rotation = Quaternion.LookRotation(newDirection);
      

    }

    
}
