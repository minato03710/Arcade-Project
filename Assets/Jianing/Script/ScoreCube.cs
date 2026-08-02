using UnityEngine;

public class ScoreCube : MonoBehaviour
{
    public int score = 10;

    private bool collected = false;

    private Renderer cubeRenderer;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            GameManager.Instance.AddScore(score);

            // 变成绿色，表示已经收集
            cubeRenderer.material.color = Color.green;
        }
    }
}
