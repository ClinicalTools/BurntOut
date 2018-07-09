using UnityEngine;

public class PositionNode : MonoBehaviour
{
    public CameraLookHere CameraLook;
    public Transform CameraPosition;
    
    public void LookAt()
    {
        var cameraLooks = FindObjectsOfType<CameraLookHere>();
        foreach (var cameraLook in cameraLooks)
            cameraLook.enabled = false;


        //CameraLook.gameObject.SetActive(true);
        //CameraLook.enabled = true;

        PlayerRotateToTarget.Instance.MoveTo(CameraPosition.gameObject, CameraLook.gameObject);



        gameObject.SetActive(false);
    }
}
