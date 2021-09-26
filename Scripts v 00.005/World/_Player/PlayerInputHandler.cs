using UnityEngine;
using System.Runtime.InteropServices; // Used to get Information from the System for CapsLock State
using BlueBird.World.Utilities;

namespace BlueBird.World.Player {
    public sealed class PlayerInputHandler : _0SingletonEventHandler<PlayerInputHandler> {
        /* Variables - For  CapsLock */
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);

        /* Instances */
        PlayerUtilities _playerUtilities = new PlayerUtilities();

        /* Instances - For InputHandler */
        LayerMask chunkMask;

        Transform mouseVizualizer;

        /* Events - For InputHandler */
        public delegate void GameInputHandler();

        public static event GameInputHandler eventOnInputKeyEscapeDown;
        public static event GameInputHandler eventOnInputKeyF3Down;
        public static event GameInputHandler eventOnInputKeyF4Down;
        public static event GameInputHandler eventOnInputKeyF5Down;

        public static event GameInputHandler eventOnInputKeyCapsLockDown;
        public static event GameInputHandler eventOnInputKeySprintDown;
        public static event GameInputHandler eventOnInputKeySprintUp;
        public static event GameInputHandler eventOnInputKeyJumpDown;

        public static event GameInputHandler eventOnInputMouseAxisScrollWheel;

        public static event GameInputHandler eventOnInputAxisHorizontal3rd;
        public static event GameInputHandler eventOnInputAxisVertical3rd;
        public static event GameInputHandler eventOnInputKeyQ3rd;
        public static event GameInputHandler eventOnInputKeyE3rd;
        public static event GameInputHandler eventOnInputKeyR3rd;
        public static event GameInputHandler eventOnInputKeyF3rd;
        public static event GameInputHandler eventOnInputKeyGDown3rd;

        public static event GameInputHandler eventOnInputMouse0Down3rd;
        public static event GameInputHandler eventOnInputMouse1Down3rd;

        public static event GameInputHandler eventOnInputAxisHorizontal1st;
        public static event GameInputHandler eventOnInputAxisVertical1st;
        public static event GameInputHandler eventOnInputMouseAxisX1st;
        public static event GameInputHandler eventOnInputMouseAxisY1st;

        public static event GameInputHandler eventOnInputMouse0Down1st;
        public static event GameInputHandler eventOnInputMouse1Down1st;

        /* Variables - For Inputs */
        public static Vector3 mouseVoxelPosition;

        public static bool view3rdPerson = false;
        public static bool isCapsLockOn = false;

        protected override void Awake() {
            Debug.Log("<b>PlayerInputHandler Awake</b> function was <color=magenta><i>fired</i></color>.");
            // Setting up
            isCapsLockOn = (((ushort)GetKeyState(0x14)) & 0xffff) != 0; // Initial State of CapsLock
        }

        private void OnEnable() {
            Debug.Log("<b>PlayerInputHandler OneEnable</b> function was <color=magenta><i>fired</i></color>.");
            // Set this Object as a Child of Player Manager
            GameObject parentObject = GameObject.Find("PlayerManager");
            this.transform.SetParent(parentObject.transform, false);

            chunkMask = LayerMask.GetMask("Chunk");
            mouseVizualizer = GameObject.Find("MouseVizualizer").transform;
        }

        private void Start() {
            Debug.Log("<b>PlayerInputHandler Start</b> function was <color=magenta><i>fired</i></color>.");

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
            if(Input.GetKeyDown(KeyCode.Escape) && eventOnInputKeyEscapeDown !=  null) {
                eventOnInputKeyEscapeDown();
            }

            if(Input.GetKeyDown(KeyCode.F3) && eventOnInputKeyF3Down != null) {
                eventOnInputKeyF3Down();
            }
            if(Input.GetKeyDown(KeyCode.F4) && eventOnInputKeyF4Down != null) {
                eventOnInputKeyF4Down();
            }
            if(Input.GetKeyDown(KeyCode.F5) && eventOnInputKeyF5Down != null) {
                eventOnInputKeyF5Down();
            }
        }

        private void GetPlayerKeyboardInputs() {
            if(view3rdPerson) {
                if(Input.GetAxis("Horizontal") != 0 && eventOnInputAxisHorizontal3rd != null) {
                    eventOnInputAxisHorizontal3rd();
                }
                if(Input.GetAxis("Vertical") != 0 && eventOnInputAxisVertical3rd != null) {
                    eventOnInputAxisVertical3rd();
                }

                if(Input.GetKey(KeyCode.Q) && eventOnInputKeyQ3rd != null) {
                    eventOnInputKeyQ3rd();
                }
                if(Input.GetKey(KeyCode.E) && eventOnInputKeyE3rd != null) {
                    eventOnInputKeyE3rd();
                }

                if(Input.GetKey(KeyCode.R) && eventOnInputKeyR3rd != null) {
                    eventOnInputKeyR3rd();
                }
                if(Input.GetKey(KeyCode.F) && eventOnInputKeyF3rd != null) {
                    eventOnInputKeyF3rd();
                }
                if(Input.GetKeyDown(KeyCode.G) && eventOnInputKeyGDown3rd != null) {
                    eventOnInputKeyGDown3rd();
                }
            } else {
                if(Input.GetAxis("Horizontal") != 0 && eventOnInputAxisHorizontal1st != null) {
                    eventOnInputAxisHorizontal1st();
                }
                if(Input.GetAxis("Vertical") != 0 && eventOnInputAxisVertical1st != null) {
                    eventOnInputAxisVertical1st();
                }
            }

            if(Input.GetKeyDown(KeyCode.CapsLock) && eventOnInputKeyCapsLockDown != null) {
                isCapsLockOn = !isCapsLockOn;
                eventOnInputKeyCapsLockDown();
            }

            if(Input.GetButtonDown("Sprint") && eventOnInputKeySprintDown != null) {
                eventOnInputKeySprintDown();
            }
            if(Input.GetButtonUp("Sprint") && eventOnInputKeySprintUp != null) {
                eventOnInputKeySprintUp();
            }

            if(Input.GetButtonDown("Jump") && eventOnInputKeySprintDown != null) {
                eventOnInputKeyJumpDown();
            }
        }

        private void GetPlayerMouseInputs() {
            if(view3rdPerson) {
                if(Input.GetMouseButtonDown(0)) {
                    mouseVoxelPosition = _playerUtilities.GetMousePosition(chunkMask);
                    mouseVizualizer.position = mouseVoxelPosition;
                    Debug.Log(mouseVizualizer.position);
                }

                if(Input.GetMouseButtonDown(0) && eventOnInputMouse0Down3rd != null) {
                    eventOnInputMouse0Down3rd();
                }

                if(Input.GetMouseButtonDown(1) && eventOnInputMouse1Down3rd != null) {
                    eventOnInputMouse1Down3rd();
                }
            } else {
                if(Input.GetMouseButtonDown(0) && eventOnInputMouse0Down1st != null) {
                    eventOnInputMouse0Down1st();
                }
                if(Input.GetMouseButtonDown(1) && eventOnInputMouse1Down1st != null) {
                    eventOnInputMouse1Down1st();
                }

                if(Input.GetAxis("Mouse X") != 0 && eventOnInputMouseAxisX1st != null) {
                    eventOnInputMouseAxisX1st();
                }
                if(Input.GetAxis("Mouse Y") != 0 && eventOnInputMouseAxisY1st != null) {
                    eventOnInputMouseAxisY1st();
                }
            }

            if(Input.GetAxis("Mouse ScrollWheel") != 0 && eventOnInputMouseAxisScrollWheel != null) {
                eventOnInputMouseAxisScrollWheel();
            }
        }
    }
}