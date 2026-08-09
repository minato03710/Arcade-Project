using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlayScreen : MonoBehaviour
{
    public bool controlsPressed; // Checks if the Controls Button was pressed
    public bool returnPressed; // Checks if the Return Button was pressed
    public SceneChanger sceneChanger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        controlsPressed = false; // Both buttons start not pressed
        returnPressed = false;
    }

    public void Update()
    {
        if(controlsPressed)
        {
            Controls();
        }

        if(returnPressed)
        {
            Return();
        }
    }

    public void ControlsButtonPressed() // Called by the Controls Button when pressed
    {
        controlsPressed = true;
    }

    public void ReturnButtonPressed() // Called by the Return Button when pressed
    {
        returnPressed = true;
    }

    public void Controls()
    {
        gameObject.SetActive(false);
        controlsPressed = false;
    }

    public void Return()
    {
        sceneChanger.changeScene = true;
    }

}
