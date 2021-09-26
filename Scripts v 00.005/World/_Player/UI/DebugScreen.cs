using UnityEngine;
using TMPro;
using BlueBird.World.WorldMap.Topography.Chunks;

namespace BlueBird.World.Player.UI {
    public class DebugScreen : MonoBehaviour {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();

        /* Instances - For Debug Screen */
        Transform characterTransform;
        TMP_Text debugMessages;

        /* Variables -  For Debug Screen */
        private float frameRate;
        private Vector3 characterChunkPosition;

        private void OnEnable() {
            PlayerEventHandler.eventOnPlayerTickMultipleOf_4 += GetFrameRate;
            PlayerEventHandler.eventOnPlayerTickMultipleOf_4 += CheckForCharacterChunkPosition;
            PlayerEventHandler.eventOnPlayerTickMultipleOf_4 += UpdateDebugMessage;
        }

        private void Start() {
            characterTransform = PlayerData.dictionaryPlayerObject["Character"].transform;
            debugMessages = GetComponent<TextMeshProUGUI>();

            // Run Once to Update the DebugMessage
            GetFrameRate();
            CheckForCharacterChunkPosition();
            UpdateDebugMessage();
        }

        private void OnDisable() {
            PlayerEventHandler.eventOnPlayerTickMultipleOf_4 -= GetFrameRate;
            PlayerEventHandler.eventOnPlayerTickMultipleOf_4 -= CheckForCharacterChunkPosition;
            PlayerEventHandler.eventOnPlayerTickMultipleOf_4 -= UpdateDebugMessage;
        }

        private void GetFrameRate() {
            frameRate = (int)(1f / Time.unscaledDeltaTime);
        }

        private void CheckForCharacterChunkPosition() {
            // Check and Update current Player ChunkPosition if had any changes
            characterChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(characterTransform.position);
        }

        private void UpdateDebugMessage() {
            string debugText = "Blue Bird Project™";
            debugText += "\n";
            debugText += frameRate + " fps";
            debugText += "\n\n";
            debugText += "Position (" + (Mathf.Round(characterTransform.position.x * 1000f) / 1000f) + ", " + (Mathf.Round(characterTransform.position.y * 1000f) / 1000f) + ", " + (Mathf.Round(characterTransform.position.z * 1000f) / 1000f) + ")";
            debugText += "\n";
            debugText += "Chunk (" + characterChunkPosition.x + ", " + characterChunkPosition.z + ")";

            debugMessages.text = debugText;
        }
    }
}