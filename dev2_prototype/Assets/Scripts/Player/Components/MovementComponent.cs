using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Zombies
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] CharacterController Controller;
        [SerializeField] float MoveSpeed;
        [SerializeField] float AirMoveSpeedReduction;
        [SerializeField] float JumpPower;
        [SerializeField] float CrouchHeight;
        [SerializeField] float StandHeight;
        
        // Player movement vector.
        Vector3 movement;

        // Jump/Falling variables.
        float fallVelocity;
        float moveGravity = Physics.gravity.y;

        // Helpful to view in the editor.
        bool isGrounded;

        void Start()
        {

        }

        void Update()
        {
            // Handle base movement.
            HandleMove();

            // Handle crouching.
            CrouchPlayer();

            // TODO: Sprinting & better strafing.
        }

        // Handle base movement.

        void HandleMove()
        {
            // I am 90% sure we need to verify this with a Raycast check against the ground. IsGrounded doesn't seem to get set while standing still?
            isGrounded = Controller.isGrounded;

            // Stop trying to fall if we are already on the ground.
            if (fallVelocity < 0f && isGrounded)
                fallVelocity = 0f;

            // Handle walking.
            movement = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
            movement *= MoveSpeed;

            // If we are in the air slow down our speed a little bit. (Ideally I should probably just do this to the Vertical and Horizontal axis')
            if (!isGrounded || fallVelocity > 0f)
                movement *= AirMoveSpeedReduction;

            // Handle jump. (I need to figure out what formula COD/Quake uses for their jumping.) They have console commands like sv_gravity to handle their movement.
            if (Input.GetButtonDown("Jump") && isGrounded)
                fallVelocity = JumpPower;

            // Apply gravity.
            fallVelocity += moveGravity * Time.deltaTime;

            // Set our falling velocity.
            movement.y = fallVelocity;

            // Move the character.
            Controller.Move(movement * Time.deltaTime);
        }

        // Handling crouching.
        void CrouchPlayer()
        {
            // Simple "crouch" just sets the character controller height.
            Controller.height = Input.GetButton("Crouch") ? CrouchHeight : StandHeight;
        }
    }
}