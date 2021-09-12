using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterBehaviour : MonoBehaviour {
        /* Instances */
        Rigidbody characterRigidBody;

        CharacterCollider _characterCollider;

        /* Variables - For Inputs */
        private float inputXaxis;
        private float inputZaxis;

        /* Variables - For Character Movement */
        public static Vector3 characterRadius = Constants_str.characterBaseRadius;
        private readonly float walkSpeed = Constants_str.characterBaseWalkSpeed;
        private readonly float sprintSpeed = Constants_str.characterBaseSprintSpeed;
        private readonly float jumpForce = Constants_str.characterBaseJumpForce;
        private readonly float jumpDelay = Constants_str.characterJumpDelay;

        private float maxVelocityChangeForce = Constants_str.characterMaximumVelocityChangeForce;
        private float characterSpeed;
        private Vector3 targetVelocity;
        private Vector3 changeVelocity;
        private Vector3 velocity;

        private bool isSprinting = false;
        private bool jumpRequest = false;
        public static bool isGrounded = false;
        private float canJump;

        private void Start() {
            // Setting up
            characterRigidBody = gameObject.GetComponent<Rigidbody>();
            _characterCollider = transform.gameObject.GetComponent<CharacterCollider>();
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                if(jumpRequest) {
                    Jump();
                }

                UpdateVelocity();

                characterRigidBody.AddForce(changeVelocity, ForceMode.VelocityChange);
            }
        }

        public void InputAxisHorizontal() {
            inputXaxis = Input.GetAxis("Horizontal");
        }
        public void InputAxisVertical() {
            inputZaxis = Input.GetAxis("Vertical");
        }

        public void InputKeySprintDown() {
            isSprinting = true;
        }
        public void InputKeySprintUp() {
            isSprinting = false;
        }

        public void InputKeyJumpDown() {
            _characterCollider.CheckIfGrounded();
            if(isGrounded && Time.fixedTime > canJump) {
                jumpRequest = true;
            }
        }
        private void Jump() {
            characterRigidBody.AddForce(Vector3.up * jumpForce);
            jumpRequest = false;
            isGrounded = false;
            canJump = Time.fixedTime + jumpDelay;
        }

        private void UpdateVelocity() {
            // Calculate how fast we should be Moving
            if(!isSprinting) {
                characterSpeed = walkSpeed;
            } else {
                characterSpeed = sprintSpeed;
            }
            targetVelocity = new Vector3(inputXaxis, 0, inputZaxis);
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= characterSpeed;
            inputXaxis = 0;
            inputZaxis = 0;

            // Apply a Force that attempts to reach our Target Velocity
            velocity = characterRigidBody.velocity;
            changeVelocity = (targetVelocity - velocity);
            changeVelocity.x = Mathf.Clamp(changeVelocity.x, -maxVelocityChangeForce, maxVelocityChangeForce);
            changeVelocity.z = Mathf.Clamp(changeVelocity.z, -maxVelocityChangeForce, maxVelocityChangeForce);
            changeVelocity.y = 0;
        }
    }
}