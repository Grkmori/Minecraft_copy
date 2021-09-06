using UnityEngine;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterGenerator {
        public void CreateCharacterGameObject(Vector3 @spawnPosition) {
            // Setting up
            GameObject @playerObject = WorldData.dictionaryMainObject["Player"];

            // Create a GameObject for the Character
            GameObject @characterObject = new GameObject("Character") {
                tag = "Player",
                layer = 8
            };
            @characterObject.transform.SetParent(@playerObject.transform, false);
            @characterObject.transform.localPosition = @spawnPosition;
            @characterObject.transform.localScale = Vector3.one;
            @characterObject.transform.localEulerAngles = Vector3.zero;
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@characterObject.name, @characterObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>Character Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @characterObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }
        }

        public void CreateCharacterCameraGameObject(float @cameraHeight) {
            // Setting up
            GameObject @characterObject = PlayerData.dictionaryPlayerObject["Character"];

            // Create a GameObject for the Character
            GameObject @characterCameraObject = new GameObject("CharacterCamera", typeof(Camera), typeof(CharacterCameraBehaviour)) {
                tag = "Player",
                layer = 8
            };
            @characterCameraObject.transform.SetParent(@characterObject.transform, false);
            @characterCameraObject.transform.localPosition = new Vector3(0, @cameraHeight, 0);
            @characterCameraObject.transform.localScale = Vector3.one;
            @characterCameraObject.transform.localEulerAngles = Vector3.zero;
            bool @tryAddFalse = PlayerData.dictionaryPlayerObject.TryAdd(@characterCameraObject.name, @characterCameraObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>CharacterCamera Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerObject</b>.");
            } else {
                Debug.Log("<b>" + @characterCameraObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryPlayerObject</i>.");
            }

            // Setting up this Camera

        }

        public void SetupCharacterGameObject() {
            // Setting up Character GameObject
            GameObject @mainCameraObject = PlayerData.dictionaryPlayerObject["MainCamera"];
            GameObject @characterObject = PlayerData.dictionaryPlayerObject["Character"];
            GameObject @characterCameraObject = PlayerData.dictionaryPlayerObject["CharacterCamera"];

            @characterObject.AddComponent(typeof(CharacterBehaviour));
            @mainCameraObject.SetActive(false);
            @characterCameraObject.SetActive(true);
        }
    }
}