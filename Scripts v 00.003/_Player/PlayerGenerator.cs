using UnityEngine;
using UnityEngine.UI;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Player.UI.Utilities;

namespace BlueBird.World.Player {
    public sealed class PlayerGenerator {
        /* Variables - For Toolbar */
        private int numberItemSlots = Constants_str.toolbarItemSlotsNumber;

        #region Dictionaries
        public void CreatePlayerDictionaries() {
            // Create Player GameObjects Dictionary
            PlayerData.dictionaryPlayerObject = new ConcurrentDictionary<string, GameObject>();
            PlayerData.dictionaryPlayerUIObject = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>Player Dictionaries</b> were successfully <color=green><i>created</i></color>.");

            /* Storage - For Inventory */
            PlayerData.dictionaryToolbarItemSlots = new ConcurrentDictionary<Vector2, ItemSlot>();
            Debug.Log("<b>Player Inventory Dictionaries</b> were successfully <color=green><i>created</i></color>.");
        }

        public void ClearPlayerDictionaries() {
            // Clearing any Old Persistent from the PlayerData Dictionaries
            PlayerData.dictionaryPlayerObject.Clear();
            PlayerData.dictionaryPlayerUIObject.Clear();

            PlayerData.dictionaryToolbarItemSlots.Clear();
            Debug.Log("<b>Old Persistent Data</b> from <b>PlayerData Dictionaries</b> were successfully <color=cyan><i>cleared</i></color> if they had any.");
        }
        #endregion

        #region Player Generation
        public void SetupPlayerGameObject(GameObject @playerObject) {
            // Setup the GameObject for the Player
            @playerObject.name = "Player";
            bool @tryAddFalse = WorldData.dictionaryMainObject.TryAdd(@playerObject.name, @playerObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>Player Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryMainObject</b>.");
            } else {
                Debug.Log("<b>" + @playerObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryMainObject</i>.");
            }
        }

        public void CatchMainCameraGameObject(GameObject @mainCameraFocusObject, Vector3 @mainCameraFocusRotation, Vector3 @mainCameraPosition, Vector3 @mainCameraRotation, Vector3 @mainCameraScale, float @mainCameraFieldOfView) {
            // Create a Parent for the MainCamera
            @mainCameraFocusObject.name = "MainCameraFocus";
            @mainCameraFocusObject.transform.localEulerAngles = @mainCameraFocusRotation;
            @mainCameraFocusObject.SetActive(false);
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@mainCameraFocusObject.name, @mainCameraFocusObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>MainCamera Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @mainCameraFocusObject.name + " GameObject</b> has been <color=green><i>caught</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }

            // Catch the MainCamera'Camera' of the Scene
            GameObject @mainCameraObject = GameObject.FindWithTag("MainCamera");
            @mainCameraObject.transform.SetParent(@mainCameraFocusObject.transform, true);
            @mainCameraObject.name = "MainCamera";
            @mainCameraObject.tag = "MainCamera";
            @mainCameraObject.layer = 8;
            @mainCameraObject.transform.localPosition = @mainCameraPosition;
            @mainCameraObject.transform.localEulerAngles = @mainCameraRotation;
            @mainCameraObject.transform.localScale = @mainCameraScale;
            Camera @mainCamera = @mainCameraObject.GetComponent<Camera>();
            @mainCamera.orthographic = false;
            @mainCamera.fieldOfView = @mainCameraFieldOfView;
            ///@mainCamera.orthographicSize = 15.0f;
        }

        public void SetupPlayerCanvasGameObject(GameObject @playerCanvasObject) {
            @playerCanvasObject.name = "PlayerCanvas";
            bool @tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(@playerCanvasObject.name, @playerCanvasObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>PlayerCanvas Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + @playerCanvasObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }
        }

        public void SetupPlayerToolbarGameObject(GameObject @playerToolbarObject) {
            @playerToolbarObject.name = "PlayerToolbar";
            bool @tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(@playerToolbarObject.name, @playerToolbarObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>PlayerToolbar Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + @playerToolbarObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }

            // Populating dictionaryToolbarItemSlots
            int @itemSlotIndex = 1;
            for(int i = 1; i <= numberItemSlots; i++) {
                GameObject @itemSlot = GameObject.Find("ToolbarItemSlot_" + i);
                ItemSlot @itemSlotData = @itemSlot.GetComponent<ItemSlot>();
                @itemSlotData.itemSlotPosition = new Vector2(1, @itemSlotIndex);
                @itemSlotData.slotItem = @itemSlot.transform.Find("SlotIcon").GetComponent<Image>();
                @itemSlotIndex++;

                bool @tryAddFalseItemSlots = PlayerData.dictionaryToolbarItemSlots.TryAdd(@itemSlotData.itemSlotPosition, @itemSlotData);
                if(!@tryAddFalseItemSlots) {
                    Debug.LogError("<b>ItemSlot " + @itemSlotData.itemSlotPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryToolbarItemSlots</b>.");
                } else {
                    ///Debug.Log("<b>ItemSlot</b> for the <b>" + @itemSlotData.itemSlotPosition + "</b> was <color=green><i>generated</i></color>, and <color=cyan><i>added</i></color> to <b>dictionaryToolbarItemSlots</b>.");
                }
            }
            if(PlayerData.dictionaryToolbarItemSlots.Count < numberItemSlots) {
                Debug.LogWarning("<b>dictionaryToolbarItemSlots</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>ToolbarItemSlots Objects</b> were <color=green><i>caught</i></color>. " +
                          "<b>dictionaryToolbarItemSlots</b> was <color=cyan><i>populated</i></color> with a total of <i>" + PlayerData.dictionaryToolbarItemSlots.Count + " ToolbarItemSlots</i>.");
            }
        }

        public void SetupDebugScreen(GameObject @debugScreen) {
            // Setup the GameObject for the DebugScreen
            @debugScreen.name = "DebugScreen";
            bool @tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(@debugScreen.name, @debugScreen);
            if(!@tryAddFalse) {
                Debug.LogError("<b>DebugScreen</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + debugScreen.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }
        }
        #endregion
    }
}