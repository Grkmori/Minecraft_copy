using UnityEngine;
using System.Collections;
using BlueBird.World.Assistants;

namespace BlueBird.World {
    public sealed class WorldDirector : MonoBehaviour {
        /* Instances */
        DirectorAssistant _directorAssistant;

        /* Variables - Game Loading Options */
        public static bool newGame = false;
        public static bool loadGame = false;

        /* Variables - Checkers to 'default' back Loading Options  */
        public static bool newGameWorldChecker = false;
        public static bool newGameVisualsChecker = false;
        public static bool newGameAudiosChecker = false;
        public static bool loadGameWorldChecker = false;
        public static bool loadGameVisualsChecker = false;
        public static bool loadGameAudiosChecker = false;

        /* Variables - Game State */
        public static bool pauseGame = true;
        public static bool battleScene = false;

        private void Awake() {
            Debug.Log("<b>WordDirector Awake</b> function was <color=magenta><i>fired</i></color>.");
            newGame = true; // TODO: Temporary for testing, DELETE after use

            // Creating a Director Assistant for various tasks
            _directorAssistant = new DirectorAssistant();
            _directorAssistant.CheckGameLoadingOptions(); // Turn Game Loading Options (newGame, loadGame) to true
            _directorAssistant.CreateLoadingOptionCheckerGameObject();

            // Initializing Managers
            var @worldManager = WorldManager.Instance;
            var @visualManager = Visual.VisualManager.Instance;
            var @audioManager = Audio.AudioManager.Instance;
        }

        private void Start() {
            DontDestroyOnLoad(this.gameObject); // Dont destroy this GameObject on Scenes changes
        }
    }
}