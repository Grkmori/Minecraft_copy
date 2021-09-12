using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Player.Character;

namespace BlueBird.World.Player {
    public sealed class PlayerManager : _0SingletonManager<PlayerManager> {
        /* Instances */
        GameObject playerObject;
        GameObject mainCameraObject;
        GameObject playerCanvasObject;
        GameObject playerToolbarObject;

        GameObject characterObject;
        GameObject characterBodyObject;
        GameObject characterCameraObject;
        GameObject characterCanvasObject;
        GameObject characterCrosshairObject;
        GameObject highlightVoxel;
        GameObject placeVoxel;

        GameObject debugScreen;

        PlayerGenerator _playerGenerator = new PlayerGenerator();
        CharacterGenerator _characterGenerator = new CharacterGenerator();

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>PlayerManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Generate Player Dictionaries
                _playerGenerator.CreatePlayerDictionaries();
                _playerGenerator.ClearPlayerDictionaries();

                CreatePlayerGameObjects();
                CreateCharacterGameObjects();

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>PlayerManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Awake'</i></color> PlayerManager</b>.");
            }
        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>PlayerManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>PlayerManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> PlayerManager</b>.");
            }
            WorldDirector.checkerPlayerStart = true;
        }

        private void CreatePlayerGameObjects() {
            // Creating Player GameObject
            playerObject = Instantiate(WorldData.dictionaryPlayerPrefabs["Player"], Vector3.zero, Quaternion.identity);
            _playerGenerator.SetupPlayerGameObject(playerObject);
            mainCameraObject = Instantiate(WorldData.dictionaryPlayerPrefabs["MainCameraFocus"], Constants_str.mainCameraFocusPosition, Quaternion.identity, playerObject.transform);
            _playerGenerator.CatchMainCameraGameObject(mainCameraObject, Constants_str.mainCameraFocusRotation, Constants_str.mainCameraPosition, Constants_str.mainCameraRotation, Constants_str.mainCameraScale, Constants_str.mainCameraFieldOfView);
            playerCanvasObject = Instantiate(WorldData.dictionaryPlayerUIPrefabs["PlayerCanvas"], Vector3.zero, Quaternion.identity, playerObject.transform);
            _playerGenerator.SetupPlayerCanvasGameObject(playerCanvasObject);
            playerToolbarObject = Instantiate(WorldData.dictionaryPlayerUIPrefabs["PlayerToolbar"], new Vector3(playerCanvasObject.transform.position.x, Constants_str.toolbarHeightInCanvas, playerCanvasObject.transform.position.z), Quaternion.identity, playerCanvasObject.transform);
            _playerGenerator.SetupPlayerToolbarGameObject(playerToolbarObject);
            debugScreen = Instantiate(WorldData.dictionaryPlayerUIPrefabs["DebugScreen"], Vector3.zero, Quaternion.identity, playerObject.transform);
            _playerGenerator.SetupDebugScreen(debugScreen);
        }

        private void CreateCharacterGameObjects() {
            // Creating Character GameObjects
            characterObject = Instantiate(WorldData.dictionaryPlayerPrefabs["Character"], Settings_str.playerSpawnPosition, Quaternion.identity, playerObject.transform);
            _characterGenerator.SetupCharacterGameObject(characterObject);
            characterBodyObject = Instantiate(WorldData.dictionaryPlayerPrefabs["CharacterBody"], characterObject.transform.position, Quaternion.identity, characterObject.transform);
            _characterGenerator.SetupCharacterBodyGameObject(characterBodyObject, Constants_str.characterBaseRadius, Constants_str.charcterColliderSize);
            characterCameraObject = Instantiate(WorldData.dictionaryPlayerPrefabs["CharacterCamera"], characterObject.transform.position + Constants_str.characterCameraPosition, Quaternion.identity, characterObject.transform);
            _characterGenerator.SetupCharacterCameraGameObject(characterCameraObject, Constants_str.characterCameraFieldOfView, Constants_str.characterCameraNearClipPlane);
            characterCanvasObject = Instantiate(WorldData.dictionaryPlayerUIPrefabs["CharacterCanvas"], Vector3.zero, Quaternion.identity, characterObject.transform);
            _characterGenerator.SetupCharacterCanvasGameObject(characterCanvasObject);
            characterCrosshairObject = Instantiate(WorldData.dictionaryPlayerUIPrefabs["CharacterCrosshair"], characterCanvasObject.transform.position, Quaternion.identity, characterCanvasObject.transform);
            _characterGenerator.SetupCharacterCrosshairGameObject(characterCrosshairObject);
            highlightVoxel = Instantiate(WorldData.dictionaryPlayerPrefabs["HighlightVoxel"], Vector3.zero, Quaternion.identity, playerObject.transform);
            _characterGenerator.SetupHighlightVoxelGameObject(highlightVoxel);
            placeVoxel = Instantiate(WorldData.dictionaryPlayerPrefabs["PlaceVoxel"], Vector3.zero, Quaternion.identity, playerObject.transform);
            _characterGenerator.SetupPlaceVoxelGameObject(placeVoxel);
        }
    }
}
