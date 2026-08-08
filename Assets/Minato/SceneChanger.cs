using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public bool changeScene;
    [SerializeField] private string nextScene; // Scene to change to next set here

    public void Start()
    {
        changeScene = false;
    }

    public void Update()
    {
        if(changeScene) // When an event triggers changeScene to become true
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene); // Loads next scene
    }
}
