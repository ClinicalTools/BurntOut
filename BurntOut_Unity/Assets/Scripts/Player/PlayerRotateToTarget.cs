using System.Collections;
using UnityEngine;

public class PlayerRotateToTarget : MonoBehaviour
{
    private const float TOLERANCE = .0001f;

    public static PlayerRotateToTarget Instance { get; private set; }
    public GameObject target;
    public float speed;

    private bool inDefaultPos = true;
    private bool cameraMoving = false;
    public bool CameraWait
    {
        get
        {
            return Main_GameManager.Instance.isCurrentlyExamine || !inDefaultPos || cameraMoving;
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
    private Vector3 movePos, lookPos;
    private Vector3 oldPos, oldDir;
    private Quaternion oldRot;

    /// <summary>
    /// Looks at an object then moves the camera to it.
    /// </summary>
    /// <param name="moveTarget">Object to move to.</param>
    /// <param name="lookTarget">Position to look at after the object moves.</param>
    public void MoveTo(GameObject moveTarget, GameObject lookTarget)
    {
        cameraMoving = true;
        moveLookTarget = moveTarget;
        movePos = moveTarget.transform.position;
        lookPos = lookTarget.transform.position;

        target = moveTarget;
        StartCoroutine(MoveTo());
    }

    private IEnumerator MoveTo()
    {
        while (target == moveLookTarget)
            yield return null;

        var distStep = Vector3.Distance(transform.position, movePos) / 30;

        var finalRot = Quaternion.LookRotation(lookPos - movePos);
        var rotStep = Quaternion.Angle(transform.rotation, finalRot) / 30;
        while (transform.position != movePos)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRot, rotStep);
            transform.position = Vector3.MoveTowards(transform.position, movePos, distStep);
            yield return new WaitForSecondsRealtime(1f / 60f);
        }


        cameraMoving = false;

        yield return null;
    }

    private bool fading;
    /// <summary>
    /// Fades the camera to black, moves to a specific place, and then fades back in.
    /// </summary>
    /// <param name="moveTarget">Object to move to.</param>
    /// <param name="lookTarget">Position to look at after the object moves.</param>
    public void FadeTo(GameObject moveTarget, GameObject lookTarget)
    {
        movePos = moveTarget.transform.position;
        lookPos = lookTarget.transform.position;

        StartCoroutine(FadeTo());
    }

    private IEnumerator FadeTo()
    {
        fading = true;

        Main_GameManager.Instance.screenfade.SetBool("fade", true);

        yield return new WaitForSeconds(0.5f);
        transform.position = movePos;
        transform.rotation = Quaternion.LookRotation(lookPos - transform.position);
        oldPos = transform.position;
        oldDir = transform.forward;
        oldRot = transform.rotation;

        Main_GameManager.Instance.screenfade.SetBool("fade", false);

        fading = false;
    }

    /// <summary>
    /// Looks at an object then moves the camera to a dist from the front of it.
    /// </summary>
    /// <param name="target">Object to look at and move in front of.</param>
    /// <param name="dist">Distance from the start of the object to move to.</param>
    public void ZoomLook(GameObject target, float dist)
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
        lookPos = moveLookTarget.transform.position;

        this.target = target;
        StartCoroutine(ZoomLook());
    }

    private IEnumerator ZoomLook()
    {
        while (target == moveLookTarget)
            yield return null;

        var distStep = Vector3.Distance(transform.position, movePos) / 30;
        while (transform.position != movePos)
        {
            transform.rotation = Quaternion.LookRotation(lookPos - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, movePos, distStep);
            yield return new WaitForSecondsRealtime(1f / 60f);
        }

        yield return null;
    }

    public void ReturnPosition()
    {
        if (!fading)
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
