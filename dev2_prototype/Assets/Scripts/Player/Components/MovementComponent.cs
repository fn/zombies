using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

namespace Zombies
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] CharacterController Controller;
        
        public float CrouchHeight;
        public float StandHeight;

        public float GravityAmount;

        public float Friction;
        public float Acceleration;
        public float Deceleration;
        public float AirAcceleration;

        public float CrouchFriction;
        public float CrouchAcceleration;
        public float CrouchDeceleration;

        public float SprintSpeed;
        public float WalkSpeed;
        public float CrouchSpeed;
        public float JumpPower;

        [SerializeField] bool ClampAirSpeed;
        [SerializeField] float MaxSpeed;

        public bool IsSprinting { get; private set; }
        public bool IsCrouching { get; private set; } 
        public bool IsJumping { get; private set; }

        struct MovementData
        {
            public Vector3 Position;
            public Vector3 Velocity;

            // Used the same as in Quake, COD, Source Engine, etc.
            public float ForwardMove;
            public float SideMove;
            
            public float GravityScale;
            public float FrictionFactor;

            public float VerticalAxis;
            public float HorizontalAxis;

            public bool InDuck;
            public bool InJump;
            public bool Sprinting;

            // Normal for the surface we are standing on.
            public Vector3 SurfaceNormal;
        }

        MovementData data = new MovementData();

        // Helpful to view in the editor.
        bool isGrounded;

        void Start()
        {
            data.Position = transform.position;
            data.GravityScale = 1f;
            data.FrictionFactor = 1f;
        }

        void Update()
        {
            UpdateMoveInput();

            // Handle movement.
            HandleMove();
        }

        // Used to check for collisions on our capsule with the ground.
        private bool CheckHitFloor(out RaycastHit outHit)
        {
            var start = data.Position;
            var end = new Vector3(data.Position.x, data.Position.y - 0.2f, data.Position.z);

            // Get edge points of capsule.
            var distance = (Controller.height * 0.5f) - Controller.radius;
            var top = start + Controller.center + Vector3.up * distance;
            var bottom = start + Controller.center - Vector3.up * distance;

            // Colliders whose distance is less than the sum of their contactOffset values will generate contacts
            // So we want to add this on to our max distance to allow for more possible collisions.
            var longSide = Mathf.Sqrt(Controller.contactOffset * Controller.contactOffset + Controller.contactOffset * Controller.contactOffset);

            var delta = end - start;
            var direction = delta.normalized;
            var maxDistance = delta.magnitude + longSide;

            // Finally check if we hit something.
            if (Physics.CapsuleCast(top, bottom, Controller.radius, direction, out RaycastHit hit, maxDistance))
            {
                // Good we did. Grab our RaycastHit info.
                outHit = hit;
                return true;
            }

            // Looks like we didn't.
            outHit = default;
            return false;
        }

        bool CheckGrounded()
        {
            // Check that we hit the floor. And If we did update our current surface normal.
            bool onGround = CheckHitFloor(out RaycastHit hit);
            if (onGround)
                data.SurfaceNormal = hit.normal;

            return onGround;
        }

        void UpdateMoveInput()
        {
            // We want the unsmoothed axis values so we use the Raw variant.
            data.VerticalAxis = Input.GetAxisRaw("Vertical");
            data.HorizontalAxis = Input.GetAxisRaw("Horizontal");

            // Check sprinting our crouching.
            data.Sprinting = Input.GetButton("Sprint");
            data.InDuck = Input.GetButton("Crouch");
            
            // We only want to set jump to true starting the frame we hit it.
            // However if we let go of the key after that we set it back to false.
            data.InJump = Input.GetButtonDown("Jump");
            if (!Input.GetButton("Jump"))
                data.InJump = false;

            bool inLeft = data.HorizontalAxis < 0f;
            bool inRight = data.HorizontalAxis > 0f;
            bool inForward = data.VerticalAxis > 0f;
            bool inBack = data.VerticalAxis < 0f;

            // Set side move and forward move. (Used in air)
            data.SideMove = (!inLeft && !inRight) ? 0f : (inLeft ? -Acceleration : inRight ? Acceleration : 0f);
            data.ForwardMove = (!inForward && !inBack) ? 0f : (inForward ? Acceleration : inBack ? -Acceleration : 0f);
        }

        void HandleMove()
        {
            // Used for tracing.
            data.Position = transform.position;

            // If our y velocity is now zero we are no longer jumping.
            if (data.Velocity.y <= 0f)
                IsJumping = false;

            // Apply gravity.
            if (!isGrounded)
                data.Velocity.y -= (data.GravityScale * GravityAmount * Time.deltaTime);

            // Check if we are on ground then setup our velocity
            isGrounded = CheckGrounded();
            SetupVelocity();

            // Move the character by our velocity.
            Controller.Move(data.Velocity * Time.deltaTime);
        }

        void SetupVelocity()
        {
            // Handle air movement.
            if (!isGrounded)
            {
                HandleAirMove();
                return;
            }

            float accel = data.InDuck ? CrouchAcceleration : Acceleration;
            float speed = data.Sprinting ? SprintSpeed : WalkSpeed;
            if (data.InDuck)
                speed = CrouchSpeed;

            IsCrouching = data.InDuck;
            IsSprinting = data.Sprinting;

            // Apply friction and jump
            if (data.InJump && !IsJumping)
            {
                ApplyFriction(0.0f, true);
                HandleJump();
                return;
            }

            // Otherwise apply our normal friction and handle the rest of our movement.
            ApplyFriction(1.0f, true);

            // Get axis values
            var forwardMove = data.VerticalAxis;
            var rightMove = data.HorizontalAxis;

            // Get movement directions from our surface normal.
            var forward = Vector3.Cross(data.SurfaceNormal, -transform.right);
            var right = Vector3.Cross(data.SurfaceNormal, forward);

            var wishdir = (forwardMove * forward + rightMove * right).normalized;

            // Set the target speed of the player
            float wishSpeed = wishdir.magnitude;
            wishSpeed *= speed;

            // Backup our Y velocity Apply accleration
            float backupY = data.Velocity.y;
            ApplyAcceleration(wishdir, wishSpeed, accel, false);

            // Clamp our velocity
            float maxMagnitude = 50f; // TODO: Make this serialized.
            data.Velocity = Vector3.ClampMagnitude(new Vector3(data.Velocity.x, 0f, data.Velocity.z), maxMagnitude);

            // Restore our Y velocity.
            data.Velocity.y = backupY;
        }

        void ApplyAcceleration(Vector3 wishDir, float wishSpeed, float acceleration, bool affectY)
        {
            float currentSpeed = Vector3.Dot(data.Velocity, wishDir);
            float delta = wishSpeed - currentSpeed;
            if (delta <= 0)
                return;

            float newSpeed = Mathf.Min(acceleration * Time.deltaTime * wishSpeed, delta);

            // Update our velocity.
            data.Velocity.x += newSpeed * wishDir.x;
            data.Velocity.z += newSpeed * wishDir.z;

            // Apply our acceleration to the Y axis only when we want to.
            if (affectY)
                data.Velocity.y += newSpeed * wishDir.y;
        }

        void ApplyFriction(float amount, bool isGrounded)
        {
            Vector3 vel = data.Velocity;

            // We just want our speed as apart of our xz components.
            vel.y = 0.0f;

            // Get our current speed.
            float speed = vel.magnitude;

            // Drop is going to what speed we lose due to friction.
            float drop = 0f;
            float friction = data.InDuck ? CrouchFriction : Friction;
            float deceleration = data.InDuck ? CrouchDeceleration : Deceleration;

            if (isGrounded)
            {
                // Calculate what we are going to remove from our speed.
                float control = Mathf.Max(deceleration, speed);
                drop = control * friction * Time.deltaTime * amount;
            }

            // The difference between our current speed and our new speed.
            float newSpeed = Mathf.Max(speed - drop, 0f);
            if (speed > 0f)
                newSpeed /= speed; // Divide out the current speed.

            // Incorporate our new speed.
            data.Velocity *= newSpeed;
        }

        void HandleJump( )
        {
            // Disable the fact that we want to jump next frame.
            // If we remove this you should be able to bunny hop like in source engine with sv_autobunnyhopping
            data.InJump = false;

            // Apply our jump power to our Y velocity.
            data.Velocity.y += JumpPower;
            IsJumping = !data.InJump;
        }

        // This is a rewritten version of Quake's air acceleration.
        void HandleAirMove()
        {
            // Get our forward and right vectors (again we only care about XZ movement)
            Vector3 forward = transform.forward, right = transform.right;
            right.y = forward.y = 0f;

            // Calcualte our wish velocity
            var wishVel = forward.normalized * data.ForwardMove + right.normalized * data.SideMove;
            wishVel.y = 0f;

            // Break down our velocity into speed and direction.
            var wishSpeed = wishVel.magnitude;
            var wishDir = wishVel.normalized;

            // Clamp our wish speed to our max possible speed.
            if (ClampAirSpeed && (wishSpeed != 0f && (wishSpeed > MaxSpeed)))
                wishSpeed = MaxSpeed;

            // Cap speed
            // wishSpeed = Mathf.Min(wishSpeed, AirCap);

            // Determine veer amount
            var curSpeed = Vector3.Dot(data.Velocity, wishDir);

            // See how much to add and return if we aren't adding anything.
            var newSpeed = wishSpeed - curSpeed;
            if (newSpeed <= 0)
                return;

            // Calculate our new acceleration speed.
            var accelSpeed = Mathf.Min(AirAcceleration * wishVel.magnitude * Time.deltaTime, newSpeed);

            data.Velocity += accelSpeed * wishDir;
        }

        void HandleCrouchMove()
        {

        }

        // Handling crouching (THIS STILL NEEDS TO BE REDONE)
        void CrouchPlayer()
        {
            // Simple "crouch" just sets the character controller height.
            Controller.height = Input.GetButton("Crouch") ? CrouchHeight : StandHeight;
        }
    }
}