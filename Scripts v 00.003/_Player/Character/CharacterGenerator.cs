using UnityEngine;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterGenerator {
        public void SetupCharacterGameObject(GameObject @characterObject) {
            // Setup the GameObject for the Character
            @characterObject.name = "Character";
            Rigidbody @characterRigidBody = @characterObject.GetComponent<Rigidbody>();
            @characterRigidBody.useGravity = false;
            @characterRigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            @characterRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@characterObject.name, @characterObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>Character Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @characterObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupCharacterBodyGameObject(GameObject @characterBodyObject, Vector3 @characterBaseRadius, Vector3 @characterColliderSize) {
            @characterBodyObject.name = "CharacterBody";
            @characterBodyObject.transform.localScale = new Vector3(@characterBaseRadius.x * 2, @characterBaseRadius.y * 2, @characterBaseRadius.z * 2);
            BoxCollider @characterCollider = @characterBodyObject.GetComponent<BoxCollider>();
            @characterCollider.size = @characterColliderSize;
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@characterBodyObject.name, @characterBodyObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>CharacterBody Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @characterBodyObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupCharacterCameraGameObject(GameObject @characterCameraObject, float @characterCameraFieldOfView, float @characterCameraNearClipPlane) {
            @characterCameraObject.name = "CharacterCamera";
            @characterCameraObject.SetActive(true);
            Camera @characterCamera = @characterCameraObject.GetComponent<Camera>();
            @characterCamera.orthographic = false;
            @characterCamera.fieldOfView = @characterCameraFieldOfView;
            @characterCamera.nearClipPlane = @characterCameraNearClipPlane;
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@characterCameraObject.name, @characterCameraObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>CharacterCamera Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @characterCameraObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupCharacterCanvasGameObject(GameObject @characterCanvasObject) {
            @characterCanvasObject.name = "CharacterCanvas";
            bool @tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(@characterCanvasObject.name, @characterCanvasObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>CharacterCanvas Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + @characterCanvasObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }
        }

        public void SetupCharacterCrosshairGameObject(GameObject @characterCrosshairObject) {
            @characterCrosshairObject.name = "CharacterCrosshair";
            bool @tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(@characterCrosshairObject.name, @characterCrosshairObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>CharacterCrosshair Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + @characterCrosshairObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }
        }

        public void SetupHighlightVoxelGameObject(GameObject @highlightVoxelObject) {
            @highlightVoxelObject.name = "HighlightVoxel";
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@highlightVoxelObject.name, @highlightVoxelObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>HighlightVoxel Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @highlightVoxelObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupPlaceVoxelGameObject(GameObject @placeVoxelObject) {
            @placeVoxelObject.name = "PlaceVoxel";
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@placeVoxelObject.name, @placeVoxelObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>PlaceVoxel Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @placeVoxelObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }
    }
}