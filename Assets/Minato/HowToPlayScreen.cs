using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlayScreen : MonoBehaviour
{
    public bool controlsPressed; // Checks if the Controls Button was pressed
    public bool returnPressed; // Checks if the Return Button was pressed
    public SceneChanger sceneChanger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlsPressed = false; // Both buttons start not pressed
        returnPressed = false;
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

    }

    public void Return()
    {
        sceneChanger.changeScene = true;
    }

}
