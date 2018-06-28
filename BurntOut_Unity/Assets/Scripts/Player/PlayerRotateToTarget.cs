using UnityEngine;

public class PlayerRotateToTarget : MonoBehaviour
{

    public GameObject target;
    public float speed;

    private void Start()
    {
        if (target == null)
            target = Camera.main.gameObject;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 targetDirection = target.transform.position - transform.position;
        Debug.DrawRay(transform.position, targetDirection, Color.red);

        float step = speed * Time.deltaTime;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0f);

        var rotation = Quaternion.LookRotation(newDirection);

        var lastRotation = transform.rotation;
        transform.rotation = rotation;

        if (Mathf.Abs(lastRotation.w - rotation.w) < .0001f && Mathf.Abs(lastRotation.x - rotation.x) < .0001f
            && Mathf.Abs(lastRotation.y - rotation.y) < .0001f && Mathf.Abs(lastRotation.z - rotation.z) < .0001f)
        {
            target = null;
        }
    }


}
