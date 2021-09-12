using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Utilities;

namespace BlueBird.World {
    public sealed class WorldManager : _0SingletonManager<WorldManager> {
        /* Instances */
        GameObject worldObject;

        ResourcesLoader _resourcesLoader = new ResourcesLoader();
        WorldGenerator _worldGenerator = new WorldGenerator();

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>WorldManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Generate World Dictionaries and Definitions Dictionaries
                _worldGenerator.CreateWorldDictionaries();
                _worldGenerator.ClearWorldDictionaries();
                _worldGenerator.LoadDefinitionsDictionaries();
                _resourcesLoader.LoadWorldResources();

                // Create WorldMap GameObject
                worldObject = Instantiate(WorldData.dictionaryStaticWorldPrefabs["WorldMap"], Vector3.zero, Quaternion.identity);
                _worldGenerator.SetupWorldGameObject(worldObject);

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>WorldManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogError("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=red><i>'Awake'</i></color> WorldManager</b>.");
            }
        }

        private void Start() {
             if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>WorldManager Start</b> function was <color=magenta><i>fired</i></color>.");

                // Generate The World
                _worldGenerator.GenerateNoiseMaps();
                _worldGenerator.GenerateStaticEntities();

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>WorldManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogError("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=red><i>'Start'</i></color> WorldManager</b>.");
            }
            WorldDirector.checkerWorldStart = true;
        }

        private void Update() {
            if(!WorldDirector.isPaused) {

            } else if(WorldDirector.isPaused) {

            }
        }
    }
}