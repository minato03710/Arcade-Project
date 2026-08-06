using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadeMachine : MonoBehaviour
{
    [Header("Model References")]
    public GameObject normalModel;
    public GameObject brokenModel;

    [Header("VFX References")]
    public ParticleSystem sparksVFX;
    public GameObject smokeVFX;

    [Header("SFX References")]
    public AudioSource audioSource;
    public AudioClip destroySFX;       // Instant crash / glass break
    public AudioClip electricLoopSFX;  // (Optional) Continuous buzzing sound after it breaks

    private bool isBroken = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame && !isBroken)
        {
            BreakMachine();
        }
    }

    public void BreakMachine()
    {
        if (isBroken) return;
        isBroken = true;

        // 1. Swap Models
        if (normalModel != null) normalModel.SetActive(false);
        if (brokenModel != null) brokenModel.SetActive(true);

        // 2. Trigger VFX
        if (sparksVFX != null) sparksVFX.Play();
        if (smokeVFX != null) smokeVFX.SetActive(true);

        // 3. Play Break SFX
        if (audioSource != null && destroySFX != null)
        {
            audioSource.PlayOneShot(destroySFX);
        }

        // 4. (Optional) Start continuous electric buzz after breaking
        if (audioSource != null && electricLoopSFX != null)
        {
            audioSource.clip = electricLoopSFX;
            audioSource.loop = true;
            audioSource.Play(); // Starts looping the buzz sound!
        }
    }
}