using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerType//Player 1 is a lemur, Player 2 is a bird.
    {
        Player1,
        Player2
    }

    [Header("Player")]
    public PlayerType playerType;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public float flySpeed = 6f;
    public float turnSpeed = 15f;

    private CharacterController controller;

    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

     
        // ground test
       

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = Vector3.zero;

       
        // Player1
       

        if (playerType == PlayerType.Player1)
        {
            if (keyboard.aKey.isPressed)
                move.x = -1;

            if (keyboard.dKey.isPressed)
                move.x = 1;

            if (keyboard.wKey.isPressed)
                move.z = 1;

            if (keyboard.sKey.isPressed)
                move.z = -1;

            move.Normalize();

            if (keyboard.spaceKey.wasPressedThisFrame && controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        
        // Player2
      

        else
        {
            if (keyboard.leftArrowKey.isPressed)
                move.x = -1;

            if (keyboard.rightArrowKey.isPressed)
                move.x = 1;

            if (keyboard.upArrowKey.isPressed)
                move.z = 1;

            if (keyboard.downArrowKey.isPressed)
                move.z = -1;

            move.Normalize();

            // 1 to fly
            if (keyboard.numpad1Key.isPressed)
            {
                velocity.y = flySpeed;
            }
        }

        if (move != Vector3.zero)
        {
            // Calculate the rotation needed to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            
            // Smoothly blend from current rotation to target rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        
        // gravity
       

        velocity.y += gravity * Time.deltaTime;

        
        // moving
       

        controller.Move(move * moveSpeed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);

       
        // face to move
        

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
    }
    public void KnockBack(Vector3 direction, float force)
    {
        CharacterController controller = GetComponent<CharacterController>();

        controller.Move(direction.normalized * force);

    }
    [Header("Physics Interaction")]
    [Tooltip("How hard the player pushes Rigidbody objects")]
    public float pushPower = 2.0f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // Make sure the object has a Rigidbody and isn't static (kinematic)
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We don't want to push objects below us (like the floor)
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        // Calculate push direction from move direction (only on X and Z axes)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push force to the Rigidbody object
        body.linearVelocity = pushDir * pushPower;
    }
}