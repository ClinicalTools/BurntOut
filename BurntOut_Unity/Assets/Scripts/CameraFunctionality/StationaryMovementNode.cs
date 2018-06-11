﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryMovementNode : MonoBehaviour {

    public InteractiveNode[] interactiveNodes;
    public LookNode[] lookhere;

    private Camera playerCamera;
    private PlayerRotateToTarget myRotateTo;
    private PlayerMoveToTarget myMoveTo;
    private MovementManager movementManager;

    public InteractiveNode currentInteractiveNode;
    public InteractiveNode previousInteractiveNode;
    public int iterator = 0;

    public bool CONTROLLERSUPPORT = false;
    

    // Use this for initialization
    void Start() {

        movementManager = GameObject.FindObjectOfType<MovementManager>();

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        myMoveTo = playerCamera.GetComponent<PlayerMoveToTarget>();      

    }

    private void Update() {

        // if any of interactive nodes are hovered...
        foreach (InteractiveNode node in interactiveNodes) {
            if (node.mouseHovered) {
                previousInteractiveNode = currentInteractiveNode;
                currentInteractiveNode = node;
            }
        }

        // if this node is currently selected, make it have functionality
        if (movementManager.currentStationNode == this) {

            if (Input.GetKeyDown(KeyCode.H) && CONTROLLERSUPPORT == true) {

                if (currentInteractiveNode == null) {
                    currentInteractiveNode = interactiveNodes[iterator];
                    iterator = 0;

                    // selection visual
                    HighlightNode(currentInteractiveNode);

                } else {

                    // if not out of bounds
                    if (iterator != interactiveNodes.Length - 1) {
                        iterator++;
                        previousInteractiveNode = currentInteractiveNode;
                        currentInteractiveNode = interactiveNodes[iterator];

                        // selection visual
                        DeHighlightNode(previousInteractiveNode);
                        HighlightNode(currentInteractiveNode);
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.G) && CONTROLLERSUPPORT == true) {

                if (currentInteractiveNode == null) {
                    currentInteractiveNode = interactiveNodes[interactiveNodes.Length - 1];
                    iterator = interactiveNodes.Length - 1;

                    // selection visual
                    HighlightNode(currentInteractiveNode);

                } else {

                    // if not out of bounds
                    if (iterator != 0) {
                        iterator--;
                        previousInteractiveNode = currentInteractiveNode;
                        currentInteractiveNode = interactiveNodes[iterator];

                        // selection visual
                        DeHighlightNode(previousInteractiveNode);
                        HighlightNode(currentInteractiveNode);
                    }
                }
            }
        }

    }

    public void HighlightNode(InteractiveNode node) {

        node.gameObject.GetComponent<Animator>().enabled = true;

    }

    public void DeHighlightNode(InteractiveNode node) {

        node.gameObject.GetComponent<Animator>().enabled = false;

    }

}
