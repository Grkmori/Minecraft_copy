using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player.Character {
    public class CharacterCameraBehaviour : MonoBehaviour {
        /* Instances - For Camera Movement */
        Transform character;

        /* Variables - For Inputs */
        private float mouseInputXaxis;
        private float mouseInputYaxis;

        /* Variables - For Camera Movement */
        private float sensitivity = Settings_str.mouseSensitivity;

        private float momentumX = 0;
        private float momentumY = 0;
        private float mouseXRotation = 0;

        private void OnEnable() {
            PlayerInputHandler.eventOnInputMouseAxisX1st += InputMouseX;
            PlayerInputHandler.eventOnInputMouseAxisY1st += InputMouseY;
        }

        private void Start() {
            // Setting up
            character = PlayerData.dictionaryPlayerObject["Character"].transform;
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                UpdateMouseMovement();
            }
        }

        private void OnDisable() {
            PlayerInputHandler.eventOnInputMouseAxisX1st -= InputMouseX;
            PlayerInputHandler.eventOnInputMouseAxisY1st -= InputMouseY;
        }

        private void InputMouseX() {
            mouseInputXaxis = Input.GetAxis("Mouse X");
            CalculateMouseMovement();
        }

        private void InputMouseY() {
            mouseInputYaxis = Input.GetAxis("Mouse Y");
            CalculateMouseMovement();
        }

        private void CalculateMouseMovement() {
            momentumX = mouseInputXaxis * sensitivity * Time.deltaTime;
            momentumY = mouseInputYaxis * sensitivity * Time.deltaTime;

            mouseXRotation -= momentumY;
            mouseXRotation = Mathf.Clamp(mouseXRotation, -90f, 90f); // Limits Mouse Horizontal Rotation(Vertical Mouse Inputs) to 90º
        }

        private void UpdateMouseMovement() {
            transform.localRotation = Quaternion.Euler(mouseXRotation, 0f, 0f); // Translates to Camera(only) Movement from Vertical Mouse Inputs
            character.Rotate(Vector3.up * momentumX); // Translates to Character Movement from Horizontal Mouse Inputs
            momentumX = 0;
            momentumY = 0;
        }
    }
}