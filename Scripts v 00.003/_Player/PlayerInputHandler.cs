using UnityEngine;
using System.Runtime.InteropServices; // Used to get Information from the System for CapsLock State
using BlueBird.World.Director;
using BlueBird.World.Utilities;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;
using BlueBird.World.Player.Character;
using BlueBird.World.Player.UI;

namespace BlueBird.World.Player {
    public sealed class PlayerInputHandler : _0SingletonEventHandler<PlayerInputHandler> {
        /* Variables - For  CapsLock */
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);

        /* Instances */
        LayerMask chunkMask;

        GameObject playerObject;
        GameObject mainCameraFocusObject;
        GameObject playerToolbar;

        GameObject characterObject;
        GameObject characterCameraObject;

        Transform mouseVizualizer;
        Ray mousePositionRay;
        RaycastHit mousePositionHit;

        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        PlayerBehaviour _playerBehaviour;
        PlayerMainCameraBehaviour _playerMainCameraBehaviour;
        PlayerToolbar _playerToolbar;

        CharacterBehaviour _characterBehaviour;
        CharacterInteraction _characterInteraction;
        CharacterCameraBehaviour _characterCameraBehaviour;

        /* Variables - For Inputs */
        private static Vector3 mousePosition;
        private static Vector3 rayDirectionNormalized;
        private Vector3 mousePositionCorrection = new Vector3(0.02f, 0.98f, 0.02f);
        private static Vector3 mousePositionNormalized;
        private static Vector3 mouseVoxelPosition;
        private static Vector3 mouseChunkPosition;

        public static bool view3rdPerson = false;
        public static bool isCapsLockOn = false;

        protected override void Awake() {
            Debug.Log("<b>PlayerInputHandler Awake</b> function was <color=magenta><i>fired</i></color>.");
            // Setting up
            isCapsLockOn = (((ushort)GetKeyState(0x14)) & 0xffff) != 0; // Initial State of CapsLock
            chunkMask = LayerMask.GetMask("Chunk");
        }

        private void OnEnable() {
            Debug.Log("<b>PlayerInputHandler OneEnable</b> function was <color=magenta><i>fired</i></color>.");
            // Set this Object as a Child of Player Manager
            GameObject @parentObject = GameObject.Find("PlayerManager");
            this.transform.SetParent(@parentObject.transform, false);
            mouseVizualizer = GameObject.Find("MouseVizualizer").transform;
        }

        private void Start() {
            Debug.Log("<b>PlayerInputHandler Start</b> function was <color=magenta><i>fired</i></color>.");
            // Setting up
            playerObject = WorldData.dictionaryMainObject["Player"];
            _playerBehaviour = playerObject.GetComponent<PlayerBehaviour>();
            mainCameraFocusObject = PlayerData.dictionaryPlayerObject["MainCameraFocus"];
            _playerMainCameraBehaviour = mainCameraFocusObject.GetComponent<PlayerMainCameraBehaviour>();
            playerToolbar = PlayerData.dictionaryPlayerUIObject["PlayerToolbar"];
            _playerToolbar = playerToolbar.GetComponent<PlayerToolbar>();

            characterObject = PlayerData.dictionaryPlayerObject["Character"];
            _characterBehaviour = characterObject.GetComponent<CharacterBehaviour>();
            _characterInteraction = characterObject.GetComponent<CharacterInteraction>();
            characterCameraObject = PlayerData.dictionaryPlayerObject["CharacterCamera"];
            _characterCameraBehaviour = characterCameraObject.GetComponent<CharacterCameraBehaviour>();

            WorldDirector.checkerPlayerInputStart = true;
        }

        private void Update() {
            GetPlayerGeneralInputs();

            if(!WorldDirector.isPaused) {
                GetPlayerKeyboardInputs();
                GetPlayerMouseInputs();
            }
        }

        private void GetPlayerGeneralInputs() {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                _playerBehaviour.InputKeyEscape();
            }

            if(Input.GetKeyDown(KeyCode.F3)) {
                _playerBehaviour.InputKeyF3();
            }
            if(Input.GetKeyDown(KeyCode.F4)) {
                _playerBehaviour.InputKeyF4();
            }
            if(Input.GetKeyDown(KeyCode.F5)) {
                _playerBehaviour.InputKeyF5();
                view3rdPerson = !view3rdPerson;
            }
        }

        private void GetPlayerKeyboardInputs() {
            if(view3rdPerson) {
                if(Input.GetAxis("Horizontal") != 0) {
                    _playerMainCameraBehaviour.InputAxisHorizontal();
                }
                if(Input.GetAxis("Vertical") != 0) {
                    _playerMainCameraBehaviour.InputAxisVertical();
                }

                if(Input.GetKey(KeyCode.Q)) {
                    _playerMainCameraBehaviour.InputKeyQ();
                }
                if(Input.GetKey(KeyCode.E)) {
                    _playerMainCameraBehaviour.InputKeyE();
                }

                if(Input.GetKey(KeyCode.R)) {
                    _playerMainCameraBehaviour.InputKeyR();
                }
                if(Input.GetKey(KeyCode.F)) {
                    _playerMainCameraBehaviour.InputKeyF();
                }

                if(Input.GetKeyDown(KeyCode.CapsLock)) {
                    isCapsLockOn = !isCapsLockOn;
                    _playerMainCameraBehaviour.InputKeyCapsLockDown();
                }
            }

            if(!view3rdPerson) {
                if(Input.GetAxis("Horizontal") != 0) {
                    _characterBehaviour.InputAxisHorizontal();
                }
                if(Input.GetAxis("Vertical") != 0) {
                    _characterBehaviour.InputAxisVertical();
                }
            }

            if(Input.GetButtonDown("Sprint")) {
                _characterBehaviour.InputKeySprintDown();
            }
            if(Input.GetButtonUp("Sprint")) {
                _characterBehaviour.InputKeySprintUp();
            }

            if(Input.GetButtonDown("Jump")) {
                _characterBehaviour.InputKeyJumpDown();
            }
        }

        private void GetPlayerMouseInputs() {
            if(view3rdPerson) {
                if(Input.GetMouseButtonDown(0)) {
                    GetMousePosition();
                }
            }

            if(!view3rdPerson) {
                if(Input.GetMouseButtonDown(0)) {
                    _characterInteraction.DestroyVoxelOnCursorPosition();
                }
                if(Input.GetMouseButtonDown(1)) {
                    _characterInteraction.PlaceVoxelOnCursorPosition();
                }

                if(Input.GetAxis("Mouse X") != 0) {
                    _characterCameraBehaviour.InputMouseX();
                }
                if(Input.GetAxis("Mouse Y") != 0) {
                    _characterCameraBehaviour.InputMouseY();
                }
            }

            if(Input.GetAxis("Mouse ScrollWheel") != 0) {
                _playerToolbar.InputMouseScrollWheel();
            }
        }

        private void GetMousePosition() {
            mousePositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(mousePositionRay, out mousePositionHit, 200, chunkMask)) {
                mousePosition = mousePositionHit.point;
                rayDirectionNormalized = new Vector3(mousePositionRay.direction.x / Mathf.Abs(mousePositionRay.direction.x),
                                                     mousePositionRay.direction.y / Mathf.Abs(mousePositionRay.direction.y),
                                                     mousePositionRay.direction.z / Mathf.Abs(mousePositionRay.direction.z));
                mousePositionNormalized = mousePosition + new Vector3(mousePositionCorrection.x * rayDirectionNormalized.x,
                                                                      mousePositionCorrection.y * -rayDirectionNormalized.y,
                                                                      mousePositionCorrection.z * rayDirectionNormalized.z);

                mouseVoxelPosition = _voxelUtilities.GetVoxelPositionFromPosition(mousePositionNormalized);
                mouseChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(mousePositionNormalized);

                mouseVizualizer.transform.position = mouseVoxelPosition;

                Debug.Log(mousePosition + " " + mouseVoxelPosition + " " + mouseChunkPosition);
            }
        }
    }
}