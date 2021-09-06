using UnityEngine;
using System.Collections.Concurrent;

namespace BlueBird.World.Director {
    public sealed class DirectorAssistant {
        public void SetGameLoadingOptions() {
            // Set/ReSet Checkers to False
            WorldDirector.checkerWorldStart = false;
            WorldDirector.checkerVisualStart = false;
            WorldDirector.checkerAudioStart = false;
            WorldDirector.checkerPlayerStart = false;
            WorldDirector.checkerGameTimeStart = false;

            //WorldDirector.checkerWorldEventStart = false;
            WorldDirector.checkerVisualEventStart = false;
            //WorldDirector.checkerAudioEventStart = false;
            //WorldDirector.checkerPlayerEventStart = false;
            //WorldDirector.checkerGameTimeEventStart = false;
        }

        public void CreateDirectorDictionaries() {
            // Create Director GameObjects Dictionaries
            DirectorData.dictionaryManagerObject = new ConcurrentDictionary<string, GameObject>();
            DirectorData.dictionaryEventHandlerObject = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>Director Dictionaries</b> was successful <color=green><i>created</i></color>.");
        }

        public void ClearDirectorDictionaries() {
            // Clearing any Old Persistent from the Director Dictionaries
            DirectorData.dictionaryManagerObject.Clear();
            DirectorData.dictionaryEventHandlerObject.Clear();
            Debug.Log("<b>Old Persitent Data</b> from <b>Director Dictionaries</b> were successful <color=cyan><i>cleared</i></color> if they had any.");
        }

        public void PopulateDirectorDictionaries() {
            // Setting up
            GameObject @worldManager = GameObject.Find("WorldManager");
            GameObject @visualManager = GameObject.Find("VisualManager");
            GameObject @audioManager = GameObject.Find("AudioManager");
            GameObject @playerManager = GameObject.Find("PlayerManager");
            GameObject @gameTimeManager = GameObject.Find("GameTimeManager");

            GameObject @visualEventHandler = GameObject.Find("VisualEventHandler");

            // Adding Managers to dictionaryManagerObject Dictionary
            bool @tryAddFalseWorldManager = DirectorData.dictionaryManagerObject.TryAdd(@worldManager.name, @worldManager);
            if(!@tryAddFalseWorldManager) {
                Debug.LogError("<b>" + @worldManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool @tryAddFalseVisualManager = DirectorData.dictionaryManagerObject.TryAdd(@visualManager.name, @visualManager);
            if(!@tryAddFalseVisualManager) {
                Debug.LogError("<b>" + @visualManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool @tryAddFalseAudioManager = DirectorData.dictionaryManagerObject.TryAdd(@audioManager.name, @audioManager);
            if(!@tryAddFalseAudioManager) {
                Debug.LogError("<b>" + @audioManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool @tryAddFalsePlayerManager = DirectorData.dictionaryManagerObject.TryAdd(@playerManager.name, @playerManager);
            if(!@tryAddFalsePlayerManager) {
                Debug.LogError("<b>" + @playerManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool @tryAddFalseGameTimeManager = DirectorData.dictionaryManagerObject.TryAdd(@gameTimeManager.name, @gameTimeManager);
            if(!@tryAddFalseGameTimeManager) {
                Debug.LogError("<b>" + @gameTimeManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }

            if(DirectorData.dictionaryManagerObject.Count < 5) {
                Debug.LogWarning("<b>dictionaryManagerObject</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Managers</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryManagerObject</b> with a total of <i>" + DirectorData.dictionaryManagerObject.Count + " Managers</i>.");
            }

            // Adding EventHandlers to dictionaryEventHandlerObject Dictionary
            bool @tryAddFalseVisualEventHandler = DirectorData.dictionaryEventHandlerObject.TryAdd(@visualEventHandler.name, @visualEventHandler);
            if(!@tryAddFalseVisualEventHandler) {
                Debug.LogError("<b>" + @visualEventHandler.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryEventHandlerObject</b>.");
            }

            if(DirectorData.dictionaryEventHandlerObject.Count < 1) {
                Debug.LogWarning("<b>dictionaryEventHandlerObject</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
            } else {
                Debug.Log("<b>EventHandlers</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryEventHandlerObject</b> with a total of <i>" + DirectorData.dictionaryEventHandlerObject.Count + " EventHandlers</i>.");
            }
        }

        public void InitializeEventHandlers() {
            // Initializing EventHandlers
            //var @worldEventHandler = WorldEventHandler.Instance;
            var @visualEventHandler = Visual.VisualEventHandler.Instance;
            //var @audioEventHandler = Audio.AudioEventHandler.Instance;
            //var @playerEventHandler = Player.PlayerEventHandler.Instance;
            //var @gameTimeEventHandler = GameTime.GameTimeEventHandler.Instance;
            Debug.Log("<b>EventHandlers</b> were <color=green><i>initialized</i></color>.");
        }
    }
}