using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap;
using BlueBird.World.Player.Character;
using BlueBird.World.Player.UI.Utilities;

namespace BlueBird.World.Player {
    public sealed class PlayerGenerator {
        /* Instances */
        PlayerGeneratorAssistant _playerGeneratorAssistant;
        CharacterGenerator _characterGenerator = new CharacterGenerator();

        /* Instances - For Player Generation */
        GameObject playerObject;
        GameObject mainCameraObject;
        GameObject debugScreen;

        GameObject characterObject;
        GameObject characterBodyObject;
        GameObject characterCameraObject;
        GameObject characterCanvasObject;
        GameObject characterToolbarObject;
        GameObject characterCrosshairObject;
        GameObject highlightVoxel;
        GameObject placeVoxel;

        #region Dictionaries
        public void CreatePlayerDictionaries() {
            // Create Player GameObjects Dictionary
            PlayerData.dictionaryPlayerObject = new ConcurrentDictionary<string, GameObject>();
            PlayerData.dictionaryPlayerUIObject = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>Player Dictionaries</b> were successfully <color=green><i>created</i></color>.");

            // Create Prefabs Dictionaries
            PlayerData.dictionaryPlayerPrefabs = new ConcurrentDictionary<string, GameObject>();
            PlayerData.dictionaryPlayerUIPrefabs = new ConcurrentDictionary<string, GameObject>();

            /* Storage - For Inventory */
            PlayerData.dictionaryToolbarItemSlots = new ConcurrentDictionary<Vector2, ItemSlot>();
            Debug.Log("<b>Player Inventory Dictionaries</b> were successfully <color=green><i>created</i></color>.");
        }

        public void ClearPlayerDictionaries() {
            // Clearing any Old Persistent from the PlayerData Dictionaries
            PlayerData.dictionaryPlayerObject.Clear();
            PlayerData.dictionaryPlayerUIObject.Clear();

            PlayerData.dictionaryPlayerPrefabs.Clear();
            PlayerData.dictionaryPlayerUIPrefabs.Clear();

            PlayerData.dictionaryToolbarItemSlots.Clear();
            Debug.Log("<b>Old Persistent Data</b> from <b>PlayerData Dictionaries</b> were successfully <color=cyan><i>cleared</i></color> if they had any.");
        }
        #endregion

        #region Player Generation
        public void CreatePlayerGameObjects() {
            // Setting up
            GameObject playerManagerAssistant = new GameObject("PlayerManagerAssistant", typeof(PlayerGeneratorAssistant)) { // Create a MonoBehaviour to assist on Creating/Instantiating GameObjects
                tag = "Assistant",
                layer = 7
            };
            _playerGeneratorAssistant = playerManagerAssistant.GetComponent<PlayerGeneratorAssistant>();

            // Creating Player GameObject
            playerObject = _playerGeneratorAssistant.InstantiatePlayerObject();
            SetupPlayerGameObject(playerObject);
            mainCameraObject = _playerGeneratorAssistant.InstantiateMainCameraObject(playerObject);
            CatchMainCameraGameObject(mainCameraObject, Constants_str.mainCameraFocusRotation, Constants_str.mainCameraPosition, Constants_str.mainCameraRotation, Constants_str.mainCameraScale, Constants_str.mainCameraFieldOfView);
            debugScreen = _playerGeneratorAssistant.InstantiateDebugScreenObject(playerObject);
            SetupDebugScreen(debugScreen);

            // Creating Character GameObjects
            characterObject = _playerGeneratorAssistant.InstantiateCharacterObject(playerObject);
            _characterGenerator.SetupCharacterGameObject(characterObject);
            characterBodyObject = _playerGeneratorAssistant.InstantiateCharacterBodyObject(characterObject);
            _characterGenerator.SetupCharacterBodyGameObject(characterBodyObject, Constants_str.characterBaseRadius, Constants_str.charcterColliderSize);
            characterCameraObject = _playerGeneratorAssistant.InstantiateCharacterCameraObject(characterObject);
            _characterGenerator.SetupCharacterCameraGameObject(characterCameraObject, Constants_str.characterCameraFieldOfView, Constants_str.characterCameraNearClipPlane);
            characterCanvasObject = _playerGeneratorAssistant.InstantiateCharacterCanvasObject(characterObject);
            _characterGenerator.SetupCharacterCanvasGameObject(characterCanvasObject);
            characterToolbarObject = _playerGeneratorAssistant.InstantiateCharacterToolbarObject(characterCanvasObject);
            _characterGenerator.SetupCharacterToolbarGameObject(characterToolbarObject);
            characterCrosshairObject = _playerGeneratorAssistant.InstantiateCharacterCrosshairObject(characterCanvasObject);
            _characterGenerator.SetupCharacterCrosshairGameObject(characterCrosshairObject);
            highlightVoxel = _playerGeneratorAssistant.InstantiateHighlightVoxelObject(playerObject);
            _characterGenerator.SetupHighlightVoxelGameObject(highlightVoxel);
            placeVoxel = _playerGeneratorAssistant.InstantiatePlaceVoxelObject(playerObject);
            _characterGenerator.SetupPlaceVoxelGameObject(placeVoxel);

            playerManagerAssistant.SetActive(false);  // Disable/Destroy Assistant GameObject that is no longer useful
        }

        private void SetupPlayerGameObject(GameObject playerObject) {
            // Setup the GameObject for the Player
            playerObject.name = "Player";
            bool tryAddFalse = WorldData.dictionaryMainObject.TryAdd(playerObject.name, playerObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>Player Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryMainObject</b>.");
            } else {
                Debug.Log("<b>" + playerObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryMainObject</i>.");
            }
        }

        private void CatchMainCameraGameObject(GameObject mainCameraFocusObject, Vector3 mainCameraFocusRotation, Vector3 mainCameraPosition, Vector3 mainCameraRotation, Vector3 mainCameraScale, float mainCameraFieldOfView) {
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

        private void SetupDebugScreen(GameObject debugScreen) {
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