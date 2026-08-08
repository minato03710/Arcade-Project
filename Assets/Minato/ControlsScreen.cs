using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsScreen : MonoBehaviour
{
    public bool objectivePressed; // Checks if the Objective Button was pressed
    public bool returnPressed; // Checks if the Return Button was pressed
    public SceneChanger sceneChanger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        objectivePressed = false; // Both buttons start not pressed
        returnPressed = false;
    }

    public void Update()
    {
        if(objectivePressed)
        {
            Objective();
        }

        if(returnPressed)
        {
            Return();
        }
    }


    public void ObjectiveButtonPressed() // Called by the Controls Button when pressed
    {
        objectivePressed = true;
    }

    public void ReturnButtonPressed() // Called by the Return Button when pressed
    {
        returnPressed = true;
    }

    public void Objective()
    {
        Debug.Log("Objective pressed");
    }

    public void Return()
    {
        Debug.Log("Entered return void");
        sceneChanger.changeScene = true;
    }

}
