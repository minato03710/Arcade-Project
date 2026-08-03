using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player")]
    public bool isPlayer1;

    [Header("Attack")]
    public float attackRange = 2f;

    void Update()
    {
        bool attack = false;

        if (Keyboard.current == null)
            return;

        if (isPlayer1)
        {
            attack = Keyboard.current.qKey.isPressed;
        }
        else
        {
            attack = Keyboard.current.numpad3Key.isPressed;
        }

        if (!attack)
            return;

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hit in hits)
        {
            DamageableObject obj = hit.GetComponent<DamageableObject>();

            if (obj == null)
                continue;

            obj.Damage(1f);

            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
