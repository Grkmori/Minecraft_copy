using UnityEngine;

namespace BlueBird.World.Assistants {
    public sealed class LoadingOptionChecker : MonoBehaviour {
        /* Instances */
        WorldDirector WorldDirector;

        private void FixedUpdate() {
            if(WorldDirector.newGame) {
                if(!WorldDirector.newGameWorldChecker && !WorldDirector.newGameVisualsChecker && !WorldDirector.newGameAudiosChecker) {
                    WorldDirector.newGame = false;
                    Debug.Log("<b>Checker GameObject</b> was <color=green><i>destroyed</i></color>. <b>'newGame'</b> was set back to <color=blue><i>false</i></color>.");
                    Destroy(gameObject);
                }
            }

            if(WorldDirector.loadGame) {
                if(!WorldDirector.loadGameWorldChecker && !WorldDirector.loadGameVisualsChecker && !WorldDirector.loadGameAudiosChecker) {
                    WorldDirector.loadGame = false;
                    Debug.Log("<b>Checker GameObject</b> was <color=green><i>destroyed</i></color>. <b>'loadGame'</b> was set back to <color=blue><i>false</i></color>.");
                    Destroy(gameObject);
                }
            }
        }
    }
}