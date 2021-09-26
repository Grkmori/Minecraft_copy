using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player {
    public sealed class PlayerGeneratorAssistant : MonoBehaviour {
        private void OnDisable() {
            Destroy(gameObject);
        }

        public GameObject InstantiatePlayerObject() {
            return Instantiate(PlayerData.dictionaryPlayerPrefabs["Player"], Vector3.zero, Quaternion.identity);
        }

        public GameObject InstantiateMainCameraObject(GameObject playerObject) {
            return Instantiate(PlayerData.dictionaryPlayerPrefabs["MainCameraFocus"], Constants_str.mainCameraFocusPosition, Quaternion.identity, playerObject.transform);
        }

        public GameObject InstantiateDebugScreenObject(GameObject playerObject) {
            return Instantiate(PlayerData.dictionaryPlayerUIPrefabs["DebugScreen"], Vector3.zero, Quaternion.identity, playerObject.transform);
        }

        public GameObject InstantiateCharacterObject(GameObject playerObject) {
            return Instantiate(PlayerData.dictionaryPlayerPrefabs["Character"], Settings_str.playerSpawnPosition, Quaternion.identity, playerObject.transform);
        }

        public GameObject InstantiateCharacterBodyObject(GameObject characterObject) {
            return Instantiate(PlayerData.dictionaryPlayerPrefabs["CharacterBody"], characterObject.transform.position, Quaternion.identity, characterObject.transform);
        }

        public GameObject InstantiateCharacterCameraObject(GameObject characterObject) {
            return Instantiate(PlayerData.dictionaryPlayerPrefabs["CharacterCamera"], characterObject.transform.position + Constants_str.characterCameraPosition, Quaternion.identity, characterObject.transform);
        }

        public GameObject InstantiateCharacterCanvasObject(GameObject characterObject) {
            return Instantiate(PlayerData.dictionaryPlayerUIPrefabs["CharacterCanvas"], Vector3.zero, Quaternion.identity, characterObject.transform);
        }

        public GameObject InstantiateCharacterToolbarObject(GameObject characterCanvasObject) {
            return Instantiate(PlayerData.dictionaryPlayerUIPrefabs["CharacterToolbar"], new Vector3(characterCanvasObject.transform.position.x, Constants_str.toolbarHeightInCanvas, characterCanvasObject.transform.position.z), Quaternion.identity, characterCanvasObject.transform);
        }

        public GameObject InstantiateCharacterCrosshairObject(GameObject characterCanvasObject) {
            return Instantiate(PlayerData.dictionaryPlayerUIPrefabs["CharacterCrosshair"], characterCanvasObject.transform.position, Quaternion.identity, characterCanvasObject.transform);
        }

        public GameObject InstantiateHighlightVoxelObject(GameObject playerObject) {
            return Instantiate(PlayerData.dictionaryPlayerPrefabs["HighlightVoxel"], Vector3.zero, Quaternion.identity, playerObject.transform);
        }

        public GameObject InstantiatePlaceVoxelObject(GameObject playerObject) {
            return Instantiate(PlayerData.dictionaryPlayerPrefabs["PlaceVoxel"], Vector3.zero, Quaternion.identity, playerObject.transform);
        }
    }
}