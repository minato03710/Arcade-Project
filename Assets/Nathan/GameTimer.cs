using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    public GameObject boss1;
    public GameObject boss2;
    public GameObject boss3;
    public GameObject boss4;
    public GameObject boss5;

    public AudioSource BGMusic1;
    public AudioSource BGMusic2;
    public AudioSource ThirtySeconds;
    public AudioSource ClockSFX;
    public AudioSource EndSFX;

    private bool gameOver = false;
    private bool bool8sec = false;
    private bool bool30sec = false;
    private bool bool60sec = false;
    private bool bool90sec = false;
    private bool bool120sec = false;
    private bool bool150sec = false;


    // Update is called once per frame
    void Update()
    {
        if (!gameOver && remainingTime <= 0)
        {
            gameOver = true;
            remainingTime = 0;

            bool150sec = true;
            bool120sec = true;
            bool90sec = true;
            bool60sec = true;
            bool30sec = true;
            bool8sec = true;

            BGMusic1.Stop();
            BGMusic2.Stop();
            //EndSFX.Play();
            player1Health.currentHealth = 0;
            player2Health.currentHealth = 0;
            Debug.Log("Timer Ended, Game over");
        }
        else if ((player1Health.currentHealth == 0) && (player2Health.currentHealth == 0))
        {
            remainingTime = 0;
        }

        else if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime < 8 && !bool8sec)
            {
                bool8sec = true;
                Debug.Log("8 seconds left");
                ClockSFX.Play();
            }

            else if (remainingTime < 30 && !bool30sec)
            {
                bool30sec = true;
                Debug.Log("30 seconds left");
                boss5.SetActive(true);
                ThirtySeconds.Play();
                BGMusic1.Stop();
                BGMusic2.Play();
            }
            else if (remainingTime < 60 && !bool60sec)
            {
                bool60sec = true;
                Debug.Log("60 seconds left");
                boss4.SetActive(true);
            }
            else if (remainingTime < 90 && !bool90sec)
            {
                bool90sec = true;
                Debug.Log("90 seconds left");
                boss3.SetActive(true);
            }
            else if (remainingTime < 120 && !bool120sec)
            {
                bool120sec = true;
                Debug.Log("120 seconds left");
                boss2.SetActive(true);
            }
            else if (remainingTime < 150 && !bool150sec)
            {
                bool150sec = true;
                Debug.Log("150 seconds left");
                boss1.SetActive(true);
            }
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        

        

    }
}
