using UnityEngine;
using TMPro;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Player.Character;

namespace BlueBird.World.Player.UI {
    public sealed class PlayerToolbar : MonoBehaviour {
        /* Instances */
        Transform playerCanvasTransform;
        TMP_Text selectedVoxelText;
        Transform highlightToolbarItemSlot;

        /* Variables - For Inputs */
        private int numberItemSlots = Constants_str.toolbarItemSlotsNumber;

        private float scrollWheel;
        private int slotIndex = 1;
        private int lastSlotIndex = 1;

        private void Start() {
            // Setting up
            playerCanvasTransform = PlayerData.dictionaryPlayerUIObject["PlayerCanvas"].transform;
            selectedVoxelText = playerCanvasTransform.Find("SelectedVoxel").GetComponent<TextMeshProUGUI>();
            highlightToolbarItemSlot = GameObject.Find("HighlightToolbarItemSlot").transform;

            // Start Setup (run once for starting information)
            Vector2 @itemSlotStartPosition = new Vector2(1, slotIndex);
            highlightToolbarItemSlot.position = PlayerData.dictionaryToolbarItemSlots[@itemSlotStartPosition].transform.position;
            selectedVoxelText.text = PlayerData.dictionaryToolbarItemSlots[@itemSlotStartPosition].slotItem.sprite.name;
            CharacterInteraction.selectedVoxelTypeName = PlayerData.dictionaryToolbarItemSlots[@itemSlotStartPosition].slotItem.sprite.name;
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                ToolbarSelection();
            }
        }

        public void InputMouseScrollWheel() {
            scrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if(scrollWheel != 0) {
                if(scrollWheel > 0) {
                    slotIndex--;
                } else {
                    slotIndex++;
                }

                if(slotIndex > numberItemSlots) {
                    slotIndex = 1;
                }
                if(slotIndex < 1) {
                    slotIndex = numberItemSlots;
                }
            }

            scrollWheel = 0;
        }

        private void ToolbarSelection() {
            if(slotIndex != lastSlotIndex) {
                Vector2 @itemSlotPosition = new Vector2(1, slotIndex);
                highlightToolbarItemSlot.position = PlayerData.dictionaryToolbarItemSlots[@itemSlotPosition].transform.position;
                if(PlayerData.dictionaryToolbarItemSlots[@itemSlotPosition].slotItem.sprite != null) {
                    selectedVoxelText.text = PlayerData.dictionaryToolbarItemSlots[@itemSlotPosition].slotItem.sprite.name;
                    CharacterInteraction.selectedVoxelTypeName = PlayerData.dictionaryToolbarItemSlots[@itemSlotPosition].slotItem.sprite.name;
                } else {
                    selectedVoxelText.text = "";
                    CharacterInteraction.selectedVoxelTypeName = "Empty";
                }

                lastSlotIndex = slotIndex;
            }
        }
    }
}