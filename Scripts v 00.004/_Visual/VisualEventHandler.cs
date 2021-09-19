using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Utilities;
using BlueBird.World.GameTime;

namespace BlueBird.World.Visual {
    public sealed class VisualEventHandler : _0SingletonEventHandler<VisualEventHandler> {
        /* Events - For CoroutineGameTicks*/
        public delegate void VisualTickHandler();

        //public static event VisualTickHandler eventOnVisualTick;
        //public static event VisualTickHandler eventOnVisualTickMultipleOf_2;
        //public static event VisualTickHandler eventOnVisualTickMultipleOf_4;

        public static event VisualTickHandler eventOnVisualTick_1;
        //public static event VisualTickHandler eventOnVisualTick_2;
        //public static event VisualTickHandler eventOnVisualTick_3;
        //public static event VisualTickHandler eventOnVisualTick_4;
        //public static event VisualTickHandler eventOnVisualTick_5;
        //public static event VisualTickHandler eventOnVisualTick_6;
        //public static event VisualTickHandler eventOnVisualTick_7;
        //public static event VisualTickHandler eventOnVisualTick_8;

        protected override void Awake() {
            Debug.Log("<b>VisualEventHandler Awake</b> function was <color=magenta><i>fired</i></color>.");
            // VisualEventHandler Subscriptions to GameTimeManager OnTick Events
            GameTimeManager.eventOnGameTick_1 += OnGameTick_1;
        }

        private void OnEnable() {
            Debug.Log("<b>VisualEventHandler OneEnable</b> function was <color=magenta><i>fired</i></color>.");
            // Set this Object as a Child of Visual Manager
            GameObject parentObject = GameObject.Find("VisualManager");
            this.transform.SetParent(parentObject.transform, false);
        }

        private void Start() {
            Debug.Log("<b>VisualEventHandler Start</b> function was <color=magenta><i>fired</i></color>.");
            // Subscriptions to VisualEventHandler OnTick Events

            WorldDirector.checkerVisualEventStart = true;
        }

        private void OnDisable() {
            // VisualEventHandler Unsubscriptions to GameTimeManager OnTick Events
            GameTimeManager.eventOnGameTick_1 -= OnGameTick_1;

            // Unsubscriptions to VisualEventHandler OnTick Events
        }

        private void OnGameTick_1() {
            if(eventOnVisualTick_1 != null) {
                eventOnVisualTick_1();
            }
        }
    }
}