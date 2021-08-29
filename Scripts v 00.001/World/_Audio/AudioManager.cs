using UnityEngine;
using BlueBird.World.Utilities;

namespace BlueBird.World.Audio {
    public sealed class AudioManager : _0Singleton<AudioManager> {
        /* Instances */
        WorldDirector WorldDirector;

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>AudioManager Awake</b> function was <color=magenta><i>fired</i></color>.");
            }
            else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>AudioManager Awake</b> function was <color=magenta><i>fired</i></color>.");
            }
            else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <i>not valid</i>. Couldnt <b><color=yellow><i>'Awake'</i></color> AudioManager</b>.");
            }
        }

        private void OnEnable() {

        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>AudioManager Start</b> function was <color=magenta><i>fired</i></color>.");

                WorldDirector.newGameAudiosChecker = false;
            }
            else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>AudioManager Start</b> function was <color=magenta><i>fired</i></color>.");

                WorldDirector.loadGameAudiosChecker = false;
            }
            else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <i>not valid</i>. Couldnt <b><color=yellow><i>'Start'</i></color> AudioManager</b>.");
            }
        }

        private void FixedUpdate() {
            if(!WorldDirector.pauseGame) {
            }
            else if(WorldDirector.pauseGame) {
            }
        }

        private void Update() {
            if(!WorldDirector.pauseGame) {
            }
            else if(WorldDirector.pauseGame) {
            }
        }

        private void LateUpdate() {
            if(!WorldDirector.pauseGame) {
            }
            else if(WorldDirector.pauseGame) {
            }
        }

        private void OnDisable() {

        }
    }
}