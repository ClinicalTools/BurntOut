using System.Collections;
using UnityEngine;

public class PlayerRotateToTarget : MonoBehaviour
{
    private const float TOLERANCE = .0001f;

    public static PlayerRotateToTarget Instance { get; private set; }
    public GameObject target;
    public float speed;

    private bool inDefaultPos = true;
    public bool CameraWait
    {
        get
        {
            return Main_GameManager.Instance.isCurrentlyExamine || !inDefaultPos;
        }
    }

    private void Awake()
    {
        Instance = this;
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

        if (Mathf.Abs(lastRotation.w - rotation.w) < TOLERANCE && Mathf.Abs(lastRotation.x - rotation.x) < TOLERANCE
            && Mathf.Abs(lastRotation.y - rotation.y) < TOLERANCE && Mathf.Abs(lastRotation.z - rotation.z) < TOLERANCE)
        {
            target = null;
        }
    }


    private GameObject moveLookTarget;
    private Vector3 movePos;
    private Vector3 oldPos, oldDir;
    private Quaternion oldRot;
    /// <summary>
    /// Looks at an object then moves dist from the front of it.
    /// </summary>
    /// <param name="target">Object to look at and move in front of.</param>
    /// <param name="dist">Distance from the start of the object to move to.</param>
    public void MoveLook(GameObject target, float dist)
    {
        inDefaultPos = false;

        moveLookTarget = target;

        oldPos = transform.position;
        oldDir = transform.forward;
        oldRot = transform.rotation;

        var frontPos = target.transform.position + (target.transform.forward * dist);
        var frontDist = Vector3.Distance(frontPos, oldPos);
        var backPos = target.transform.position - (target.transform.forward * dist);
        var backDist = Vector3.Distance(backPos, oldPos);

        if (frontDist < backDist)
            movePos = frontPos;
        else
            movePos = backPos;

        this.target = target;
        StartCoroutine(MoveLook());
    }

    private IEnumerator MoveLook()
    {
        while (target == moveLookTarget)
            yield return null;

        var distStep = Vector3.Distance(transform.position, movePos) / 30;
        while (transform.position != movePos)
        {
            transform.rotation = Quaternion.LookRotation(moveLookTarget.transform.position - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, movePos, distStep);
            yield return new WaitForSecondsRealtime(1f / 60f);
        }

        yield return null;
    }

    public void ReturnPosition()
    {
        StartCoroutine(ReturnPos());
    }

    private IEnumerator ReturnPos()
    {
        var rotStep = Quaternion.Angle(transform.rotation, oldRot) * Mathf.Deg2Rad / 30;
        var distStep = Vector3.Distance(transform.position, oldPos) / 30;
        while (transform.position != oldPos)
        {
            var dir = Vector3.RotateTowards(transform.forward, oldDir, rotStep, 0f);
            transform.rotation = Quaternion.LookRotation(dir);
            transform.position = Vector3.MoveTowards(transform.position, oldPos, distStep);
            yield return new WaitForSecondsRealtime(1f / 60f);
        }

        inDefaultPos = true;

        yield return null;
    }
}
