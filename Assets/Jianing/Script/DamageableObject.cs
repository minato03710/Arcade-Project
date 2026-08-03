using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    public int maxHealth = 10;

    [Header("Score")]
    public int scoreReward = 20;
    private int currentHealth;

    private float timer;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damagePerSecond)
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timer = 0f;

            currentHealth -= Mathf.RoundToInt(damagePerSecond);

            Debug.Log(gameObject.name + " HP : " + currentHealth);

            if (currentHealth <= 0)
            {
                GameManager.Instance.AddScore(scoreReward);
                Destroy(gameObject);
            }
        }
    }
}
