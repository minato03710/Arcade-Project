using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayer1;

    [Header("Attack")]
    public float attackRange = 2f;

    [Header("Progress UI")]
    public ProgressBarUI progressUI;

    [Header("Audio")]
    public AudioSource audioSource;


    private float attackTimer;

    private DamageableObject currentObject;


    void Update()
    {
        bool attack = false;


        // Check if the keyboard is available
        if (Keyboard.current == null)
            return;


        // Player 1 uses Q
        if (isPlayer1)
        {
            attack = Keyboard.current.qKey.isPressed;
        }
        // Player 2 uses Numpad 3
        else
        {
            attack = Keyboard.current.numpad3Key.isPressed;
        }


        //========================================
        // Stop attacking
        //========================================

        if (!attack)
        {
            attackTimer = 0f;

            currentObject = null;

            if (progressUI != null)
                progressUI.Hide();

            return;
        }


        //========================================
        // Find objects inside attack range
        //========================================

        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                attackRange
            );


        DamageableObject target = null;


        foreach (Collider hit in hits)
        {
            target =
                hit.GetComponent<DamageableObject>();


            if (target == null)
            {
                target =
                    hit.GetComponentInParent<DamageableObject>();
            }


            // Only allow interaction with active DamageableObjects
            if (target != null && target.enabled)
            {
                break;
            }


            target = null;
        }


        //========================================
        // No target found
        //========================================

        if (target == null)
        {
            attackTimer = 0f;

            currentObject = null;

            if (progressUI != null)
                progressUI.Hide();

            return;
        }


        //========================================
        // New target
        //========================================

        if (currentObject != target)
        {
            currentObject = target;

            attackTimer = 0f;

            Debug.Log(
                "New attack target: " +
                currentObject.gameObject.name
            );
        }


        //========================================
        // Attack timer
        //========================================

        attackTimer += Time.deltaTime;


        //========================================
        // Update progress bar
        //========================================

        if (progressUI != null)
        {
            float progress =
                attackTimer /
                currentObject.destroyTime;

            progressUI.Show(progress);
        }


        //========================================
        // Destroy / Damage object
        //========================================

        if (attackTimer >= currentObject.destroyTime)
        {
            Debug.Log(
                "Attacking object: " +
                currentObject.gameObject.name
            );


            currentObject.DestroyObject();


            // Play attack sound
            if (audioSource != null)
            {
                audioSource.Play();
            }


            // Reset attack timer
            attackTimer = 0f;


            // Keep the same target if it still exists
            if (currentObject != null)
            {
                // If the object has been destroyed,
                // Unity will make this reference null.
                if (currentObject == null)
                {
                    currentObject = null;

                    if (progressUI != null)
                        progressUI.Hide();
                }
            }
        }
    }


    //========================================
    // Attack Range Gizmo
    //========================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}