using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public RectTransform fillBar;

    public TMP_Text healthText;

    [Header("How many pixels correspond to each unit of life")]
    public float healthWidth = 40f;

    void Update()
    {
        if (playerHealth == null)
            return;

        float width = playerHealth.currentHealth * healthWidth;

        fillBar.sizeDelta = new Vector2(width, fillBar.sizeDelta.y);

        if (healthText != null)
        {
            healthText.text =
                playerHealth.currentHealth + " / " + playerHealth.maxHealth;
        }
    }
}