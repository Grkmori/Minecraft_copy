using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player.Character {
    public class CharacterCameraBehaviour : MonoBehaviour {
        /* Instances */
        Transform character;

        /* Variables - For Inputs */
        private float mouseInputXaxis;
        private float mouseInputYaxis;

        /* Variables - For Camera Movement */
        private float sensitivity = Settings_str.mouseSensitivity;

        private float momentumX = 0;
        private float momentumY = 0;
        private float mouseXRotation = 0;

        private void Start() {
            // Setting up
            character = PlayerData.dictionaryPlayerObject["Character"].transform;
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                UpdateMouseMovement();

                transform.localRotation = Quaternion.Euler(mouseXRotation, 0f, 0f); // Translates to Camera(only) Movement from Vertical Mouse Inputs
                character.Rotate(Vector3.up * momentumX); // Translates to Character Movement from Horizontal Mouse Inputs
            }
        }

        public void InputMouseX() {
            mouseInputXaxis = Input.GetAxis("Mouse X");
        }

        public void InputMouseY() {
            mouseInputYaxis = Input.GetAxis("Mouse Y");
        }

        private void UpdateMouseMovement() {
            momentumX = mouseInputXaxis * sensitivity * Time.deltaTime;
            momentumY = mouseInputYaxis * sensitivity * Time.deltaTime;

            mouseXRotation -= momentumY;
            mouseXRotation = Mathf.Clamp(mouseXRotation, -90f, 90f); // Limits Mouse Horizontal Rotation(Vertical Mouse Inputs) to 90º

            mouseInputXaxis = 0;
            mouseInputYaxis = 0;
        }
    }
}