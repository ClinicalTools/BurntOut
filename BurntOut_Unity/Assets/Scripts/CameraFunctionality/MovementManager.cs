using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

    public StationaryMovementNode[] stationaryMovementNodes;
    public StationaryMovementNode currentStationNode;
    public StationaryMovementNode previousStationNode;
    public int iterator = 0;

    public LookNode[] lookhere;

    private Camera playerCamera;
    private PlayerMoveToTarget myMoveTo;
    private PlayerRotateToTarget myRotateTo;

    void Start () {

        currentStationNode = stationaryMovementNodes[0];

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        myMoveTo = playerCamera.GetComponent<PlayerMoveToTarget>();

        // start player at this node
        playerCamera.transform.position = stationaryMovementNodes[0].transform.position;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.X)) {

            // if currentnode has the function: go to next stationary movement node 
            if (currentStationNode.currentInteractiveNode != null) {
                                
                if (currentStationNode.currentInteractiveNode.fx_moveStation) {

                    // degighlight 
                    currentStationNode.DeHighlightNode(currentStationNode.currentInteractiveNode);                    

                    // set new
                    myMoveTo.enabled = true;
                    currentStationNode = currentStationNode.currentInteractiveNode.nextStation;
                    previousStationNode = currentStationNode;

                    // moveto and rotateto
                    myMoveTo.target = currentStationNode.gameObject;
                    myRotateTo.target = currentStationNode.lookhere[0].gameObject;

                    // reset iterator value
                    previousStationNode.iterator = 0;
                    previousStationNode.currentInteractiveNode = null;
                    previousStationNode.previousInteractiveNode = null;
                }

            }

            // if currentnode has the function: move to interactive node (USE THIS FUNCTIONALITY ONLY FOR LEVEL SELECT)
            if (currentStationNode.currentInteractiveNode != null) {

                if (currentStationNode.currentInteractiveNode.fx_moveTo) {
                    myMoveTo.target = currentStationNode.currentInteractiveNode.gameObject;
                }

            }

        }

    }

 



}
