using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Data.InvisibleData.Chunks;

namespace BlueBird.World.Player {
    public sealed class PlayerMainCameraBehaviour : MonoBehaviour {
        /* Instances */
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

        public void InputAxisHorizontal() {
            inputXaxis = Input.GetAxis("Horizontal");
        }
        public void InputAxisVertical() {
            inputZaxis = Input.GetAxis("Vertical");
        }

        public void InputKeyQ() {
            inputQE = 1;
        }
        public void InputKeyE() {
            inputQE = -1;
        }

        public void InputKeyR() {
            newZoom += zoomAmount;
        }
        public void InputKeyF() {
            newZoom -= zoomAmount;
        }

        public void InputKeyCapsLockDown() {
            isFastSpeedOn = PlayerInputHandler.isCapsLockOn;
            Debug.Log(isFastSpeedOn);
        }

        private void UpdateCamera() {
            // Horizontal Movement on X and Z Axis
            if(!isFastSpeedOn) {
                newPosition += (transform.right * inputXaxis * normalSpeed * Time.fixedDeltaTime);
                newPosition += (transform.forward * inputZaxis * normalSpeed * Time.fixedDeltaTime);
            } else {
                newPosition += (transform.right * inputXaxis * fastSpeed * Time.fixedDeltaTime);
                newPosition += (transform.forward * inputZaxis * fastSpeed * Time.fixedDeltaTime);
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, movementTimeSpeed * Time.fixedDeltaTime);
            inputXaxis = 0;
            inputZaxis = 0;

            // Horizontal Rotation on Screen Center
            newRotation *= Quaternion.Euler(Vector3.up * inputQE * rotationSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, movementTimeSpeed * Time.fixedDeltaTime);
            inputQE = 0;

            // Main Camera Zoom Amount
            newZoom.y = Mathf.Clamp(newZoom.y, minZoom.y, maxZoom.y);
            newZoom.z = Mathf.Clamp(newZoom.z, minZoom.z, maxZoom.z);
            mainCameraTransform.localPosition = Vector3.Lerp(mainCameraTransform.localPosition, newZoom, movementTimeSpeed * Time.fixedDeltaTime);
        }
    }
}