using UnityEngine;
using UnityEngine.UI;
using BlueBird.World.Parameters;
using BlueBird.World.Player.UI.Utilities;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterGenerator {
        /* Variables - For Toolbar */
        private int numberItemSlots = Constants_str.toolbarItemSlotsNumber;

        public void SetupCharacterGameObject(GameObject characterObject) {
            // Setup the GameObject for the Character
            characterObject.name = "Character";
            Rigidbody characterRigidBody = characterObject.GetComponent<Rigidbody>();
            characterRigidBody.useGravity = false;
            characterRigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            characterRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            bool tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(characterObject.name, characterObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>Character Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + characterObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupCharacterBodyGameObject(GameObject characterBodyObject, Vector3 characterBaseRadius, Vector3 characterColliderSize) {
            characterBodyObject.name = "CharacterBody";
            characterBodyObject.transform.localScale = new Vector3(characterBaseRadius.x * 2, characterBaseRadius.y * 2, characterBaseRadius.z * 2);
            BoxCollider characterCollider = characterBodyObject.GetComponent<BoxCollider>();
            characterCollider.size = characterColliderSize;
            bool tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(characterBodyObject.name, characterBodyObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>CharacterBody Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + characterBodyObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupCharacterCameraGameObject(GameObject characterCameraObject, float characterCameraFieldOfView, float characterCameraNearClipPlane) {
            characterCameraObject.name = "CharacterCamera";
            characterCameraObject.SetActive(true);
            Camera characterCamera = characterCameraObject.GetComponent<Camera>();
            characterCamera.orthographic = false;
            characterCamera.fieldOfView = characterCameraFieldOfView;
            characterCamera.nearClipPlane = characterCameraNearClipPlane;
            bool tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(characterCameraObject.name, characterCameraObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>CharacterCamera Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + characterCameraObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupCharacterCanvasGameObject(GameObject characterCanvasObject) {
            characterCanvasObject.name = "CharacterCanvas";
            bool tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(characterCanvasObject.name, characterCanvasObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>CharacterCanvas Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + characterCanvasObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }
        }

        public void SetupCharacterToolbarGameObject(GameObject characterToolbarObject) {
            characterToolbarObject.name = "CharacterToolbar";
            bool tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(characterToolbarObject.name, characterToolbarObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>CharacterToolbar Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + characterToolbarObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }

            // Populating dictionaryToolbarItemSlots
            int itemSlotIndex = 1;
            for(int i = 1; i <= numberItemSlots; i++) {
                GameObject itemSlot = GameObject.Find("ToolbarItemSlot_" + i);
                ItemSlot itemSlotData = itemSlot.GetComponent<ItemSlot>();
                itemSlotData.itemSlotPosition = new Vector2(1, itemSlotIndex);
                itemSlotData.slotItem = itemSlot.transform.Find("SlotIcon").GetComponent<Image>();
                itemSlotIndex++;

                bool tryAddFalseItemSlots = PlayerData.dictionaryToolbarItemSlots.TryAdd(itemSlotData.itemSlotPosition, itemSlotData);
                if(!tryAddFalseItemSlots) {
                    Debug.LogError("<b>ItemSlot " + itemSlotData.itemSlotPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryToolbarItemSlots</b>.");
                } else {
                    ///Debug.Log("<b>ItemSlot</b> for the <b>" + itemSlotData.itemSlotPosition + "</b> was <color=green><i>generated</i></color>, and <color=cyan><i>added</i></color> to <b>dictionaryToolbarItemSlots</b>.");
                }
            }
            if(PlayerData.dictionaryToolbarItemSlots.Count < numberItemSlots) {
                Debug.LogWarning("<b>dictionaryToolbarItemSlots</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>ToolbarItemSlots Objects</b> were <color=green><i>caught</i></color>. " +
                          "<b>dictionaryToolbarItemSlots</b> was <color=cyan><i>populated</i></color> with a total of <i>" + PlayerData.dictionaryToolbarItemSlots.Count + " ToolbarItemSlots</i>.");
            }
        }

        public void SetupCharacterCrosshairGameObject(GameObject characterCrosshairObject) {
            characterCrosshairObject.name = "CharacterCrosshair";
            bool tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(characterCrosshairObject.name, characterCrosshairObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>CharacterCrosshair Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + characterCrosshairObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }
        }

        public void SetupHighlightVoxelGameObject(GameObject highlightVoxelObject) {
            highlightVoxelObject.name = "HighlightVoxel";
            bool tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(highlightVoxelObject.name, highlightVoxelObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>HighlightVoxel Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + highlightVoxelObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void SetupPlaceVoxelGameObject(GameObject placeVoxelObject) {
            placeVoxelObject.name = "PlaceVoxel";
            bool tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(placeVoxelObject.name, placeVoxelObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>PlaceVoxel Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + placeVoxelObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }
    }
}