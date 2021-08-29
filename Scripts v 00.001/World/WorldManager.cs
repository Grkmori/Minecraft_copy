using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;

namespace BlueBird.World {
    public sealed class WorldManager : _0Singleton<WorldManager> {
        /* Instances */
        WorldDirector WorldDirector;
        WorldGenerator _worldGenerator;

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>WorldManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Setting up
                _worldGenerator = new WorldGenerator();

                // Generate World Dictionaries and Definitions Dictionaries
                _worldGenerator.CreateWorldDictionaries();
                _worldGenerator.LoadDefinitionsDictionaries();
            }
            else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>WorldManager Awake</b> function was <color=magenta><i>fired</i></color>.");
            }
            else { // Error to Awake
                Debug.LogError("<b>Booleans newGame or loadGame</b> are <i>not valid</i>. Couldnt <b><color=red><i>'Awake'</i></color> WorldManager</b>.");
            }
        }

        private void OnEnable() {

        }

        private void Start() {
             if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>WorldManager Start</b> function was <color=magenta><i>fired</i></color>.");

                Settings_str.worldSizeChunks = new Vector2(2, 2); // TODO: Temporary for World Testing

                _worldGenerator.CreateWorldGameObject(); // Create The World GameObject
                _worldGenerator.GenerateStaticEntities(); // Generate World Data Entities

                WorldDirector.newGameWorldChecker = false;
            }
            else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>WorldManager Start</b> function was <color=magenta><i>fired</i></color>.");

                WorldDirector.loadGameWorldChecker = false;
            }
            else { // Error to Start
                Debug.LogError("<b>Booleans newGame or loadGame</b> are <i>not valid</i>. Couldnt <b><color=red><i>'Start'</i></color> WorldManager</b>.");
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
                //Time.timeScale = 0f;
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