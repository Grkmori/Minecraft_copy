using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Player.Character;

namespace BlueBird.World.Player {
    public sealed class PlayerManagerAssistant : MonoBehaviour {
        /* Instances */
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

        PlayerGenerator _playerGenerator = new PlayerGenerator();
        CharacterGenerator _characterGenerator = new CharacterGenerator();

        private void OnDisable() {
            Destroy(gameObject);
        }

        public void CreatePlayerGameObjects() {
            // Creating Player GameObject
            playerObject = Instantiate(WorldData.dictionaryPlayerPrefabs["Player"], Vector3.zero, Quaternion.identity);
            _playerGenerator.SetupPlayerGameObject(playerObject);
            mainCameraObject = Instantiate(WorldData.dictionaryPlayerPrefabs["MainCameraFocus"], Constants_str.mainCameraFocusPosition, Quaternion.identity, playerObject.transform);
            _playerGenerator.CatchMainCameraGameObject(mainCameraObject, Constants_str.mainCameraFocusRotation, Constants_str.mainCameraPosition, Constants_str.mainCameraRotation, Constants_str.mainCameraScale, Constants_str.mainCameraFieldOfView);
            debugScreen = Instantiate(WorldData.dictionaryPlayerUIPrefabs["DebugScreen"], Vector3.zero, Quaternion.identity, playerObject.transform);
            _playerGenerator.SetupDebugScreen(debugScreen);
        }

        public void CreateCharacterGameObjects() {
            // Creating Character GameObjects
            characterObject = Instantiate(WorldData.dictionaryPlayerPrefabs["Character"], Settings_str.playerSpawnPosition, Quaternion.identity, playerObject.transform);
            _characterGenerator.SetupCharacterGameObject(characterObject);
            characterBodyObject = Instantiate(WorldData.dictionaryPlayerPrefabs["CharacterBody"], characterObject.transform.position, Quaternion.identity, characterObject.transform);
            _characterGenerator.SetupCharacterBodyGameObject(characterBodyObject, Constants_str.characterBaseRadius, Constants_str.charcterColliderSize);
            characterCameraObject = Instantiate(WorldData.dictionaryPlayerPrefabs["CharacterCamera"], characterObject.transform.position + Constants_str.characterCameraPosition, Quaternion.identity, characterObject.transform);
            _characterGenerator.SetupCharacterCameraGameObject(characterCameraObject, Constants_str.characterCameraFieldOfView, Constants_str.characterCameraNearClipPlane);
            characterCanvasObject = Instantiate(WorldData.dictionaryPlayerUIPrefabs["CharacterCanvas"], Vector3.zero, Quaternion.identity, characterObject.transform);
            _characterGenerator.SetupCharacterCanvasGameObject(characterCanvasObject);
            characterToolbarObject = Instantiate(WorldData.dictionaryPlayerUIPrefabs["CharacterToolbar"], new Vector3(characterCanvasObject.transform.position.x, Constants_str.toolbarHeightInCanvas, characterCanvasObject.transform.position.z), Quaternion.identity, characterCanvasObject.transform);
            _characterGenerator.SetupCharacterToolbarGameObject(characterToolbarObject);
            characterCrosshairObject = Instantiate(WorldData.dictionaryPlayerUIPrefabs["CharacterCrosshair"], characterCanvasObject.transform.position, Quaternion.identity, characterCanvasObject.transform);
            _characterGenerator.SetupCharacterCrosshairGameObject(characterCrosshairObject);
            highlightVoxel = Instantiate(WorldData.dictionaryPlayerPrefabs["HighlightVoxel"], Vector3.zero, Quaternion.identity, playerObject.transform);
            _characterGenerator.SetupHighlightVoxelGameObject(highlightVoxel);
            placeVoxel = Instantiate(WorldData.dictionaryPlayerPrefabs["PlaceVoxel"], Vector3.zero, Quaternion.identity, playerObject.transform);
            _characterGenerator.SetupPlaceVoxelGameObject(placeVoxel);
        }
    }
}