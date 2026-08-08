using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NPCController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Random Movement Area")]
    public float minX = -20f;
    public float maxX = 20f;

    public float minZ = -20f;
    public float maxZ = 20f;

    [Header("Wait")]
    public float waitTime = 2f;

    [Header("Attack")]
    public float attackTime = 1f;

    [Header("Fall")]
    public Transform model;
    public float fallAngle = 90f;
    public float fallSpeed = 5f;


    private CharacterController controller;

    private Vector3 targetPosition;

    private bool waiting = false;

    private float waitTimer = 0f;

    private bool isStopped = false;

    private bool isFalling = false;

    private Quaternion targetRotation;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        PickNewTarget();
    }


    void Update()
    {
        // Stop normal movement after being attacked
        if (isStopped)
        {
            FallDown();
            return;
        }

        RandomMove();
    }


    //==================================================
    // Random Movement
    //==================================================

    void RandomMove()
    {
        if (waiting)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waiting = false;

                waitTimer = 0f;

                PickNewTarget();
            }

            return;
        }


        Vector3 direction =
            targetPosition - transform.position;

        direction.y = 0f;


        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();

            controller.Move(
                direction *
                moveSpeed *
                Time.deltaTime
            );

            transform.forward = direction;
        }


        if (Vector3.Distance(
            transform.position,
            targetPosition) < 0.5f)
        {
            waiting = true;

            waitTimer = 0f;
        }
    }


    //==================================================
    // Pick Random Target
    //==================================================

    void PickNewTarget()
    {
        float randomX =
            Random.Range(minX, maxX);

        float randomZ =
            Random.Range(minZ, maxZ);


        targetPosition = new Vector3(
            randomX,
            transform.position.y,
            randomZ
        );
    }


    //==================================================
    // Stop NPC
    //==================================================

    public void StopNPC()
    {
        if (isStopped)
            return;

        isStopped = true;

        waiting = false;

        waitTimer = 0f;

        // Stop movement immediately
        moveSpeed = 0f;

        // Start falling animation
        StartFalling();

        Debug.Log(
            gameObject.name +
            " has been stopped!"
        );


    }
    public bool IsStopped()
    {
        return isStopped;
    }


    //==================================================
    // Start Falling
    //==================================================

    void StartFalling()
    {
        if (model == null)
        {
            Debug.LogWarning(
                "NPC Model is not assigned!"
            );

            return;
        }


        isFalling = true;


        // Rotate around the local X axis
        targetRotation =
            model.localRotation *
            Quaternion.Euler(
                fallAngle,
                0f,
                0f
            );
    }


    //==================================================
    // Falling Animation
    //==================================================

    void FallDown()
    {
        if (!isFalling)
            return;


        model.localRotation =
            Quaternion.RotateTowards(
                model.localRotation,
                targetRotation,
                fallSpeed * Time.deltaTime * 90f
            );


        // Stop rotating when the target angle is reached
        if (Quaternion.Angle(
                model.localRotation,
                targetRotation) < 0.5f)
        {
            model.localRotation =
                targetRotation;

            isFalling = false;
        }
    }
}