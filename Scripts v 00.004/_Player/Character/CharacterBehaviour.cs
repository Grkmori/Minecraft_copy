using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterBehaviour : MonoBehaviour {
        /* Instances */
        Rigidbody characterRigidBody;

        CharacterCollider _characterCollider;

        /* Variables - For Character Movement */
        private readonly float sprintMultiplier = Constants_str.characterSprintSpeedMultiplier;
        private readonly float jumpDelay = Constants_str.characterJumpDelay;

        private float maxVelocityChangeForce = Constants_str.characterMaximumVelocityChangeForce;
        private float characterSpeed;
        private Vector3 targetVelocity;
        private Vector3 changeVelocity;
        private Vector3 velocity;

        /* Variables - For Inputs */
        private float inputXaxis;
        private float inputZaxis;

        private bool isSprinting = false;
        private bool jumpRequest = false;
        public static bool isGrounded = false;
        private float canJump;

        private void OnEnable() {
            PlayerInputHandler.eventOnInputAxisHorizontal1st += InputCharacterMovement;
            PlayerInputHandler.eventOnInputAxisVertical1st += InputCharacterMovement;
            PlayerInputHandler.eventOnInputKeySprintDown += InputKeySprintDown;
            PlayerInputHandler.eventOnInputKeySprintUp += InputKeySprintUp;
            PlayerInputHandler.eventOnInputKeyJumpDown += InputKeyJumpDown;
        }

        private void Start() {
            // Setting up
            characterRigidBody = PlayerData.dictionaryPlayerObject["Character"].GetComponent<Rigidbody>();
            _characterCollider = transform.gameObject.GetComponent<CharacterCollider>();
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                if(jumpRequest) {
                    Jump();
                }
                UpdateVelocity();
            }
        }

        private void OnDisable() {
            PlayerInputHandler.eventOnInputAxisHorizontal1st -= InputCharacterMovement;
            PlayerInputHandler.eventOnInputAxisVertical1st -= InputCharacterMovement;
            PlayerInputHandler.eventOnInputKeySprintDown -= InputKeySprintDown;
            PlayerInputHandler.eventOnInputKeySprintUp -= InputKeySprintUp;
            PlayerInputHandler.eventOnInputKeyJumpDown -= InputKeyJumpDown;
        }

        private void InputCharacterMovement() {
            inputXaxis = Input.GetAxis("Horizontal");
            inputZaxis = Input.GetAxis("Vertical");
            CalculateCharacterMovement();
        }

        private void InputKeySprintDown() {
            isSprinting = true;
        }
        private void InputKeySprintUp() {
            isSprinting = false;
        }

        private void InputKeyJumpDown() {
            _characterCollider.CheckIfGrounded();
            if(isGrounded && Time.fixedTime > canJump) {
                jumpRequest = true;
            }
        }
        private void Jump() {
            characterRigidBody.AddForce(Vector3.up * Character.jumpForce);
            jumpRequest = false;
            isGrounded = false;
            canJump = Time.fixedTime + jumpDelay;
        }

        private void CalculateCharacterMovement() {
            // Calculate how fast we should be Moving
            if(!isSprinting) {
                characterSpeed = Character.walkSpeed;
            } else {
                characterSpeed = Character.walkSpeed * sprintMultiplier;
            }
            targetVelocity = new Vector3(inputXaxis, 0, inputZaxis);
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= characterSpeed;
        }

        private void UpdateVelocity() {
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
                // Apply a Force that attempts to Reach our Target Velocity
                velocity = characterRigidBody.velocity;
                changeVelocity = (targetVelocity - velocity);
                changeVelocity.x = Mathf.Clamp(changeVelocity.x, -maxVelocityChangeForce, maxVelocityChangeForce);
                changeVelocity.z = Mathf.Clamp(changeVelocity.z, -maxVelocityChangeForce, maxVelocityChangeForce);
                changeVelocity.y = 0;
                characterRigidBody.AddForce(changeVelocity, ForceMode.VelocityChange);
            } else if(characterRigidBody.velocity == Vector3.zero) {
                changeVelocity = Vector3.zero;
            } else {
                // Apply a Force that attempts to Stop our Target Velocity
                changeVelocity = -characterRigidBody.velocity;
                changeVelocity.y = 0;
                characterRigidBody.AddForce(changeVelocity, ForceMode.VelocityChange);
            }
        }
    }
}