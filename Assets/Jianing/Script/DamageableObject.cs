using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;

    [Header("Destroy Time")]
    public float destroyTime = 3f;

    [Header("Score")]
    public int scoreReward = 20;

    public void DestroyObject()
    {
        GameManager.Instance.AddScore(scoreReward);

        Debug.Log(gameObject.name + " Destroyed!");

        Destroy(gameObject);
    }
}
