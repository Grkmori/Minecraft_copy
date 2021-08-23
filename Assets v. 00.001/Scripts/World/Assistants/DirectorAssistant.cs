using UnityEngine;

namespace BlueBird.World.Assistants {
    public sealed class DirectorAssistant {
        /* Instances */
        private GameObject _gameObject;
        private Transform _transform;

        private WorldDirector WorldDirector;

        public void CheckGameLoadingOptions() {
            if(WorldDirector.newGame) {
                WorldDirector.newGameWorldChecker = true;
                WorldDirector.newGameVisualsChecker = true;
                WorldDirector.newGameAudiosChecker = true;
            }
            if(WorldDirector.loadGame) {
                WorldDirector.loadGameWorldChecker = true;
                WorldDirector.loadGameVisualsChecker = true;
                WorldDirector.loadGameAudiosChecker = true;
            }

            GameObject @checkerObject = new GameObject("Assistant", typeof(LoadingOptionChecker));
            GameObject @parentObject = GameObject.FindWithTag("Director");
            @checkerObject.transform.SetParent(@parentObject.transform, false);
            @checkerObject.transform.localPosition = Vector3.zero;
            @checkerObject.transform.localScale = Vector3.one;
            @checkerObject.transform.localEulerAngles = Vector3.zero;
            @checkerObject.tag = "Assistant";
            @checkerObject.layer = 7;
            Debug.Log("<b>Assistant GameObject</b> was <color=green><i>created</i></color>.");
        }
    }
}