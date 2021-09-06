using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Utilities;

namespace BlueBird.World.Audio {
    public sealed class AudioManager : _0SingletonManager<AudioManager> {
        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>AudioManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>AudioManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Awake'</i></color> AudioManager</b>.");
            }
        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>AudioManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>AudioManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> AudioManager</b>.");
            }
            WorldDirector.checkerAudioStart = true;
        }

        private void Update() {
            if(!WorldDirector.isPaused) {

            } else if(WorldDirector.isPaused) {

            }
        }
    }
}