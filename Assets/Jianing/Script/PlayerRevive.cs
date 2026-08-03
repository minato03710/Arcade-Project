using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRevive : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayer1;

    [Header("References")]
    public PlayerHealth myHealth;
    public PlayerHealth teammateHealth;

    [Header("Revive")]
    public float reviveDistance = 2.5f;
    public float reviveTime = 3f;

    private float reviveTimer = 0f;

    void Update()
    {
        // 自己倒地不能救人
        if (myHealth.IsDown)
        {
            reviveTimer = 0;
            return;
        }

        // 队友没有倒地
        if (!teammateHealth.IsDown)
        {
            reviveTimer = 0;
            return;
        }

        // 距离不够
        float distance = Vector3.Distance(
            transform.position,
            teammateHealth.transform.position);

        if (distance > reviveDistance)
        {
            reviveTimer = 0;
            return;
        }

        // 判断互动键
        bool interact = false;

        if (isPlayer1)
        {
            interact = Keyboard.current.eKey.isPressed;
        }
        else
        {
            interact = Keyboard.current.numpad2Key.isPressed;
        }

        if (!interact)
        {
            reviveTimer = 0;
            return;
        }

        // 开始救援
        reviveTimer += Time.deltaTime;

        Debug.Log("Reviving... " + reviveTimer.ToString("F1"));

        if (reviveTimer >= reviveTime)
        {
            teammateHealth.Revive();

            GameManager.Instance.RemoveScore(50);

            reviveTimer = 0;

            Debug.Log("Revive Success!");
        }
    }
}