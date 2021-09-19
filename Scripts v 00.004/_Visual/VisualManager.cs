using UnityEngine;
using System.Threading;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Data.Topography.Chunks;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Visual {
    public sealed class VisualManager : _0SingletonManager<VisualManager> {
        /* Instances */
        Thread threadVisualManager;

        ResourcesLoader _resourcesLoader = new ResourcesLoader();
        VisualGenerator _visualGenerator = new VisualGenerator();

        ChunkRuntime _chunkRuntime = new ChunkRuntime();
        VoxelRuntime _voxelRuntime = new VoxelRuntime();

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>VisualManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Create Sprite Atlases, Generate Visuals Dictionaries and Load Resources
                _visualGenerator.CreateSpriteAtlases();
                _visualGenerator.CreateVisualDictionaries();
                _visualGenerator.ClearVisualDictionaries();
                _resourcesLoader.LoadVisualResources();

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>VisualManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Awake'</i></color> VisualManager</b>.");
            }
        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // New Game
                Debug.Log("<b>VisualManager Start</b> function was <color=magenta><i>fired</i></color>.");

                _chunkRuntime.ManualStart();

                if(Settings_str.enableThreading) {
                    if(WorldDirector.activeThreads < Settings_str.maxThreads) {
                        threadVisualManager = new Thread(new ThreadStart(ThreadedUpdate));
                        threadVisualManager.Start();
                        WorldDirector.activeThreads++;
                        Debug.Log("<b>threadVisualManager</b> function has <color=magenta><i>started</i></color>.");
                    } else {
                        Debug.LogWarning("<b>maxThreads Limit</b> has been <color=orange><i>reached</i></color>. Couldnt <b><color=yellow><i>Start</i></color> a new Thread</b>. Threading is <color=cyan><i>disabled</i></color> in <b>VisualManager</b>.");
                        Settings_str.enableThreading = false;
                    }
                }

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>VisualManager Start</b> function was <color=magenta><i>fired</i></color>.");


            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> VisualManager</b>.");
            }
            WorldDirector.checkerVisualStart = true;
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                _chunkRuntime.QueueChunkMesh();
            } else if(WorldDirector.isPaused) {

            }
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                if(!Settings_str.enableThreading) {
                    _chunkRuntime.QueueChunkMeshData();
                    _voxelRuntime.QueueVoxelMeshData();
                    _voxelRuntime.QueueFaceVoxelMeshData();
                }
            } else if(WorldDirector.isPaused) {

            }
        }

        private void ThreadedUpdate() {
            while(Settings_str.enableThreading) {
                if(!WorldDirector.isPaused) {
                    _chunkRuntime.QueueChunkMeshData();
                    _voxelRuntime.QueueVoxelMeshData();
                    _voxelRuntime.QueueFaceVoxelMeshData();

                    Thread.Sleep(100);
                }
            }
        }

        private void OnDisable() {
            threadVisualManager.Abort();
            Debug.Log("<b>threadVisualManager</b> function has been <color=magenta><i>aborted</i></color>.");
        }
    }
}