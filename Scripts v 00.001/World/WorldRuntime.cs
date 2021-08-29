using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World {
    public sealed class WorldRuntime {
        private Vector2 maxSizeChunks = Settings_str.worldSizeChunks;

        private bool ChunkInWorld(Vector3 @position) {
            if(@position.x >= 0 && @position.x <= maxSizeChunks.x && @position.z >= 0 && @position.z <= maxSizeChunks.y) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}