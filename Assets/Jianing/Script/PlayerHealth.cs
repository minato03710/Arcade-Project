using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;

    [HideInInspector]
    public int currentHealth;

    [Header("Damage")]
    public float invincibleTime = 1f;

    private float invincibleTimer;

    [HideInInspector]
    public bool IsDown = false;

    private PlayerController playerController;

    void Start()
    {
        currentHealth = maxHealth;

        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    //==========================
    // 受到伤害
    //==========================

    public void TakeDamage(int damage)
    {
        if (IsDown)
            return;

        if (invincibleTimer > 0)
            return;

        currentHealth -= damage;

        invincibleTimer = invincibleTime;

        Debug.Log(gameObject.name + " HP : " + currentHealth);

        if (currentHealth <= 0)
        {
            Down();
        }
    }

    //==========================
    // 倒地
    //==========================

    void Down()
    {
        IsDown = true;

        currentHealth = 0;

        Debug.Log(gameObject.name + " Down!");

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    //==========================
    // 复活
    //==========================

    public void Revive()
    {
        IsDown = false;

        currentHealth = maxHealth;

        invincibleTimer = 2f;

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        Debug.Log(gameObject.name + " Revived!");
    }

    //==========================
    // 加血（以后可用）
    //==========================

    public void Heal(int amount)
    {
        if (IsDown)
            return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
