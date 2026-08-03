using UnityEngine;
using UnityEngine.InputSystem; // Added New Input System namespace

public class CoinPickup : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Coin pickup SFX played!");
        }
    }

    private void Update()
    {
        // New Input System syntax for checking Spacebar press
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (audioSource != null) audioSource.Play();
        }
    }
}