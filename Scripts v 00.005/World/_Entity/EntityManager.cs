using UnityEngine;
using System.Threading;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;

namespace BlueBird.World.Entity {
    public sealed class EntityManager : _0SingletonManager<EntityManager> {
        /* Instances */
        EntityGenerator _entityGenerator = new EntityGenerator();

        ResourcesLoader _resourcesLoader = new ResourcesLoader();

        /* Instances - For Threading */
        Thread threadPlayerManager;

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>EntityManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Generate Entity Dictionaries
                _entityGenerator.CreateEntityDictionaries();
                _entityGenerator.ClearEntityDictionaries();
                _resourcesLoader.LoadEntityResources();

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>EntityManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Awake'</i></color> EntityManager</b>.");
            }
        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>EntityManager Start</b> function was <color=magenta><i>fired</i></color>.");

                if(Settings_str.enableThreading) {
                    if(WorldDirector.activeThreads < Settings_str.maxThreads) {
                        threadPlayerManager = new Thread(new ThreadStart(ThreadedUpdate));
                        threadPlayerManager.Start();
                        WorldDirector.activeThreads++;
                        Debug.Log("<b>threadEntityManager</b> function has <color=magenta><i>started</i></color>.");
                    } else {
                        Debug.LogWarning("<b>maxThreads Limit</b> has been <color=orange><i>reached</i></color>. Couldnt <b><color=yellow><i>Start</i></color> a new Thread</b>. Threading is <color=cyan><i>disabled</i></color>.");
                        Settings_str.enableThreading = false;
                    }
                }

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>EntityManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> EntityManager</b>.");
            }
            WorldDirector.checkerEntityStart = true;
        }

        private void Update() {
            if(!WorldDirector.isPaused) {

            } else if(WorldDirector.isPaused) {

            }
        }

        private void ThreadedUpdate() {
            while(Settings_str.enableThreading) {
                if(!WorldDirector.isPaused) {

                    Thread.Sleep(100);
                }
            }
        }

        private void OnDisable() {
            threadPlayerManager.Abort();
            Debug.Log("<b>threadEntityManager</b> function has been <color=magenta><i>aborted</i></color>.");
        }
    }
}