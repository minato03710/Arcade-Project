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
    private bool targetLost = false;
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
        // 如果刚刚击倒玩家，就保持巡逻
        if (targetLost)
        {
            currentState = BossState.Patrol;

            // 当Boss到达新的巡逻点之后，再允许重新寻找目标
            if (!waiting)
                return;
        }

        PlayerHealth p1Health = player1.GetComponent<PlayerHealth>();
        PlayerHealth p2Health = player2.GetComponent<PlayerHealth>();

        bool p1Alive = p1Health != null && !p1Health.IsDown;
        bool p2Alive = p2Health != null && !p2Health.IsDown;

        // 两个玩家都倒地
        if (!p1Alive && !p2Alive)
        {
            targetPlayer = null;
            currentState = BossState.Patrol;
            return;
        }

        float d1 = p1Alive ? Vector3.Distance(transform.position, player1.position) : Mathf.Infinity;
        float d2 = p2Alive ? Vector3.Distance(transform.position, player2.position) : Mathf.Infinity;

        float nearestDistance = Mathf.Min(d1, d2);

        if (nearestDistance > detectRange)
        {
            targetPlayer = null;
            currentState = BossState.Patrol;
            return;
        }

        targetPlayer = d1 < d2 ? player1 : player2;
        currentState = BossState.Chase;

        targetLost = false;
    }

    //------------------------------------------------
    // 巡逻
    //------------------------------------------------

    void Patrol()
    {
        if (waiting)
        {
            if (waitTimer >= waitTime)
            {
                waiting = false;

                // 巡逻结束后允许重新寻找目标
                targetLost = false;

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
        {
            currentState = BossState.Patrol;
            return;
        }

        PlayerHealth health = targetPlayer.GetComponent<PlayerHealth>();

        if (health != null && health.IsDown)
        {
            targetPlayer = null;

            currentState = BossState.Patrol;

            targetLost = true;

            waiting = false;
            waitTimer = 0f;

            PickNewPatrolPoint();

            return;
        }

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

        PlayerHealth health = hit.gameObject.GetComponent<PlayerHealth>();

        if (health != null)
        {
            if (!health.IsDown)
            {
                health.TakeDamage(1);

                // 如果这一击让玩家倒地
                if (health.IsDown)
                {
                    targetPlayer = null;

                    currentState = BossState.Patrol;

                    targetLost = true;

                    // 立即开始巡逻
                    waiting = false;
                    waitTimer = 0f;

                    PickNewPatrolPoint();

                    return;
                }
            }
        }

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