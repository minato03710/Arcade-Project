using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [Header("Slider")]
    public Slider slider;

    private void Start()
    {
        Hide();
    }

    public void Show(float progress)
    {
        gameObject.SetActive(true);

        slider.value = Mathf.Clamp01(progress);
    }

    public void Hide()
    {
        slider.value = 0;

        gameObject.SetActive(false);
    }
}
