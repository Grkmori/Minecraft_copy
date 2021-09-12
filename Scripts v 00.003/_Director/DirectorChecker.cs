using UnityEngine;
using System.Collections;
using BlueBird.World.Player;

namespace BlueBird.World.Director {
    public sealed class DirectorChecker {
        /* Instances */
        GameObject characterObject;

        WaitForSeconds checkersDelay = new WaitForSeconds(checkersDelayTime);

        DirectorAssistant _directorAssistant = new DirectorAssistant();

        /* Variables - For Coroutine Checkers*/
        private bool checkerLoadingOptions = false;
        private bool checkerEventHandler = false;
        private bool checkerIsPaused = false;
        private static float checkersDelayTime = 0.5f;

        public IEnumerator LoadingOptionsChecker() {
            while(!checkerLoadingOptions) {
                if(WorldDirector.checkerWorldStart && WorldDirector.checkerVisualStart && WorldDirector.checkerAudioStart && WorldDirector.checkerPlayerStart && WorldDirector.checkerGameTimeStart) {
                    if(WorldDirector.newGame) {
                        WorldDirector.newGame = false;
                        Debug.Log("<b>'newGame'</b> was set back to <color=blue><i>" + WorldDirector.newGame + "</i></color>.");
                    }
                    if(WorldDirector.loadGame) {
                        WorldDirector.loadGame = false;
                        Debug.Log("<b>'loadGame'</b> was set back to <color=blue><i>" + WorldDirector.loadGame + "</i></color>.");
                    }
                    checkerLoadingOptions = true;
                }

                yield return checkersDelay;
            }
            checkerEventHandler = true;
        }

        public IEnumerator HandlerChecker() {
            while(!checkerEventHandler) {
                yield return checkersDelay;
            }
            _directorAssistant.InitializeHandlers();
        }

        public IEnumerator IsPausedChecker() {
            while(!checkerIsPaused) {
                if(WorldDirector.checkerVisualEventStart) {
                    // Populating DirectorData Dictionaries
                    _directorAssistant.PopulateDirectorDictionaries();

                    characterObject = PlayerData.dictionaryPlayerObject["Character"];
                    Rigidbody @characterRigidBody = characterObject.GetComponent<Rigidbody>();
                    @characterRigidBody.useGravity = true;

                    //Time.timeScale = 1f;
                    WorldDirector.isPaused = false;
                    Debug.Log("<b>'isPaused'</b> was set to <color=blue><i>" + WorldDirector.isPaused + "</i></color>.");

                    checkerIsPaused = true;
                }

                yield return checkersDelay;
            }
        }
    }
}