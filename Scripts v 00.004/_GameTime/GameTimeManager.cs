using UnityEngine;
using System.Collections;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;

namespace BlueBird.World.GameTime {
    public sealed class GameTimeManager : _0SingletonManager<GameTimeManager> {
        /* Instances */
        WaitForSeconds gameTickDelay = new WaitForSeconds(gameTicksInSeconds);

        /* Events - For CoroutineGameTicks*/
        public delegate void GameTickHandler();

        //public static event GameTickHandler eventOnGameTick;
        //public static event GameTickHandler eventOnGameTickMultipleOf_2;
        public static event GameTickHandler eventOnGameTickMultipleOf_4;

        public static event GameTickHandler eventOnGameTick_1;
        //public static event GameTickHandler eventOnGameTick_2;
        //public static event GameTickHandler eventOnGameTick_3;
        //public static event GameTickHandler eventOnGameTick_4;
        //public static event GameTickHandler eventOnGameTick_5;
        //public static event GameTickHandler eventOnGameTick_6;
        //public static event GameTickHandler eventOnGameTick_8;

        /* Variables - For CoroutineGameTicks */
        public static float gameSpeed = Constants_str.gameNormalSpeed;

        private static float ticksPerSecond = Constants_str.gameTicksPerSecond;
        private static float gameTicksInSeconds = (1 / ticksPerSecond) * gameSpeed;

        public static float gameTick = 0;
        public static float gameTickSeconds = 0;
        public static float gameTickMinutes = 0;
        public static float gameTickHours = 0;

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>GameTimeManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>GameTimeManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Awake'</i></color> GameTimeManager</b>.");
            }
        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>GameTimeManager Start</b> function was <color=magenta><i>fired</i></color>.");

                StartCoroutine(CoroutineGameTicks());
                Debug.Log("<b>CoroutineGameTicks</b> function has <color=magenta><i>started</i></color>.");

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>GameTimeManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> GameTimeManager</b>.");
            }
            WorldDirector.checkerGameTimeStart = true;
        }

        private void OnDisable() {
            StopCoroutine(CoroutineGameTicks());
            Debug.Log("<b>CoroutineGameTicks</b> function has been <color=magenta><i>stopped</i></color>.");
        }

        private IEnumerator CoroutineGameTicks() {
            while(true) {
                if(!WorldDirector.isPaused) {
                    gameTick++;

                    // Game Ticks Events - MultipleOf
                    //if(eventOnGameTick != null) {
                    //    eventOnGameTick();
                    //}
                    //if(gameTick % (ticksPerSecond / 4) == 0 && eventOnGameTickMultipleOf_2 != null) {
                    //eventOnGameTickMultipleOf_2();
                    //}
                    if(gameTick % (ticksPerSecond / 2) == 0 && eventOnGameTickMultipleOf_4 != null) {
                    eventOnGameTickMultipleOf_4();
                    }

                    // Game Ticks Events - Per Tick
                    if(gameTick % ticksPerSecond == 1 && eventOnGameTick_1 != null) {
                        eventOnGameTick_1();
                    }
                    //if(gameTick % ticksPerSecond == 2 && eventOnGameTick_2 != null) {
                    //    eventOnGameTick_2();
                    //}
                    //if(gameTick % ticksPerSecond == 3 && eventOnGameTick_3 != null) {
                    //    eventOnGameTick_3();
                    //}
                    //if(gameTick % ticksPerSecond == 4 && eventOnGameTick_4 != null) {
                    //    eventOnGameTick_4();
                    //}
                    //if(gameTick % ticksPerSecond == 5 && eventOnGameTick_5 != null) {
                    //    eventOnGameTick_5();
                    //}
                    //if(gameTick % ticksPerSecond == 6 && eventOnGameTick_6 != null) {
                    //    eventOnGameTick_6();
                    //}
                    //if(gameTick % ticksPerSecond == 7 && eventOnGameTick_7 != null) {
                    //    eventOnGameTick_7();
                    //}
                    //if(gameTick % ticksPerSecond == 8 && eventOnGameTick_8 != null) {
                    //    eventOnGameTick_8();
                    //}

                    // Calculating InGameTime
                    if(gameTick >= ticksPerSecond) {
                        gameTickSeconds++;
                        ///Debug.Log(gameTickSeconds + " gameTickSeconds");
                        gameTick -= ticksPerSecond;
                        if(gameTickSeconds >= 60) {
                            gameTickMinutes++;
                            ///Debug.Log(gameTickMinutes + " gameTickMinutes");
                            gameTickSeconds -= 60;
                            if(gameTickMinutes >= 60) {
                                gameTickHours++;
                                ///Debug.Log(gameTickHours + " gameTickHours");
                                gameTickMinutes -= 60;
                            }
                        }
                    }

                    yield return gameTickDelay;
                }

                yield return gameTickDelay;
            }
        }
    }
}