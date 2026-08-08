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


    private float attackTimer = 0f;

    private DamageableObject currentObject;

    private NPCController currentNPC;


    void Update()
    {
        bool attack = false;


        //========================================
        // Check keyboard
        //========================================

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
            ResetAttack();

            return;
        }


        //========================================
        // Find target
        //========================================

        FindTarget();


        //========================================
        // No target
        //========================================

        if (currentObject == null &&
            currentNPC == null)
        {
            ResetAttack();

            return;
        }


        //========================================
        // Increase attack timer
        //========================================

        attackTimer += Time.deltaTime;


        //========================================
        // Get attack duration
        //========================================

        float attackDuration = GetAttackDuration();


        // Prevent division by zero
        if (attackDuration <= 0f)
        {
            attackDuration = 0.1f;
        }


        //========================================
        // Update progress bar
        //========================================

        if (progressUI != null)
        {
            float progress =
                attackTimer / attackDuration;

            progress = Mathf.Clamp01(progress);

            progressUI.Show(progress);
        }


        //========================================
        // Attack completed
        //========================================

        if (attackTimer >= attackDuration)
        {
            CompleteAttack();
        }
    }


    //==================================================
    // Find Target
    //==================================================

    void FindTarget()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                attackRange
            );


        DamageableObject newObject = null;

        NPCController newNPC = null;


        foreach (Collider hit in hits)
        {
            // Check for DamageableObject
            newObject =
                hit.GetComponent<DamageableObject>();


            // If not found, check parent
            if (newObject == null)
            {
                newObject =
                    hit.GetComponentInParent<DamageableObject>();
            }


            // Check if the DamageableObject is active
            if (newObject != null &&
                !newObject.enabled)
            {
                newObject = null;
            }


            // Check for NPCController
            newNPC =
                hit.GetComponent<NPCController>();


            // If not found, check parent
            if (newNPC == null)
            {
                newNPC =
                    hit.GetComponentInParent<NPCController>();
            }


            // Ignore already stopped NPCs
            if (newNPC != null &&
                newNPC.IsStopped())
            {
                newNPC = null;
            }


            //========================================
            // Prefer DamageableObject
            //========================================

            if (newObject != null)
            {
                currentObject = newObject;

                currentNPC = null;

                break;
            }


            //========================================
            // Otherwise use NPC
            //========================================

            if (newNPC != null)
            {
                currentNPC = newNPC;

                currentObject = null;

                break;
            }
        }
    }


    //==================================================
    // Get Attack Duration
    //==================================================

    float GetAttackDuration()
    {
        // Normal object or Machine
        if (currentObject != null)
        {
            return currentObject.destroyTime;
        }


        // NPC
        if (currentNPC != null)
        {
            return currentNPC.attackTime;
        }


        return 1f;
    }


    //==================================================
    // Complete Attack
    //==================================================

    void CompleteAttack()
    {
        //========================================
        // Attack normal object or Machine
        //========================================

        if (currentObject != null)
        {
            Debug.Log(
                "Attacking object: " +
                currentObject.gameObject.name
            );


            currentObject.DestroyObject();


            PlayAttackSound();


            ResetAttack();

            return;
        }


        //========================================
        // Attack NPC
        //========================================

        if (currentNPC != null)
        {
            Debug.Log(
                "Attacking NPC: " +
                currentNPC.gameObject.name
            );


            currentNPC.StopNPC();


            PlayAttackSound();


            ResetAttack();

            return;
        }
    }


    //==================================================
    // Reset Attack
    //==================================================

    void ResetAttack()
    {
        attackTimer = 0f;

        currentObject = null;

        currentNPC = null;


        if (progressUI != null)
        {
            progressUI.Hide();
        }
    }


    //==================================================
    // Play Attack Sound
    //==================================================

    void PlayAttackSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }


    //==================================================
    // Attack Range Gizmo
    //==================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}