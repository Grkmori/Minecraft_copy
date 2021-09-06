using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World;
using BlueBird.World.Parameters;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;
using BlueBird.World.Player;
using BlueBird.World.Player.Character.Collisions;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterBehaviour : MonoBehaviour {
        /* Instances - For Character Controller */
        Chunk _currentChunk;

        CharacterCollisions _characterCollisions = new CharacterCollisions();

        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Variables - For Inputs */
        private float inputX;
        private float inputY;

        /* Variables - For Character Controller */
        private readonly float gravity = Constants_str.gravity;

        public static Vector2 characterRadius = Constants_str.characterBaseRadius;
        public static float characterHeight = Constants_str.characterBaseHeight;

        /* Variables - For Character Movement */
        private float walkSpeed = Constants_str.characterBaseWalkSpeed;
        private float sprintSpeed = Constants_str.characterBaseSprintSpeed;
        private float jumpForce = Constants_str.characterBaseJumpForce;

        private static Vector3 velocity;
        private static float verticalMomentum = 0;

        public static bool isGrounded = false;
        private bool isSprinting = false;
        private bool jumpRequest = false;
        private bool characterOutOfWorldBounds = false;

        private void Start() {
            // Setting up
            Vector3 @characterChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(transform.position);
            _currentChunk = WorldData.dictionaryChunkData[@characterChunkPosition];
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                if(jumpRequest) {
                    Jump();
                }
            }
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                GetPlayerMovementInputs();

                velocity = (transform.forward * inputY) + (transform.right * inputX);
                if(verticalMomentum > gravity) {
                    verticalMomentum += Time.deltaTime * gravity;
                }
                velocity += Vector3.up * verticalMomentum * Time.deltaTime;

                CheckForCollisions();

                GetOtherPlayerInputs();

                transform.Translate(velocity, Space.World);
            }
        }

        private void GetPlayerMovementInputs() {
            if(!isSprinting) {
                inputX = Input.GetAxis("Horizontal") * Time.deltaTime * walkSpeed;
                inputY = Input.GetAxis("Vertical") * Time.deltaTime * walkSpeed;
            } else {
                inputX = Input.GetAxis("Horizontal") * Time.deltaTime * sprintSpeed;
                inputY = Input.GetAxis("Vertical") * Time.deltaTime * sprintSpeed;
            }
        }

        private void GetOtherPlayerInputs() {
            if(Input.GetButtonDown("Sprint")) {
                isSprinting = true;
            }
            if(Input.GetButtonUp("Sprint")) {
                isSprinting = false;
            }

            if(isGrounded && Input.GetButtonDown("Jump")) {
                jumpRequest = true;
            }

            if(Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }

        private void Jump() {
            verticalMomentum = jumpForce;
            isGrounded = false;
            jumpRequest = false;
        }

        private void CheckForCollisions() {
            // Setting up
            CheckForCurrentChunk();

            // Checking for Collision
            if(!characterOutOfWorldBounds) {
                Vector3 @characterVoxelPosition = _voxelUtilities.GetVoxelPositionFromPosition(transform.position);
                Vector3 @offSetFromVoxelCenter = transform.position - @characterVoxelPosition;
                velocity.y = _characterCollisions.CheckForCollisionTB(velocity, transform.position, _currentChunk, @characterVoxelPosition, @offSetFromVoxelCenter);
                velocity.x = _characterCollisions.CheckForCollisionEW(velocity, transform.position, _currentChunk, @characterVoxelPosition, @offSetFromVoxelCenter);
                velocity.z = _characterCollisions.CheckForCollisionNS(velocity, transform.position, _currentChunk, @characterVoxelPosition, @offSetFromVoxelCenter);
            }
        }

        private void CheckForCurrentChunk() {
            Vector3 @characterChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(transform.position);
            if(_currentChunk.chunkPosition != @characterChunkPosition) {
                if(WorldData.dictionaryChunkData.ContainsKey(@characterChunkPosition)) {
                    _currentChunk = WorldData.dictionaryChunkData[@characterChunkPosition];
                } else {
                    characterOutOfWorldBounds = true;
                    Debug.LogWarning("<b>Character</b> is <color=yellow><i>out of World Bounds</i></color>.");
                }
            }
        }
    }
}