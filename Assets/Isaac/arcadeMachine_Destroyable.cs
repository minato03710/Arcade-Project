using UnityEngine;

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
    public AudioClip destroySFX;
    public AudioClip electricLoopSFX;

    private bool isBroken = false;


    void Start()
    {
        // Show the normal model when the game starts
        if (normalModel != null)
            normalModel.SetActive(true);

        // Hide the broken model when the game starts
        if (brokenModel != null)
            brokenModel.SetActive(false);

        // Disable smoke when the game starts
        if (smokeVFX != null)
            smokeVFX.SetActive(false);

        // Disable sparks when the game starts
        if (sparksVFX != null)
            sparksVFX.gameObject.SetActive(false);
    }


    public void BreakMachine()
    {
        if (isBroken)
            return;

        isBroken = true;

        Debug.Log("Machine Broken!");


        //========================================
        // 1. Swap Models
        //========================================

        if (normalModel != null)
            normalModel.SetActive(false);

        if (brokenModel != null)
            brokenModel.SetActive(true);


        //========================================
        // 2. Play Sparks VFX
        //========================================

        if (sparksVFX != null)
        {
            sparksVFX.gameObject.SetActive(true);

            sparksVFX.Play();
        }


        //========================================
        // 3. Play Smoke VFX
        //========================================

        if (smokeVFX != null)
        {
            smokeVFX.SetActive(true);
        }


        //========================================
        // 4. Play Destruction Sound
        //========================================

        if (audioSource != null && destroySFX != null)
        {
            audioSource.PlayOneShot(destroySFX);
        }


        //========================================
        // 5. Play Continuous Electric Sound
        //========================================

        if (audioSource != null && electricLoopSFX != null)
        {
            audioSource.clip = electricLoopSFX;

            audioSource.loop = true;

            audioSource.Play();
        }
    }
}