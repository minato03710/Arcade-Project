using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public RectTransform fillBar;

    public TMP_Text healthText;

    [Header("每一点生命对应多少像素")]
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