using System.Collections;
using UnityEngine;

public class PositionNode : MonoBehaviour
{
    public CameraLookHere CameraLook;
    public Transform CameraPosition;
    
    public void MoveTo()
    {
        var cameraLooks = FindObjectsOfType<CameraLookHere>();
        foreach (var cameraLook in cameraLooks)
            cameraLook.enabled = false;

        PlayerMovement.Instance.MoveTo(CameraPosition.gameObject, CameraLook.gameObject);

        gameObject.SetActive(false);
    }

    public void FadeTo()
    {
        var cameraLooks = FindObjectsOfType<CameraLookHere>();
        foreach (var cameraLook in cameraLooks)
            cameraLook.enabled = false;

        PlayerMovement.Instance.FadeTo(CameraPosition.gameObject, CameraLook.gameObject);

        gameObject.SetActive(false);
    }
}
