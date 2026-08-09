using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartCanvas : MonoBehaviour
{

    public SceneChanger sceneChangerOne;
    public SceneChanger sceneChangerTwo;

    public void StartPressed()
    {
        sceneChangerOne.changeScene = true;
    }

    public void HowToPlayPressed()
    {
        sceneChangerTwo.changeScene = true;
    }

    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit game");
    }

}
