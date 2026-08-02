using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int score = 20;
    public float rotateSpeed = 120f;

    private bool collected = false;

    void Update()
    {
        // coin keep rotating
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            // add score
            GameManager.Instance.AddScore(score);

            // destroy coin
            Destroy(gameObject);
        }
    }
}
