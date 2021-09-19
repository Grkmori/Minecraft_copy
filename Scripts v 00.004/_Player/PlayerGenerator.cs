using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Player.UI.Utilities;

namespace BlueBird.World.Player {
    public sealed class PlayerGenerator {
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
        public void SetupPlayerGameObject(GameObject playerObject) {
            // Setup the GameObject for the Player
            playerObject.name = "Player";
            bool tryAddFalse = WorldData.dictionaryMainObject.TryAdd(playerObject.name, playerObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>Player Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryMainObject</b>.");
            } else {
                Debug.Log("<b>" + playerObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryMainObject</i>.");
            }
        }

        public void CatchMainCameraGameObject(GameObject mainCameraFocusObject, Vector3 mainCameraFocusRotation, Vector3 mainCameraPosition, Vector3 mainCameraRotation, Vector3 mainCameraScale, float mainCameraFieldOfView) {
            // Create a Parent for the MainCamera
            mainCameraFocusObject.name = "MainCameraFocus";
            mainCameraFocusObject.transform.localEulerAngles = mainCameraFocusRotation;
            mainCameraFocusObject.SetActive(false);
            bool tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(mainCameraFocusObject.name, mainCameraFocusObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>MainCamera Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + mainCameraFocusObject.name + " GameObject</b> has been <color=green><i>caught</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }

            // Catch the MainCamera'Camera' of the Scene
            GameObject mainCameraObject = GameObject.FindWithTag("MainCamera");
            mainCameraObject.transform.SetParent(mainCameraFocusObject.transform, true);
            mainCameraObject.name = "MainCamera";
            mainCameraObject.tag = "MainCamera";
            mainCameraObject.layer = 8;
            mainCameraObject.transform.localPosition = mainCameraPosition;
            mainCameraObject.transform.localEulerAngles = mainCameraRotation;
            mainCameraObject.transform.localScale = mainCameraScale;
            Camera mainCamera = mainCameraObject.GetComponent<Camera>();
            mainCamera.orthographic = false;
            mainCamera.fieldOfView = mainCameraFieldOfView;
            ///mainCamera.orthographicSize = 15.0f;
        }

        public void SetupDebugScreen(GameObject debugScreen) {
            // Setup the GameObject for the DebugScreen
            debugScreen.name = "DebugScreen";
            bool tryAddFalse = PlayerData.dictionaryPlayerUIObject.TryAdd(debugScreen.name, debugScreen);
            if(!tryAddFalse) {
                Debug.LogError("<b>DebugScreen</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIObject</b>.");
            } else {
                Debug.Log("<b>" + debugScreen.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerUIObject</i>.");
            }
        }
        #endregion
    }
}