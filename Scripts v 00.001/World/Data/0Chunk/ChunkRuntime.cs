using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Data {
    public sealed class ChunkRuntime {
        /* Variables - For Voxels */
        private Vector3 baseVector = Constants_str.worldBaseVector3;
        private float maxWidth = Constants_str.chunkWidth;
        private float maxHeight = Constants_str.chunkHeight;
        private float addWidth = Constants_str.voxelWidth;
        private float addHeight = Constants_str.voxelHeight;

        private bool VoxelInChunk(Vector3 @position) {
            if(@position.x < baseVector.x || @position.x > maxWidth ||
               @position.y < baseVector.y || @position.y > maxHeight ||
               @position.z < baseVector.z || @position.z > maxWidth) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}