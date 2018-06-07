using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveNode : MonoBehaviour {

    public int id;

    public Material myMaterial;

    public bool fx_moveStation;
    public StationaryMovementNode nextStation;

    //private WorldSpaceUI ws_ui;

    private Camera playerCamera;
    private PlayerRotateToTarget myRotateTo;
    private PlayerMoveToTarget myMoveTo;

    // Use this for initialization
    void Start() {

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        myMoveTo = playerCamera.GetComponent<PlayerMoveToTarget>();
        myMaterial = this.gameObject.GetComponent<Material>();
        
    }

    public void MoveTo() {
        myMoveTo.target = this.gameObject;
    }


}
