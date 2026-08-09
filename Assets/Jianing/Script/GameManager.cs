using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private EndScreenHandler endScreenScript;
    public GameObject endScriptGameObject;
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;
    private bool gameOver = false;

    [Header("UI")]
    public TMP_Text scoreText;

    public int score = 0;  //I changed from private to public -Nathan

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

    void Update()
    {
        CheckGameOver();
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
    public void CheckGameOver()
    {
        if ((player1Health.currentHealth == 0) && (player2Health.currentHealth == 0) && !gameOver)
        {
            gameOver = true;

            Debug.Log("GAME OVER");

            //Time.timeScale = 0f;

            //endScreenScript.GameEnd();
            endScriptGameObject.SetActive(true);
        }
    }
}
