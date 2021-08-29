using UnityEngine;

namespace BlueBird.World.Assistants {
    public sealed class DirectorAssistant {
        /* Instances */
        WorldDirector WorldDirector;

        public void CreateLoadingOptionCheckerGameObject() {
            GameObject @loadingOptionCheckerObject = new GameObject("LoadingOptionChecker", typeof(LoadingOptionChecker));
            GameObject @parentObject = GameObject.FindWithTag("Director");
            @loadingOptionCheckerObject.transform.SetParent(@parentObject.transform, false);
            @loadingOptionCheckerObject.transform.localPosition = Vector3.zero;
            @loadingOptionCheckerObject.transform.localScale = Vector3.one;
            @loadingOptionCheckerObject.transform.localEulerAngles = Vector3.zero;
            @loadingOptionCheckerObject.tag = "Checker";
            @loadingOptionCheckerObject.layer = 7;
            Debug.Log("<b>" + @loadingOptionCheckerObject.name + "</b> was <color=green><i>created</i></color>.");
        }

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
        }
    }
}