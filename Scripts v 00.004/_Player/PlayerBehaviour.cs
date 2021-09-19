using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.GameTime;

namespace BlueBird.World.Player {
    public sealed class PlayerBehaviour : MonoBehaviour {
        /* Instances */
        GameObject mainCameraFocusObject;
        GameObject debugScreenObject;

        GameObject characterCameraObject;
        GameObject characterCanvasObject;
        GameObject highlightVoxelObject;
        GameObject placeVoxelObject;

        /* Variables - For Player Inputs */
        private readonly float normalSpeed = Constants_str.gameNormalSpeed;
        private readonly float fastSpeed = Constants_str.gameFastSpeed;
        private readonly float superFastSpeed = Constants_str.gameSuperFastSpeed;

        private void OnEnable() {
            PlayerInputHandler.eventOnInputKeyEscapeDown += InputKeyEscape;
            PlayerInputHandler.eventOnInputKeyF3Down += InputKeyF3;
            PlayerInputHandler.eventOnInputKeyF4Down += InputKeyF4;
            PlayerInputHandler.eventOnInputKeyF5Down += InputKeyF5;
        }

        private void Start() {
            // Setting up
            mainCameraFocusObject = PlayerData.dictionaryPlayerObject["MainCameraFocus"];
            debugScreenObject = PlayerData.dictionaryPlayerUIObject["DebugScreen"];

            characterCameraObject = PlayerData.dictionaryPlayerObject["CharacterCamera"];
            characterCanvasObject = PlayerData.dictionaryPlayerUIObject["CharacterCanvas"];
            highlightVoxelObject = PlayerData.dictionaryPlayerObject["HighlightVoxel"];
            placeVoxelObject = PlayerData.dictionaryPlayerObject["PlaceVoxel"];

            Cursor.lockState = CursorLockMode.Locked; // Lock cursor to Game Screen
        }

        private void OnDisable() {
            PlayerInputHandler.eventOnInputKeyEscapeDown -= InputKeyEscape;
            PlayerInputHandler.eventOnInputKeyF3Down -= InputKeyF3;
            PlayerInputHandler.eventOnInputKeyF4Down -= InputKeyF4;
            PlayerInputHandler.eventOnInputKeyF5Down -= InputKeyF5;
        }

        private void InputKeyEscape() {
            WorldDirector.isPaused = !WorldDirector.isPaused;
            ///Application.Quit();
        }

        private void InputKeyF3() {
            debugScreenObject.SetActive(!debugScreenObject.activeSelf);
        }

        private void InputKeyF4() {
            if(GameTimeManager.gameSpeed == normalSpeed) {
                GameTimeManager.gameSpeed = fastSpeed;
            } else if(GameTimeManager.gameSpeed == fastSpeed) {
                GameTimeManager.gameSpeed = superFastSpeed;
            } else if(GameTimeManager.gameSpeed == superFastSpeed) {
                GameTimeManager.gameSpeed = normalSpeed;
            } else {
                Debug.LogWarning("<b>gameSpeed</b> is not one of the <color=yellow><i>'default' values</i></color>.");
            }
        }

        private void InputKeyF5() {
            if(Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            PlayerInputHandler.view3rdPerson = !PlayerInputHandler.view3rdPerson;

            mainCameraFocusObject.SetActive(!mainCameraFocusObject.activeSelf);

            characterCameraObject.SetActive(!characterCameraObject.activeSelf);
            characterCanvasObject.SetActive(!characterCanvasObject.activeSelf);
            highlightVoxelObject.SetActive(!highlightVoxelObject.activeSelf);
            placeVoxelObject.SetActive(!placeVoxelObject.activeSelf);

            Debug.Log("<b>Player Camera</b> switched, and <b>3rd Person View</b> is now <color=blue><i>" + PlayerInputHandler.view3rdPerson + "</i></color>.");
        }
    }
}