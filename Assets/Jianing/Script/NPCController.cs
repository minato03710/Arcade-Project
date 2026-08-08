using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NPCController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Random Movement")]
    public float minX = -20f;
    public float maxX = 20f;
    public float minZ = -20f;
    public float maxZ = 20f;

    [Header("Wait")]
    public float waitTime = 2f;

    private CharacterController controller;

    private Vector3 targetPosition;

    private bool waiting = false;

    private float waitTimer = 0f;

    // Has the NPC already been attacked
    private bool hasBeenAttacked = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Choose the first random position at the start of the game
        PickNewTarget();
    }

    void Update()
    {
        // NPC stops moving after being attacked
        if (hasBeenAttacked)
            return;

        RandomMove();
    }

    //========================================
    // random move
    //========================================

    void RandomMove()
    {
        // NPC is waiting
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

        // Move toward the goal
        Vector3 direction = targetPosition - transform.position;

        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();

            controller.Move(
                direction * moveSpeed * Time.deltaTime
            );

            transform.forward = direction;
        }

        // Arrive at the destination
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            waiting = true;
            waitTimer = 0f;
        }
    }

    //========================================
    // Randomly select a target
    //========================================

    void PickNewTarget()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        targetPosition = new Vector3(
            randomX,
            transform.position.y,
            randomZ
        );
    }

    //========================================
    // be attacked
    //========================================

    public void StopMoving()
    {
        hasBeenAttacked = true;

        waiting = false;
        waitTimer = 0f;
    }
}