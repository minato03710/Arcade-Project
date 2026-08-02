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

    [Header("Target Switch")]
    public float switchThreshold = 2f;

    [Header("Attack")]
    public int damageScore = 10;
    public float knockbackForce = 2f;

    [Header("Patrol Wait")]
    public float waitTime = 2f;

    private CharacterController controller;

    private BossState currentState = BossState.Patrol;

    private Transform targetPlayer;

    private Vector3 patrolTarget;

    private bool waiting;

    private float waitTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();

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

    //------------------------------------------------
    // 更新目标
    //------------------------------------------------

    void UpdateTarget()
    {
        float d1 = Vector3.Distance(transform.position, player1.position);
        float d2 = Vector3.Distance(transform.position, player2.position);

        Transform nearest = d1 < d2 ? player1 : player2;

        float nearestDistance = Mathf.Min(d1, d2);

        if (currentState == BossState.Patrol)
        {
            if (nearestDistance <= detectRange)
            {
                targetPlayer = nearest;
                currentState = BossState.Chase;
            }
        }
        else
        {
            if (nearestDistance > loseRange)
            {
                currentState = BossState.Patrol;
                targetPlayer = null;
                PickNewPatrolPoint();
                return;
            }

            if (targetPlayer != null)
            {
                float currentDistance =
                    Vector3.Distance(transform.position, targetPlayer.position);

                if (currentDistance - nearestDistance > switchThreshold)
                {
                    targetPlayer = nearest;
                }
            }
        }
    }

    //------------------------------------------------
    // 巡逻
    //------------------------------------------------

    void Patrol()
    {
        if (waiting)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waiting = false;
                PickNewPatrolPoint();
            }

            return;
        }

        MoveTo(patrolTarget);

        if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            waiting = true;
            waitTimer = 0;
        }
    }

    //------------------------------------------------
    // 追玩家
    //------------------------------------------------

    void Chase()
    {
        if (targetPlayer == null)
            return;

        MoveTo(targetPlayer.position);
    }

    //------------------------------------------------
    // 公共移动
    //------------------------------------------------

    void MoveTo(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        direction.y = 0;

        if (direction.magnitude < 0.1f)
            return;

        direction.Normalize();

        controller.Move(direction * moveSpeed * Time.deltaTime);

        transform.forward = direction;
    }

    //------------------------------------------------
    // 新巡逻点
    //------------------------------------------------

    void PickNewPatrolPoint()
    {
        patrolTarget = new Vector3(
            Random.Range(minX, maxX),
            transform.position.y,
            Random.Range(minZ, maxZ));
    }

    //------------------------------------------------
    // 碰撞玩家
    //------------------------------------------------

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.gameObject.CompareTag("Player"))
            return;

        GameManager.Instance.RemoveScore(damageScore);

        PlayerController player =
            hit.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            Vector3 dir =
                hit.transform.position - transform.position;

            dir.y = 0;

            player.KnockBack(dir, knockbackForce);
        }
    }
}