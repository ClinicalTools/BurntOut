using UnityEngine;

public class PlayerRotateToTarget : MonoBehaviour
{

    public GameObject target;
    public float speed;
    private Main_GameManager gamemanager;

    private void Start()
    {
        gamemanager = FindObjectOfType<Main_GameManager>();
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
        if (transform.rotation == rotation)
            target = null;
        else
            transform.rotation = Quaternion.LookRotation(newDirection);
    }


}
