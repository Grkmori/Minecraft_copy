using System.Collections;
using UnityEngine;
using BlueBird.World.Assistants;

namespace BlueBird.World {
    public sealed class WorldDirector : MonoBehaviour {
        /* Instances */ /// WorldManager
        DirectorAssistant _directorAssistant;

        /* Variables - Game Loading Options */
        internal static bool newGame = false;
        internal static bool loadGame = false;

        /* Variables - Checkers to 'default' back Loading Options  */
        internal static bool newGameWorldChecker = false;
        internal static bool newGameVisualsChecker = false;
        internal static bool newGameAudiosChecker = false;
        internal static bool loadGameWorldChecker = false;
        internal static bool loadGameVisualsChecker = false;
        internal static bool loadGameAudiosChecker = false;

        /* Variables - Game State */
        internal static bool pauseGame = true;
        internal static bool battleScene = false;

        private void Awake() {
            Debug.Log("<b>WordDirector Awake</b> function was <color=magenta><i>fired</i></color>.");
            newGame = true; // TODO: Temporary for testing, DELETE after use
            DontDestroyOnLoad(this.gameObject); // Dont destroy this GameObject on Scenes changes.

            // Creating a Director Assistant for various tasks
            _directorAssistant = new DirectorAssistant();
            _directorAssistant.CheckGameLoadingOptions(); // Return Game Loading Options (newGame, loadGame) back to false

            // Initializing Managers
            var @worldManager = WorldManager.Instance;
            var @visualManager = Visual.VisualManager.Instance;
            var @audioManager = Audio.AudioManager.Instance;
        }

        private void Start() {

        }
    }
}