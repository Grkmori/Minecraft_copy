using UnityEngine;
using System.Threading;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Data.InvisibleData.Chunks;

namespace BlueBird.World.Visual {
    public sealed class VisualManager : _0SingletonManager<VisualManager> {
        /* Instances */
        Thread threadChunkUpdate;

        VisualGenerator _visualGenerator = new VisualGenerator();
        VisualRuntimeChunk _visualRuntimeChunk = new VisualRuntimeChunk();
        ResourcesLoader _resourcesLoader = new ResourcesLoader();

        /* Variables - For World */
        private Vector3 playerCharacterPosition;
        private Thread ChunkUpdateThread;

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
                _resourcesLoader.LoadResources();

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
                _visualRuntimeChunk.ManualStart();

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>VisualManager Start</b> function was <color=magenta><i>fired</i></color>.");


            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> VisualManager</b>.");
            }
            WorldDirector.checkerVisualStart = true;
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {

            } else if(WorldDirector.isPaused) {

            }
        }

        private void Update() {
            if(!WorldDirector.isPaused) {
                // Update Chunks
                _visualRuntimeChunk.UpdateChunkMesh();
                if(!threadingEnable) {
                    _visualRuntimeChunk.UpdateChunkMeshData();
                }

            } else if(WorldDirector.isPaused) {

            }
        }

        private void ThreadedUpdate() {
            while(true) {
                if(!WorldDirector.isPaused) {
                    _visualRuntimeChunk.UpdateChunkMeshData();
                }
            }
        }
    }
}