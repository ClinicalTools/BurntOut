using UnityEngine;

public class LookNode : MonoBehaviour
{

    public int id;
    //public bool start;
    public LookNode next;
    public LookNode behind;
    public LookNode currentlook = null;
    
    private PlayerRotateToTarget myRotateTo;
    private PlayerMoveToTarget myMoveTo;

    // Use this for initialization
    void Start()
    {
        var playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        myMoveTo = playerCamera.GetComponent<PlayerMoveToTarget>();
    }

    public void MoveTo()
    {
        //myMoveTo.target = gameObject;
    }

    public void RotateNext()
    {
        //myRotateTo.target = next.gameObject;
    }

    public void RotateBehind()
    {
        //myRotateTo.target = behind.gameObject;
    }

}
