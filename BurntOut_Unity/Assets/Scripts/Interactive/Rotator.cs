using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 200;

    public void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, -Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        // Y component should always be 0, since we want it to be flat
        var horizontalAxis = new Vector3(transform.parent.forward.z, 0, -transform.parent.forward.x);
        transform.RotateAround(transform.position, horizontalAxis, Input.GetAxis("Vertical") * speed * Time.deltaTime);
    }
}
