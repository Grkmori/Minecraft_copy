using UnityEngine;
using System.Threading;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World {
    public sealed class WorldManager : _0SingletonManager<WorldManager> {
        /* Instances */
        Thread threadWorldManager;

        GameObject worldObject;

        ResourcesLoader _resourcesLoader = new ResourcesLoader();
        WorldGenerator _worldGenerator = new WorldGenerator();

        VoxelRuntime _voxelRuntime = new VoxelRuntime();

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

                if(Settings_str.enableThreading) {
                    if(WorldDirector.activeThreads < Settings_str.maxThreads) {
                        threadWorldManager = new Thread(new ThreadStart(ThreadedUpdate));
                        threadWorldManager.Start();
                        WorldDirector.activeThreads++;
                        Debug.Log("<b>threadWorldManager</b> function has <color=magenta><i>started</i></color>.");
                    } else {
                        Debug.LogWarning("<b>maxThreads Limit</b> has been <color=orange><i>reached</i></color>. Couldnt <b><color=yellow><i>Start</i></color> a new Thread</b>. Threading is <color=cyan><i>disabled</i></color> in <b>WorldManager</b>.");
                        Settings_str.enableThreading = false;
                    }
                }

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>WorldManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogError("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=red><i>'Start'</i></color> WorldManager</b>.");
            }
            WorldDirector.checkerWorldStart = true;
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                if(!Settings_str.enableThreading) {
                    _voxelRuntime.QueueListWalkableVoxels();
                    _voxelRuntime.QueueListVoxelNeighboursOfBaseVoxel();
                    _voxelRuntime.QueueListVoxelNeighboursOfNeighbours();
                }
            } else if(WorldDirector.isPaused) {

            }
        }

        private void ThreadedUpdate() {
            while(Settings_str.enableThreading) {
                if(!WorldDirector.isPaused) {
                    _voxelRuntime.QueueListWalkableVoxels();
                    _voxelRuntime.QueueListVoxelNeighboursOfBaseVoxel();
                    _voxelRuntime.QueueListVoxelNeighboursOfNeighbours();

                    Thread.Sleep(100);
                }
            }
        }
        private void OnDisable() {
            threadWorldManager.Abort();
            Debug.Log("<b>threadWorldManager</b> function has been <color=magenta><i>aborted</i></color>.");
        }
    }
}