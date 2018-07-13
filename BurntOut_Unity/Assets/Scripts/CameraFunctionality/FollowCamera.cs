using UnityEngine;

[ExecuteInEditMode]
public class FollowCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;
    }
}