using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World {
    public sealed class WorldDirector : MonoBehaviour {
        /* Instances */
        DirectorAssistant _directorAssistant;
        DirectorChecker _directorChecker;

        /* Variables - Game Loading Options */
        public static bool newGame;
        public static bool loadGame;

        /* Variables - Checkers to 'default' back Loading Options  */
        public static bool checkerWorldMapStart;
        public static bool checkerVisualStart;
        public static bool checkerAudioStart;
        public static bool checkerPlayerStart;
        public static bool checkerEntityStart;
        public static bool checkerGameTimeStart;

        //public static bool checkerWorldMapEventStart;
        public static bool checkerVisualEventStart;
        //public static bool checkerAudioEventStart;
        public static bool checkerPlayerEventStart;
        public static bool checkerPlayerInputStart;
        public static bool checkerEntityEventStart;

        /* Variables - Game State */
        public static bool isPaused = true;

        /* Variables - For Threading */
        public static int activeThreads = 0;

        private void Awake() {
            Debug.Log("<b>WordDirector Awake</b> function was <color=magenta><i>fired</i></color>.");
            // Setting up Bools
            newGame = true; // TODO: Temporary for testing, DELETE after use
            loadGame = false; // TODO: Temporary for testing, DELETE after use

            // Game Overall Settings
            //Time.timeScale = 0f;
            QualitySettings.vSyncCount = Settings_str.vSync;
            //Application.targetFrameRate = Settings_str.targetFPS;

            // Creating a Director Assistant for various tasks
            _directorAssistant = new DirectorAssistant();
            _directorChecker = new DirectorChecker();
            _directorAssistant.SetGameLoadingOptions(); // Turn Game Loading Options (newGame, loadGame) to true
            _directorAssistant.CreateDirectorDictionaries();
            _directorAssistant.ClearDirectorDictionaries();

            // Initializing Managers
            var worldMapManager = WorldMap.WorldMapManager.Instance;
            var visualManager = Visual.VisualManager.Instance;
            var audioManager = Audio.AudioManager.Instance;
            var playerManager = Player.PlayerManager.Instance;
            var entityManager = Entity.EntityManager.Instance;
            var gameTimeManager = GameTime.GameTimeManager.Instance;
            Debug.Log("<b>Managers</b> were <color=green><i>initialized</i></color>.");
        }

        private void Start() {
            Debug.Log("<b>WordDirector Start</b> function was <color=magenta><i>fired</i></color>.");

            // Initialize Coroutine Checkers
            StartCoroutine(_directorChecker.LoadingOptionsChecker());
            StartCoroutine(_directorChecker.HandlerChecker());
            StartCoroutine(_directorChecker.IsPausedChecker());
        }
    }
}