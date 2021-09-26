using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player {
    public sealed class PlayerMainCameraBehaviour : MonoBehaviour {
        /* Instances - For Camera Movement */
        Transform mainCameraTransform;

        /* Variables - For Inputs */
        private float inputXaxis;
        private float inputZaxis;
        private float inputQE;

        /* Variables - For Camera Movement */
        private readonly float movementTimeSpeed = Settings_str.mainCameraMovementTimeSpeed;
        private readonly float normalSpeed = Settings_str.mainCameraNormalMovementSpeed;
        private readonly float fastSpeed = Settings_str.mainCameraFastMovementSpeed;
        private readonly float rotationSpeed = Settings_str.mainCameraRotationSpeed;
        private readonly Vector3 zoomAmount = Settings_str.mainCameraZoomAmount;
        private readonly Vector3 minZoom = new Vector3(0, Constants_str.mainCameraPosition.y + Settings_str.mainCameraMinimumZoomAmount.y, Constants_str.mainCameraPosition.z + Settings_str.mainCameraMinimumZoomAmount.z);
        private readonly Vector3 maxZoom = new Vector3(0, Constants_str.mainCameraPosition.y + Settings_str.mainCameraMaximumZoomAmount.y, Constants_str.mainCameraPosition.z + Settings_str.mainCameraMaximumZoomAmount.z);

        private bool isFastSpeedOn;

        private Vector3 newPosition;
        private Quaternion newRotation;
        private Vector3 newZoom;

        private void OnEnable() {
            PlayerInputHandler.eventOnInputAxisHorizontal3rd += InputCameraMovement;
            PlayerInputHandler.eventOnInputAxisVertical3rd += InputCameraMovement;
            PlayerInputHandler.eventOnInputKeyQ3rd += InputKeyQ;
            PlayerInputHandler.eventOnInputKeyE3rd += InputKeyE;
            PlayerInputHandler.eventOnInputKeyR3rd += InputKeyR;
            PlayerInputHandler.eventOnInputKeyF3rd += InputKeyF;
            PlayerInputHandler.eventOnInputKeyCapsLockDown += InputKeyCapsLockDown;
        }

        private void Start() {
            // Setting up
            mainCameraTransform = GameObject.FindWithTag("MainCamera").transform;
            Camera mainCamera = mainCameraTransform.gameObject.GetComponent<Camera>();

            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = mainCameraTransform.localPosition;
        }

        private void FixedUpdate() {
            UpdateCamera();
        }

        private void OnDisable() {
            PlayerInputHandler.eventOnInputAxisHorizontal3rd -= InputCameraMovement;
            PlayerInputHandler.eventOnInputAxisVertical3rd -= InputCameraMovement;
            PlayerInputHandler.eventOnInputKeyQ3rd -= InputKeyQ;
            PlayerInputHandler.eventOnInputKeyE3rd -= InputKeyE;
            PlayerInputHandler.eventOnInputKeyR3rd -= InputKeyR;
            PlayerInputHandler.eventOnInputKeyF3rd -= InputKeyF;
            PlayerInputHandler.eventOnInputKeyCapsLockDown -= InputKeyCapsLockDown;
        }

        private void InputCameraMovement() {
            inputXaxis = Input.GetAxis("Horizontal");
            inputZaxis = Input.GetAxis("Vertical");
            CalculateCameraMovement();
        }

        private void InputKeyQ() {
            inputQE = 1;
            CalculateCameraRotation();
        }

        private void InputKeyE() {
            inputQE = -1;
            CalculateCameraRotation();
        }

        private void InputKeyR() {
            newZoom += zoomAmount;
            CalculateCameraZoom();
        }
        private void InputKeyF() {
            newZoom -= zoomAmount;
            CalculateCameraZoom();
        }

        private void InputKeyCapsLockDown() {
            isFastSpeedOn = PlayerInputHandler.isCapsLockOn;
        }

        private void CalculateCameraMovement() {
            if(!isFastSpeedOn) {
                newPosition += (transform.right * inputXaxis * normalSpeed * Time.fixedDeltaTime);
                newPosition += (transform.forward * inputZaxis * normalSpeed * Time.fixedDeltaTime);
            } else {
                newPosition += (transform.right * inputXaxis * fastSpeed * Time.fixedDeltaTime);
                newPosition += (transform.forward * inputZaxis * fastSpeed * Time.fixedDeltaTime);
            }
        }

        private void CalculateCameraRotation() {
            newRotation *= Quaternion.Euler(Vector3.up * inputQE * rotationSpeed);
        }

        private void CalculateCameraZoom() {
            newZoom.y = Mathf.Clamp(newZoom.y, minZoom.y, maxZoom.y);
            newZoom.z = Mathf.Clamp(newZoom.z, minZoom.z, maxZoom.z);
        }

        private void UpdateCamera() {
            // Horizontal Movement on X and Z Axis
            transform.position = Vector3.Lerp(transform.position, newPosition, movementTimeSpeed * Time.fixedDeltaTime);

            // Horizontal Rotation on Screen Center
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTimeSpeed * Time.fixedDeltaTime);

            // Main Camera Zoom Amount
            mainCameraTransform.localPosition = Vector3.Lerp(mainCameraTransform.localPosition, newZoom, movementTimeSpeed * Time.fixedDeltaTime);
        }
    }
}