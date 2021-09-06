using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World;
using BlueBird.World.Parameters;

namespace BlueBird.World.Player {
    public sealed class PlayerGenerator {
        public void CreatePlayerDictionaries() {
            // Create Player GameObjects Dictionary
            PlayerData.dictionaryPlayerObject = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>Player Dictionary</b> was successful <color=green><i>created</i></color>.");
        }

        public void ClearPlayerDictionaries() {
            // Clearing any Old Persistent from the PlayerData Dictionaries
            PlayerData.dictionaryPlayerObject.Clear();
            Debug.Log("<b>Old Persitent Data</b> from <b>PlayerData Dictionaries</b> were successful <color=cyan><i>cleared</i></color> if they had any.");
        }

        public void CreatePlayerGameObject() {
            // Create a GameObject for the Character
            GameObject @playerObject = new GameObject("Player") {
                tag = "Player",
                layer = 8
            };
            @playerObject.transform.localPosition = Vector3.zero;
            @playerObject.transform.localScale = Vector3.one;
            @playerObject.transform.localEulerAngles = Vector3.zero;
            bool @tryAddFalse = WorldData.dictionaryMainObject.TryAdd(@playerObject.name, @playerObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>Player Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryMainObject</b>.");
            } else {
                Debug.Log("<b>" + @playerObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryMainObject</i>.");
            }
        }

        public void CatchMainCameraGameObject() {
            // Setting up
            GameObject @playerObject = WorldData.dictionaryMainObject["Player"];

            // Catch the MainCamera of the Scene
            GameObject @mainCameraObject = GameObject.FindWithTag("MainCamera");
            @mainCameraObject.transform.SetParent(@playerObject.transform, true);
            @mainCameraObject.name = "MainCamera";
            @mainCameraObject.tag = "Player";
            @mainCameraObject.layer = 8;
            @mainCameraObject.transform.localPosition = new Vector3(15, 20, -5);
            @mainCameraObject.transform.localScale = Vector3.one;
            @mainCameraObject.transform.localEulerAngles = new Vector3(45, 0, 0);
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@mainCameraObject.name, @mainCameraObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>MainCamera Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @mainCameraObject.name + " GameObject</b> has been <color=green><i>caught</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }
    }
}