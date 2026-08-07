using UnityEngine;

public class EndScreenHandler : MonoBehaviour
{
    public bool gameEnded;
    public GameObject PointsHolder;
    public float PointsAmount;
    public float PointsAddition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnGameEnd(bool gameEnded)
    {
        if (gameEnded == true)
        {
            
        }
    }


}
