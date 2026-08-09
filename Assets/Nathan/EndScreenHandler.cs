using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VectorGraphics;

public class EndScreenHandler : MonoBehaviour
{
    //public bool gameEnded;

    //Setup:
    //  - ReplayButton = Inactive
    //  - MenuButton = Inactive
    //  - CanvasPoints = Inactive
    //  - CanvasBG = Inactive

    [SerializeField] private GameManager gameManagerScript; //Put gameobject that has the script cointaining the score. Change "PointsExample" to script name that has the score
    [SerializeField] string replaySceneName;
    [SerializeField] string menuSceneName;
    public int ScoreAmount; //The variable that represents the score taken from GameManager script
    public int ScoreAddition; //The final score divided by 110 (calculated in GameEnd() - Reason: The end screen counts up to the final score in 11 seconds, adding points every 0.10s
    public int finalScore = 0; //Score counter that will be used to count up to the final score
    public GameObject ReplayButton;
    public GameObject MenuButton;
    public GameObject CanvasPoints;
    public GameObject CanvasBG;
    public TextMeshProUGUI ScoreUI;
    public AudioSource AudioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEnd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameEnd()
    {
        Time.timeScale = 1f;
        CanvasBG.SetActive(true);
        CanvasPoints.SetActive(true);

        Debug.Log("Game Ended = TRUE");
        ScoreAmount = gameManagerScript.score;
        ScoreAddition = ScoreAmount / 110;
        Debug.Log("Scores calculated");

        StartCoroutine(AddPointValue());
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        Debug.Log("Clicked Replay Button");
        SceneManager.LoadScene(replaySceneName);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        Debug.Log("Clicked Menu Button");
        SceneManager.LoadScene(menuSceneName);
    }

    IEnumerator AddPointValue()
    {
        AudioSource.Play();
        while (finalScore < ScoreAmount)
        {
            finalScore += ScoreAddition;
            ScoreUI.text = finalScore.ToString();
            yield return new WaitForSecondsRealtime(0.10f);
        }

        finalScore = ScoreAmount;
        ScoreUI.text = finalScore.ToString();

        ReplayButton.SetActive(true);
        MenuButton.SetActive(true);
    }
}
