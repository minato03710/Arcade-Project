using UnityEngine;
using UnityEngine.InputSystem;

public class CoinPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public AudioClip coinSound;       // Drag your .wav / .mp3 audio file directly here
    public GameObject coinVfxPrefab; // Drag your particle prefab here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Play Audio at camera position (Guarantees full volume without 3D distance fading)
            if (coinSound != null)
            {
                Vector3 soundPos = Camera.main != null ? Camera.main.transform.position : transform.position;
                AudioSource.PlayClipAtPoint(coinSound, soundPos, 1.0f);
            }

            // 2. Spawn Particle VFX
            if (coinVfxPrefab != null)
            {
                Instantiate(coinVfxPrefab, transform.position, Quaternion.identity);
            }

            // 3. Destroy the coin
            Destroy(gameObject);
        }
    }


 }
