using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayer1;

    [Header("Attack")]
    public float attackRange = 2f;

    public ProgressBarUI progressUI;

    private float attackTimer;

    private DamageableObject currentObject;

    void Update()
    {
        bool attack = false;

        if (Keyboard.current == null)
            return;

        if (isPlayer1)
            attack = Keyboard.current.qKey.isPressed;
        else
            attack = Keyboard.current.numpad3Key.isPressed;

        if (!attack)
        {
            attackTimer = 0;

            currentObject = null;

            progressUI.Hide();

            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        DamageableObject target = null;

        foreach (Collider hit in hits)
        {
            target = hit.GetComponent<DamageableObject>();

            if (target != null)
                break;
        }

        if (target == null)
        {
            attackTimer = 0;

            currentObject = null;

            progressUI.Hide();

            return;
        }

        if (currentObject != target)
        {
            currentObject = target;

            attackTimer = 0;
        }

        attackTimer += Time.deltaTime;

        progressUI.Show(attackTimer / currentObject.destroyTime);

        if (attackTimer >= currentObject.destroyTime)
        {
            currentObject.DestroyObject();

            attackTimer = 0;

            currentObject = null;

            progressUI.Hide();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}