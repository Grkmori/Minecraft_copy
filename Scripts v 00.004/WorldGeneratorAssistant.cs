using UnityEngine;

namespace BlueBird.World {
    public sealed class WorldGeneratorAssistant : MonoBehaviour {
        private void OnDisable() {
            Destroy(gameObject);
        }

        public GameObject InstantiateChunkObject(GameObject chunkPrefab, Transform worldTransform, Vector3 position, Vector3 baseVector, Vector2 chunkSize) {
            GameObject chunkObject = Instantiate(chunkPrefab, new Vector3(position.x * chunkSize.x, baseVector.y, position.z * chunkSize.x), Quaternion.identity, worldTransform);
            return chunkObject;
        }
    }
}