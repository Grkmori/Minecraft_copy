using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Data.InvisibleData.Chunks {
    public sealed class ChunkUtilities {
        /* Variables - For  Chunks */
        private readonly float chunkPosY = Constants_str.worldBaseVector3.y;
        private readonly float chunkWidth = Constants_str.chunkSize.x;
        private readonly float chunkCorrection = 0.01f;

        public Vector3 GetChunkPositionFromPosition(Vector3 @position) {
            float x = Mathf.RoundToInt((@position.x + chunkCorrection) / chunkWidth);
            float y = chunkPosY;
            float z = Mathf.RoundToInt((@position.z + chunkCorrection) / chunkWidth);
            return new Vector3(x, y, z);
        }
    }
}
