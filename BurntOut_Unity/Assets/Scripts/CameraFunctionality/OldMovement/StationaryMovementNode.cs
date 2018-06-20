using System.Linq;
using UnityEngine;

public class StationaryMovementNode : MonoBehaviour
{
    public InteractiveNode[] interactiveNodes;
    public LookNode[] lookhere;

    private MovementManager movementManager;

    public InteractiveNode currentInteractiveNode;
    public InteractiveNode previousInteractiveNode;
    public int iterator = 0;

    public bool CONTROLLERSUPPORT = false;


    // Use this for initialization
    void Start()
    {
        movementManager = FindObjectOfType<MovementManager>();
    }

    private void Update()
    {
        // if any of interactive nodes are hovered...
        var node = interactiveNodes.FirstOrDefault(n => n.mouseHovered);
        if (node != null)
        {
            previousInteractiveNode = currentInteractiveNode;
            currentInteractiveNode = node;
        }

        // if this node is currently selected, make it have functionality
        if (movementManager.currentStationNode == this)
        {
            if (Input.GetKeyDown(KeyCode.H) && CONTROLLERSUPPORT)
            {
                if (currentInteractiveNode == null)
                {
                    currentInteractiveNode = interactiveNodes[iterator];
                    iterator = 0;

                    // selection visual
                    HighlightNode(currentInteractiveNode);
                }
                else
                {
                    // if not out of bounds
                    if (iterator != interactiveNodes.Length - 1)
                    {
                        iterator++;
                        previousInteractiveNode = currentInteractiveNode;
                        currentInteractiveNode = interactiveNodes[iterator];

                        // selection visual
                        DeHighlightNode(previousInteractiveNode);
                        HighlightNode(currentInteractiveNode);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.G) && CONTROLLERSUPPORT)
            {
                if (currentInteractiveNode == null)
                {
                    currentInteractiveNode = interactiveNodes[interactiveNodes.Length - 1];
                    iterator = interactiveNodes.Length - 1;

                    // selection visual
                    HighlightNode(currentInteractiveNode);
                }
                else
                {
                    // if not out of bounds
                    if (iterator != 0)
                    {
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

    public void HighlightNode(InteractiveNode node)
    {
        node.gameObject.GetComponent<Animator>().enabled = true;
    }

    public void DeHighlightNode(InteractiveNode node)
    {
        node.gameObject.GetComponent<Animator>().enabled = false;
    }

}
