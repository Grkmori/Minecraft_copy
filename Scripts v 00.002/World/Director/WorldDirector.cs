using UnityEngine;
using System;
using BlueBird.World.Director;

namespace BlueBird.World.Director {
    public sealed class WorldDirector : MonoBehaviour {
        /* Instances */
        Coroutine _coroutine;

        DirectorAssistant _directorAssistant;
        DirectorChecker _directorChecker;

        /* Variables - Game Loading Options */
        public static bool newGame;
        public static bool loadGame;

        /* Variables - Checkers to 'default' back Loading Options  */
        public static bool checkerWorldStart = false;
        public static bool checkerVisualStart = false;
        public static bool checkerAudioStart = false;
        public static bool checkerPlayerStart = false;
        public static bool checkerGameTimeStart = false;

        //public static bool checkerWorldEventStart = false;
        public static bool checkerVisualEventStart = false;
        //public static bool checkerAudioEventStart = false;
        //public static bool checkerPlayerEventStart = false;
        //public static bool checkerGameTimeEventStart = false;

        /* Variables - Game State */
        public static bool isPaused = true;

        /* Variables - For Threading */
        public static int activeThreads = 0;

        private void Awake() {
            Debug.Log("<b>WordDirector Awake</b> function was <color=magenta><i>fired</i></color>.");
            // Setting up Bools
            newGame = true; // TODO: Temporary for testing, DELETE after use
            loadGame = false; // TODO: Temporary for testing, DELETE after use

            // Pausing the Game, waiting for all Awake and Start Methods to Load.
            //Time.timeScale = 0f;

            // Dont destroy this GameObject on Scenes changes
            DontDestroyOnLoad(this.gameObject);

            // Creating a Director Assistant for various tasks
            _directorAssistant = new DirectorAssistant();
            _directorChecker = new DirectorChecker();
            _directorAssistant.SetGameLoadingOptions(); // Turn Game Loading Options (newGame, loadGame) to true
            _directorAssistant.CreateDirectorDictionaries();
            _directorAssistant.ClearDirectorDictionaries();

            // Initializing Managers
            var @worldManager = WorldManager.Instance;
            var @visualManager = Visual.VisualManager.Instance;
            var @audioManager = Audio.AudioManager.Instance;
            var @playerManager = Player.PlayerManager.Instance;
            var @gameTimeManager = GameTime.GameTimeManager.Instance;
            Debug.Log("<b>Managers</b> were <color=green><i>initialized</i></color>.");
        }

        private void Start() {
            Debug.Log("<b>WordDirector Start</b> function was <color=magenta><i>fired</i></color>.");

            // Initialize Coroutine Checkers
            StartCoroutine(_directorChecker.LoadingOptionsChecker());
            StartCoroutine(_directorChecker.EventHandlerChecker());
            StartCoroutine(_directorChecker.IsPausedChecker());
        }
    }
}