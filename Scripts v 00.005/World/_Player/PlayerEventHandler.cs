using UnityEngine;
using BlueBird.World.Utilities;
using BlueBird.World.GameTime;

namespace BlueBird.World.Player {
    public sealed class PlayerEventHandler : _0SingletonEventHandler<PlayerEventHandler> {
         /* Events - For CoroutineGameTicks*/
        public delegate void PlayerTickHandler();

        //public static event PlayerTickHandler eventOnPlayerTick;
        //public static event PlayerTickHandler eventOnPlayerTickMultipleOf_2;
        public static event PlayerTickHandler eventOnPlayerTickMultipleOf_4;

        //public static event PlayerTickHandler eventOnPlayerTick_1;
        //public static event PlayerTickHandler eventOnPlayerTick_2;
        //public static event PlayerTickHandler eventOnPlayerTick_3;
        //public static event PlayerTickHandler eventOnPlayerTick_4;
        //public static event PlayerTickHandler eventOnPlayerTick_5;
        //public static event PlayerTickHandler eventOnPlayerTick_6;
        //public static event PlayerTickHandler eventOnPlayerTick_7;
        //public static event PlayerTickHandler eventOnPlayerTick_8;

        protected override void Awake() {
            Debug.Log("<b>PlayerEventHandler Awake</b> function was <color=magenta><i>fired</i></color>.");
            // PlayerEventHandler Subscriptions to GameTimeManager OnTick Events
            GameTimeManager.eventOnGameTickMultipleOf_4 += OnGameTickMultipleOf_4;
        }

        private void OnEnable() {
            Debug.Log("<b>PlayerEventHandler OneEnable</b> function was <color=magenta><i>fired</i></color>.");
            // Set this Object as a Child of Player Manager
            GameObject parentObject = GameObject.Find("PlayerManager");
            this.transform.SetParent(parentObject.transform, false);
        }

        private void Start() {
            Debug.Log("<b>PlayerEventHandler Start</b> function was <color=magenta><i>fired</i></color>.");
            // Subscriptions to PlayerEventHandler OnTick Events

            WorldDirector.checkerPlayerEventStart = true;
        }

        private void OnDisable() {
            // PlayerEventHandler Unsubscriptions to GameTimeManager OnTick Events
            GameTimeManager.eventOnGameTickMultipleOf_4 -= OnGameTickMultipleOf_4;
        }

        private void OnGameTickMultipleOf_4() {
            if(eventOnPlayerTickMultipleOf_4 != null) {
                eventOnPlayerTickMultipleOf_4();
            }
        }
    }
}