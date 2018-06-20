using System.Collections;
using UnityEngine;

public class ExamineObject_Canvas : MonoBehaviour {


    public float distanceFromCamera = 0.1f;
    private Camera mainCamera;


    public void Start() {
        mainCamera = Camera.main;
        
        StartCoroutine(SetPosAndRot());
    }

    public IEnumerator SetPosAndRot() {

        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
        gameObject.GetComponent<PlayerRotateToTarget>().enabled = true;

        yield return new WaitForSeconds(.2f);

        gameObject.GetComponent<PlayerRotateToTarget>().enabled = false;

    }

    private void Update() {
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
    }
}
