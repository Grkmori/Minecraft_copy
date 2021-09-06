using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player.Character {
    public class CharacterCameraBehaviour : MonoBehaviour {
        /* Instances - For Character Controller */
        Transform character;

        /* Variables - For Inputs */
        private float mouseInputX;
        private float mouseInputY;

        /* Variables - For Camera Movement */
        private float sensitivity = Settings_str.mouseSensitivity;
        private float mouseXRotation = 0f;

        private void Start() {
            // Setting up
            character = PlayerData.dictionaryPlayerObject["Character"].transform;

            //Cursor.lockState = CursorLockMode.Locked; // Lock cursor to Game Screen
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                GetPlayerMouseInputs();

                transform.localRotation = Quaternion.Euler(mouseXRotation, 0f, 0f);
                character.Rotate(Vector3.up * mouseInputX);
            }
        }

        private void GetPlayerMouseInputs() {
            mouseInputX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouseInputY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            mouseXRotation -= mouseInputY;
            mouseXRotation = Mathf.Clamp(mouseXRotation, -90f, 90f);
        }
    }
}