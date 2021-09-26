using UnityEngine;
using BlueBird.World.Utilities;
using BlueBird.World.GameTime;

namespace BlueBird.World.Entity {
    public sealed class EntityEventHandler : _0SingletonEventHandler<EntityEventHandler> {
        /* Events - For CoroutineGameTicks*/
        public delegate void EntityTickHandler();

        //public static event EntityTickHandler eventOnEntityTick;
        public static event EntityTickHandler eventOnEntityTickMultipleOf_2;
        //public static event EntityTickHandler eventOnEntityTickMultipleOf_4;

        //public static event EntityTickHandler eventOnEntityTick_1;
        //public static event EntityTickHandler eventOnEntityTick_2;
        //public static event EntityTickHandler eventOnEntityTick_3;
        //public static event EntityTickHandler eventOnEntityTick_4;
        //public static event EntityTickHandler eventOnEntityTick_5;
        //public static event EntityTickHandler eventOnEntityTick_6;
        //public static event EntityTickHandler eventOnEntityTick_7;
        //public static event EntityTickHandler eventOnEntityTick_8;

        protected override void Awake() {
            Debug.Log("<b>EntityEventHandler Awake</b> function was <color=magenta><i>fired</i></color>.");
            // EntityEventHandler Subscriptions to GameTimeManager OnTick Events
            GameTimeManager.eventOnGameTickMultipleOf_2 += OnGameTickMultipleOf_2;
        }

        private void OnEnable() {
            Debug.Log("<b>EntityEventHandler OneEnable</b> function was <color=magenta><i>fired</i></color>.");
            // Set this Object as a Child of Entity Manager
            GameObject parentObject = GameObject.Find("EntityManager");
            this.transform.SetParent(parentObject.transform, false);
        }

        private void Start() {
            Debug.Log("<b>EntityEventHandler Start</b> function was <color=magenta><i>fired</i></color>.");
            // Subscriptions to EntityEventHandler OnTick Events

            WorldDirector.checkerEntityEventStart = true;
        }

        private void OnDisable() {
            // EntityEventHandler Unsubscriptions to GameTimeManager OnTick Events
            GameTimeManager.eventOnGameTickMultipleOf_2 -= OnGameTickMultipleOf_2;
        }

        private void OnGameTickMultipleOf_2() {
            if(eventOnEntityTickMultipleOf_2 != null) {
                eventOnEntityTickMultipleOf_2();
            }
        }
    }
}