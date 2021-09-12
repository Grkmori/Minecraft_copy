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
        public static float gameTickLoop = 0;
        public static float gameTickLoop10 = 0;
        public static float gameTickLoop100 = 0;

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

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>GameTimeManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> GameTimeManager</b>.");
            }
            WorldDirector.checkerGameTimeStart = true;
        }

        private void OnDisable() {
            StopCoroutine(CoroutineGameTicks());
        }

        private IEnumerator CoroutineGameTicks() {
            while(true) {
                if(!WorldDirector.isPaused) {
                    gameTick++;

                    // Game Ticks Events - MultipleOf
                    //if(eventOnGameTick != null) {
                    //    eventOnGameTick();
                    //}
                    //if(eventOnGameTickMultipleOf_2 != null && gameTick % (ticksPerSecond / 4) == 0) {
                    //eventOnGameTickMultipleOf_2();
                    //}
                    if(eventOnGameTickMultipleOf_4 != null && gameTick % (ticksPerSecond / 2) == 0) {
                    eventOnGameTickMultipleOf_4();
                    }

                    // Game Ticks Events - Per Tick
                    if(eventOnGameTick_1 != null && gameTick % ticksPerSecond == 1) {
                        eventOnGameTick_1();
                    }
                    //if(eventOnGameTick_2 != null && gameTick % ticksPerSecond == 2) {
                    //    eventOnGameTick_2();
                    //}
                    //if(eventOnGameTick_3 != null && gameTick % ticksPerSecond == 3) {
                    //    eventOnGameTick_3();
                    //}
                    //if(eventOnGameTick_4 != null && gameTick % ticksPerSecond == 4) {
                    //    eventOnGameTick_4();
                    //}
                    //if(eventOnGameTick_5 != null && gameTick % ticksPerSecond == 5) {
                    //    eventOnGameTick_5();
                    //}
                    //if(eventOnGameTick_6 != null && gameTick % ticksPerSecond == 6) {
                    //    eventOnGameTick_6();
                    //}
                    //if(eventOnGameTick_7 != null && gameTick % ticksPerSecond == 7) {
                    //    eventOnGameTick_7();
                    //}
                    //if(eventOnGameTick_8 != null && gameTick % ticksPerSecond == 8) {
                    //    eventOnGameTick_8();
                    //}

                    // Calculating InGameTime
                    if(gameTick >= ticksPerSecond) {
                        gameTickLoop++;
                        ///Debug.Log(gameTickLoop + " gameTickLoop");
                        gameTick -= ticksPerSecond;
                        if(gameTickLoop >= 10) {
                            gameTickLoop10++;
                            ///Debug.Log(gameTickLoop10 + " gameTickLoop10");
                            gameTickLoop -= 10;
                            if(gameTickLoop10 >= 10) {
                                gameTickLoop100++;
                                ///Debug.Log(gameTickLoop100 + " gameTickLoop100");
                                gameTickLoop10 -= 10;
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