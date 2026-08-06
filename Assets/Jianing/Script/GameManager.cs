using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score : " + score;
    }

    public int GetScore()
    {
        return score;
    }
    public void RemoveScore(int amount)
    {
        score -= amount;

        if (score < 0)
            score = 0;

        UpdateScoreUI();
    }
    public void CheckGameOver(PlayerHealth p1, PlayerHealth p2)
    {
        if (p1.IsDown && p2.IsDown)
        {
            Debug.Log("GAME OVER");

            Time.timeScale = 0f;
        }
    }
}
