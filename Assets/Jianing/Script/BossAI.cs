using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BossAI : MonoBehaviour
{
    public enum BossState
    {
        Patrol,
        Chase
    }

    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Patrol Area")]
    public float minX = -20f;
    public float maxX = 20f;
    public float minZ = -20f;
    public float maxZ = 20f;

    [Header("Detection")]
    public float detectRange = 10f;
    public float loseRange = 15f;

    [Header("Attack")]
    public int damageScore = 10;
    public float knockbackForce = 2f;

    [Header("Patrol Wait")]
    public float waitTime = 2f;

    private CharacterController controller;

    private BossState currentState = BossState.Patrol;

    private Transform targetPlayer;

    private Vector3 patrolTarget;

    private bool waiting = false;

    private bool targetLost = false;

    private float waitTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // The first patrol point is randomly selected at the start of the game.
        PickNewPatrolPoint();
    }

    void Update()
    {
        UpdateTarget();

        switch (currentState)
        {
            case BossState.Patrol:
                Patrol();
                break;

            case BossState.Chase:
                Chase();
                break;
        }
    }

    //==================================================
    // Update goal
    //==================================================

    void UpdateTarget()
    {
        // Boss just knocked down the player
        // Don't immediately look for another player during this period.
        if (targetLost)
        {
            currentState = BossState.Patrol;

            return;
        }

        if (player1 == null || player2 == null)
            return;

        PlayerHealth p1Health =
            player1.GetComponent<PlayerHealth>();

        PlayerHealth p2Health =
            player2.GetComponent<PlayerHealth>();

        bool p1Alive =
            p1Health != null && !p1Health.IsDown;

        bool p2Alive =
            p2Health != null && !p2Health.IsDown;

        // Both players collapsed to the ground.
        if (!p1Alive && !p2Alive)
        {
            targetPlayer = null;
            currentState = BossState.Patrol;
            return;
        }

        float d1 = p1Alive
            ? Vector3.Distance(transform.position, player1.position)
            : Mathf.Infinity;

        float d2 = p2Alive
            ? Vector3.Distance(transform.position, player2.position)
            : Mathf.Infinity;

        float nearestDistance = Mathf.Min(d1, d2);

        // Patrol player is not within detection range
        if (nearestDistance > detectRange)
        {
            targetPlayer = null;
            currentState = BossState.Patrol;
            return;
        }

        // Select the nearest alive player
        targetPlayer = d1 < d2 ? player1 : player2;

        currentState = BossState.Chase;
    }

    //==================================================
    // patrol
    //==================================================

    void Patrol()
    {
        // Boss has just knocked down the player and entered patrol mode.

        if (waiting)
        {

            // Let waiting time truly increase
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waiting = false;

                waitTimer = 0f;

                // Waiting to end
                targetLost = false;

                // Re-select patrol point
                PickNewPatrolPoint();
            }

            return;
        }

        // Move to patrol point
        MoveTo(patrolTarget);

        // Arrived at patrol point
        if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            waiting = true;

            waitTimer = 0f;
        }
    }

    //==================================================
    // Chase the player
    //==================================================

    void Chase()
    {
        if (targetPlayer == null)
        {
            currentState = BossState.Patrol;
            return;
        }

        PlayerHealth health =
            targetPlayer.GetComponent<PlayerHealth>();

        //The target has fallen.
        if (health != null && health.IsDown)
        {
            targetPlayer = null;

            currentState = BossState.Patrol;

            // Mark Boss has just knocked down the target
            targetLost = true;

            // Start a new patrol now
            waiting = false;

            waitTimer = 0f;

            PickNewPatrolPoint();

            return;
        }

        // If the target is too far away
        float distance =
            Vector3.Distance(transform.position, targetPlayer.position);

        if (distance > loseRange)
        {
            targetPlayer = null;

            currentState = BossState.Patrol;

            return;
        }

        MoveTo(targetPlayer.position);
    }

    //==================================================
    // Move
    //==================================================

    void MoveTo(Vector3 target)
    {
        Vector3 direction =
            target - transform.position;

        direction.y = 0f;

        if (direction.magnitude < 0.1f)
            return;

        direction.Normalize();

        controller.Move(
            direction *
            moveSpeed *
            Time.deltaTime
        );

        transform.forward = direction;
    }

    //==================================================
    //Randomly select patrol points
    //==================================================

    void PickNewPatrolPoint()
    {
        float randomX =
            Random.Range(minX, maxX);

        float randomZ =
            Random.Range(minZ, maxZ);

        patrolTarget = new Vector3(
            randomX,
            transform.position.y,
            randomZ
        );
    }

    //==================================================
    // Boss collides with player
    //==================================================

    private void OnControllerColliderHit(
        ControllerColliderHit hit)
    {
        if (!hit.gameObject.CompareTag("Player"))
            return;

        PlayerHealth health =
            hit.gameObject.GetComponent<PlayerHealth>();

        if (health == null)
            return;

        // Players who have fallen are no longer affected by the boss's attacks.
        if (health.IsDown)
            return;

        // Take damage
        health.TakeDamage(1);

        // The player was knocked down by this attack.
        if (health.IsDown)
        {
            targetPlayer = null;

            currentState = BossState.Patrol;

            targetLost = true;

            // Resume patrol immediately
            waiting = false;

            waitTimer = 0f;

            PickNewPatrolPoint();

            return;
        }

        // Defeat the player
        PlayerController player =
            hit.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            Vector3 dir =
                hit.transform.position -
                transform.position;

            dir.y = 0f;

            if (dir.magnitude > 0.01f)
            {
                dir.Normalize();

                player.KnockBack(
                    dir,
                    knockbackForce
                );
            }
        }
    }
}