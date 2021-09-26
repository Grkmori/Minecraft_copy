using UnityEngine;
using System.Collections.Concurrent;

namespace BlueBird.World {
    public sealed class DirectorAssistant {
        public void SetGameLoadingOptions() {
            // Set/ReSet Checkers to False
            WorldDirector.checkerWorldMapStart = false;
            WorldDirector.checkerVisualStart = false;
            WorldDirector.checkerAudioStart = false;
            WorldDirector.checkerPlayerStart = false;
            WorldDirector.checkerEntityStart = false;
            WorldDirector.checkerGameTimeStart = false;

            //WorldDirector.checkerWorldMapEventStart = false;
            WorldDirector.checkerVisualEventStart = false;
            //WorldDirector.checkerAudioEventStart = false;
            WorldDirector.checkerPlayerEventStart = false;
            WorldDirector.checkerPlayerInputStart = false;
            WorldDirector.checkerEntityEventStart = false;
        }

        public void CreateDirectorDictionaries() {
            // Create Director GameObjects Dictionaries
            WorldData.dictionaryManagerObject = new ConcurrentDictionary<string, GameObject>();
            WorldData.dictionaryHandlerObject = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>Director Dictionaries</b> was successfully <color=green><i>created</i></color>.");
        }

        public void ClearDirectorDictionaries() {
            // Clearing any Old Persistent from the Director Dictionaries
            WorldData.dictionaryManagerObject.Clear();
            WorldData.dictionaryHandlerObject.Clear();
            Debug.Log("<b>Old Persistent Data</b> from <b>Director Dictionaries</b> were successfully <color=cyan><i>cleared</i></color> if they had any.");
        }

        public void PopulateDirectorDictionaries() {
            // Setting up
            GameObject worldMapManager = GameObject.Find("WorldMapManager");
            GameObject visualManager = GameObject.Find("VisualManager");
            GameObject audioManager = GameObject.Find("AudioManager");
            GameObject playerManager = GameObject.Find("PlayerManager");
            GameObject entityManager = GameObject.Find("EntityManager");
            GameObject gameTimeManager = GameObject.Find("GameTimeManager");

            //GameObject worldMapEventHandler = GameObject.Find("WorldMapEventHandler");
            GameObject visualEventHandler = GameObject.Find("VisualEventHandler");
            //GameObject audioEventHandler = GameObject.Find("AudioEventHandler");
            GameObject playerEventHandler = GameObject.Find("PlayerEventHandler");
            GameObject playerInputHandler = GameObject.Find("PlayerInputHandler");
            GameObject entityEventHandler = GameObject.Find("EntityEventHandler");

            // Adding Managers to dictionaryManagerObject Dictionary
            bool tryAddFalseWorldMapManager = WorldData.dictionaryManagerObject.TryAdd(worldMapManager.name, worldMapManager);
            if(!tryAddFalseWorldMapManager) {
                Debug.LogError("<b>" + worldMapManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool tryAddFalseVisualManager = WorldData.dictionaryManagerObject.TryAdd(visualManager.name, visualManager);
            if(!tryAddFalseVisualManager) {
                Debug.LogError("<b>" + visualManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool tryAddFalseAudioManager = WorldData.dictionaryManagerObject.TryAdd(audioManager.name, audioManager);
            if(!tryAddFalseAudioManager) {
                Debug.LogError("<b>" + audioManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool tryAddFalsePlayerManager = WorldData.dictionaryManagerObject.TryAdd(playerManager.name, playerManager);
            if(!tryAddFalsePlayerManager) {
                Debug.LogError("<b>" + playerManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool tryAddFalseEntityManager = WorldData.dictionaryManagerObject.TryAdd(entityManager.name, entityManager);
            if(!tryAddFalseEntityManager) {
                Debug.LogError("<b>" + entityManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }
            bool tryAddFalseGameTimeManager = WorldData.dictionaryManagerObject.TryAdd(gameTimeManager.name, gameTimeManager);
            if(!tryAddFalseGameTimeManager) {
                Debug.LogError("<b>" + gameTimeManager.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryManagerObject</b>.");
            }

            if(WorldData.dictionaryManagerObject.Count < 5) {
                Debug.LogWarning("<b>dictionaryManagerObject</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Managers</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryManagerObject</b> with a total of <i>" + WorldData.dictionaryManagerObject.Count + " Managers</i>.");
            }

            // Adding EventHandlers to dictionaryEventHandlerObject Dictionary
            //bool tryAddFalseWorldMapEventHandler = WorldData.dictionaryHandlerObject.TryAdd(worldMapEventHandler.name, worldMapEventHandler);
            //if(!tryAddFalseWorldMapEventHandler) {
            //    Debug.LogError("<b>" + worldMapEventHandler.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryHandlerObject</b>.");
            //}
            bool tryAddFalseVisualEventHandler = WorldData.dictionaryHandlerObject.TryAdd(visualEventHandler.name, visualEventHandler);
            if(!tryAddFalseVisualEventHandler) {
                Debug.LogError("<b>" + visualEventHandler.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryHandlerObject</b>.");
            }
            //bool tryAddFalseAudioEventHandler = WorldData.dictionaryHandlerObject.TryAdd(audioEventHandler.name, audioEventHandler);
            //if(!tryAddFalseAudioEventHandler) {
            //    Debug.LogError("<b>" + audioEventHandler.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryHandlerObject</b>.");
            //}
            bool tryAddFalsePlayerEventHandler = WorldData.dictionaryHandlerObject.TryAdd(playerEventHandler.name, playerEventHandler);
            if(!tryAddFalsePlayerEventHandler) {
                Debug.LogError("<b>" + playerEventHandler.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryHandlerObject</b>.");
            }
            bool tryAddFalsePlayerInputHandler = WorldData.dictionaryHandlerObject.TryAdd(playerInputHandler.name, playerInputHandler);
            if(!tryAddFalsePlayerInputHandler) {
                Debug.LogError("<b>" + playerInputHandler.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryHandlerObject</b>.");
            }
            bool tryAddFalseEntityEventHandler = WorldData.dictionaryHandlerObject.TryAdd(entityEventHandler.name, entityEventHandler);
            if(!tryAddFalseEntityEventHandler) {
                Debug.LogError("<b>" + entityEventHandler.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryHandlerObject</b>.");
            }

            if(WorldData.dictionaryHandlerObject.Count < 4) {
                Debug.LogWarning("<b>dictionaryHandlerObject</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Handlers</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryHandlerObject</b> with a total of <i>" + WorldData.dictionaryHandlerObject.Count + " Handlers</i>.");
            }
        }

        public void InitializeHandlers() {
            // Initializing EventHandlers
            //var worldEventHandler = WorldEventHandler.Instance;
            var visualEventHandler = Visual.VisualEventHandler.Instance;
            //var audioEventHandler = Audio.AudioEventHandler.Instance;
            var playerEventHandler = Player.PlayerEventHandler.Instance;
            var playerInputHandler = Player.PlayerInputHandler.Instance;
            var entityEventHandler = Entity.EntityEventHandler.Instance;
            Debug.Log("<b>Handlers</b> were <color=green><i>initialized</i></color>.");
        }
    }
}