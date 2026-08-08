using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;

    [Header("Destroy Time")]
    public float destroyTime = 3f;

    [Header("Score")]
    public int scoreReward = 20;

    // Prevent the object from being interacted with after destruction
    private bool isDestroyed = false;


    public void DestroyObject()
    {
        // Stop interaction if the object has already been destroyed
        if (isDestroyed)
            return;


        Debug.Log("meow");


        // Reduce health by 1
        maxHealth -= 1;

        Debug.Log("Current Health: " + maxHealth);


        //========================================
        // Check if health reaches zero
        //========================================

        if (maxHealth <= 0)
        {
            Debug.Log("Destroyed Object");


            //========================================
            // Check if this object is a Machine
            //========================================

            ArcadeMachine machine =
                GetComponent<ArcadeMachine>();


            if (machine != null)
            {
                Debug.Log("This is a Machine!");


                // Mark the object as destroyed
                isDestroyed = true;


                // Trigger the Machine's destruction effect
                machine.BreakMachine();


                // Add score
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddScore(scoreReward);
                }


                // Disable further interaction
                enabled = false;


                return;
            }


            //========================================
            // Normal destroyable object
            //========================================

            isDestroyed = true;


            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreReward);
            }


            Debug.Log(gameObject.name + " Destroyed!");


            Destroy(gameObject);
        }
    }
}
