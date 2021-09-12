using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterCollider : MonoBehaviour {
        /* Instances */
        LayerMask chunkMask;

        GameObject characterBodyObject;
        BoxCollider characterBoxCollider;

        /* Variables - For Collisions */
        private readonly float boxColliderHeight = Constants_str.boxColliderHeightForGroundCheck;
        private readonly Vector3 boxColliderSizeOffSet = Constants_str.boxColliderSizeOffSetForGroundCheck;

        private void Start() {
            // Setting up
            chunkMask = LayerMask.GetMask("Chunk");
            characterBodyObject = PlayerData.dictionaryPlayerObject["CharacterBody"];
            characterBoxCollider = characterBodyObject.GetComponent<BoxCollider>();
        }

        private void OnCollisionEnter(Collision collision) {
            RaycastHit @raycastHit;
            CharacterBehaviour.isGrounded = Physics.BoxCast(characterBoxCollider.bounds.center,
                                                            new Vector3(characterBoxCollider.bounds.size.x, boxColliderHeight, characterBoxCollider.bounds.size.z) + boxColliderSizeOffSet,
                                                            -transform.up,
                                                            out @raycastHit,
                                                            characterBoxCollider.transform.rotation,
                                                            CharacterBehaviour.characterRadius.y);
        }

        public void CheckIfGrounded() {
            RaycastHit @raycastHit;
            CharacterBehaviour.isGrounded = Physics.BoxCast(characterBoxCollider.bounds.center,
                                                            new Vector3(characterBoxCollider.bounds.size.x, boxColliderHeight, characterBoxCollider.bounds.size.z) + boxColliderSizeOffSet,
                                                            -transform.up,
                                                            out @raycastHit,
                                                            characterBoxCollider.transform.rotation,
                                                            CharacterBehaviour.characterRadius.y);
        }
    }
}