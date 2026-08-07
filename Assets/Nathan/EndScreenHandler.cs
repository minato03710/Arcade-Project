using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class EndScreenHandler : MonoBehaviour
{
    //public bool gameEnded;

    //Setup:
    //  - ReplayButton = Inactive
    //  - MenuButton = Inactive
    //  - CanvasPoints = Inactive
    //  - CanvasBG = Inactive

    [SerializeField] private PointsExample gameManagerScript;
    public float ScoreAmount; //The variable that represents the score taken from GameManager script
    public float ScoreAddition; //The final score divided by 110 (calculated in GameEnd() - Reason: The end screen counts up to the final score in 11 seconds, adding points every 0.10s
    public float finalScore = 0; //Score counter that will be used to count up to the final score
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
        CanvasBG.SetActive(true);
        CanvasPoints.SetActive(true);

        Debug.Log("Game Ended = TRUE");
        ScoreAmount = gameManagerScript.score;
        ScoreAddition = ScoreAmount / 110;
        Debug.Log("Scores calculated");

        StartCoroutine(AddPointValue());
    }

    IEnumerator AddPointValue()
    {
        AudioSource.Play();
        while (finalScore < ScoreAmount)
        {
            finalScore += ScoreAddition;
            ScoreUI.text = finalScore.ToString();
            yield return new WaitForSeconds(0.10f);
        }

        finalScore = ScoreAmount;
        ScoreUI.text = finalScore.ToString();

        ReplayButton.SetActive(true);
        MenuButton.SetActive(true);
    }
}
