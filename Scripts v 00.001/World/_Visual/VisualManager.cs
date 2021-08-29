using UnityEngine;
using BlueBird.World.Utilities;

namespace BlueBird.World.Visual {
    public sealed class VisualManager : _0Singleton<VisualManager> {
        /* Instances */
        WorldDirector WorldDirector;
        ResourcesLoader _resourcesLoader;
        VisualGenerator _visualGenerator;

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>VisualManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Setting up
                _visualGenerator = new VisualGenerator();
                _resourcesLoader = new ResourcesLoader();

                // Create Sprite Atlases, Generate Visuals Dictionaries and Load Resources
                _visualGenerator.CreateSpriteAtlases();
                _visualGenerator.CreateVisualDictionaries();
                _resourcesLoader.LoadResources();
            }
            else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>VisualManager Awake</b> function was <color=magenta><i>fired</i></color>.");
            }
            else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <i>not valid</i>. Couldnt <b><color=yellow><i>'Awake'</i></color> VisualManager</b>.");
            }
        }

        private void OnEnable() {

        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>VisualManager Start</b> function was <color=magenta><i>fired</i></color>.");

                WorldDirector.newGameVisualsChecker = false;
            }
            else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>VisualManager Start</b> function was <color=magenta><i>fired</i></color>.");

                WorldDirector.loadGameVisualsChecker = false;
            }
            else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <i>not valid</i>. Couldnt <b><color=yellow><i>'Start'</i></color> VisualManager</b>.");
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