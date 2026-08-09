using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneCanvas : MonoBehaviour
{

    public bool continuePressed; // Checks if the Continue Button was pressed
    public SceneChanger sceneChanger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        continuePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(continuePressed)
        {
            Continue();
        }
    }

    public void Continue()
    {
        sceneChanger.changeScene = true;
    }

}
