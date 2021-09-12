using UnityEngine;
using System.Threading;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Visual {
    public sealed class VisualManager : _0SingletonManager<VisualManager> {
        /* Instances */
        Thread threadChunkUpdate;

        ResourcesLoader _resourcesLoader = new ResourcesLoader();
        VisualGenerator _visualGenerator = new VisualGenerator();

        ChunkRuntime _chunkRuntime = new ChunkRuntime();
        VoxelRuntime _voxelRuntime = new VoxelRuntime();

        /* Variables - For Threads */
        private readonly int maxNumberThreads = Settings_str.maxThreads;
        private bool threadingEnable = Settings_str.enableThreading;

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

                if(threadingEnable) {
                    if(WorldDirector.activeThreads < maxNumberThreads) {
                        threadChunkUpdate = new Thread(new ThreadStart(ThreadedUpdate));
                        threadChunkUpdate.Start();
                        WorldDirector.activeThreads++;
                    } else {
                        Debug.LogWarning("<b>maxThreads Limit</b> has been <color=orange><i>reached</i></color>. Couldnt <b><color=yellow><i>Start</i></color> a new Thread</b>. Threading is <color=cyan><i>disabled</i></color> in <b>VisualManager</b>.");
                        threadingEnable = false;
                    }
                }
                _chunkRuntime.ManualStart();

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>VisualManager Start</b> function was <color=magenta><i>fired</i></color>.");


            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> VisualManager</b>.");
            }
            WorldDirector.checkerVisualStart = true;
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                _chunkRuntime.UpdateChunkMesh();
            } else if(WorldDirector.isPaused) {

            }
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                if(!threadingEnable) {
                    _chunkRuntime.UpdateChunkMeshData();
                    _voxelRuntime.UpdateVoxelMeshData();
                    _voxelRuntime.UpdateFaceVoxelMeshData();
                }
            } else if(WorldDirector.isPaused) {

            }
        }

        private void OnDisable() {
            threadChunkUpdate.Abort();
        }

        private void ThreadedUpdate() {
            while(threadingEnable) {
                if(!WorldDirector.isPaused) {
                    _chunkRuntime.UpdateChunkMeshData();
                    _voxelRuntime.UpdateVoxelMeshData();
                    _voxelRuntime.UpdateFaceVoxelMeshData();

                    Thread.Sleep(100);
                }
            }
        }
    }
}