using UnityEngine;
using System.Threading;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Player.Character;

namespace BlueBird.World.Player {
    public sealed class PlayerManager : _0SingletonManager<PlayerManager> {
        /* Instances */
        Thread threadPlayerManager;

        PlayerManagerAssistant _playerManagerAssistant;

        PlayerGenerator _playerGenerator = new PlayerGenerator();

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>PlayerManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Generate Player Dictionaries
                _playerGenerator.CreatePlayerDictionaries();
                _playerGenerator.ClearPlayerDictionaries();

                GameObject playerManagerAssistant = new GameObject("PlayerManagerAssistant", typeof(PlayerManagerAssistant)) { // Create a MonoBehaviour to assist on Creating/Instantiating GameObjects
                    tag = "Assistant",
                    layer = 7
                };
                _playerManagerAssistant = playerManagerAssistant.GetComponent<PlayerManagerAssistant>();

                // Generate Player GameObjects and Setups
                _playerManagerAssistant.CreatePlayerGameObjects();
                _playerManagerAssistant.CreateCharacterGameObjects();

                playerManagerAssistant.SetActive(false);  // Disable/Destroy Assistant GameObject that is no longer useful

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>PlayerManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Awake'</i></color> PlayerManager</b>.");
            }
        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>PlayerManager Start</b> function was <color=magenta><i>fired</i></color>.");

                if(Settings_str.enableThreading) {
                    if(WorldDirector.activeThreads < Settings_str.maxThreads) {
                        threadPlayerManager = new Thread(new ThreadStart(ThreadedUpdate));
                        threadPlayerManager.Start();
                        WorldDirector.activeThreads++;
                        Debug.Log("<b>threadPlayerManager</b> function has <color=magenta><i>started</i></color>.");
                    } else {
                        Debug.LogWarning("<b>maxThreads Limit</b> has been <color=orange><i>reached</i></color>. Couldnt <b><color=yellow><i>Start</i></color> a new Thread</b>. Threading is <color=cyan><i>disabled</i></color> in <b>PlayerManager</b>.");
                        Settings_str.enableThreading = false;
                    }
                }

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>PlayerManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> PlayerManager</b>.");
            }

            WorldDirector.checkerPlayerStart = true;
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
            Debug.Log("<b>threadPlayerManager</b> function has been <color=magenta><i>aborted</i></color>.");
        }
    }
}
