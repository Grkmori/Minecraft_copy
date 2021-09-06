using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelUtilities {
        /* Variables - For  Voxels */
        private readonly float chunkPosY = Constants_str.worldBaseVector3.y;

        public Vector3 GetVoxelPositionFromPosition(Vector3 @position) {
            float x = Mathf.RoundToInt(@position.x);
            float y = Mathf.RoundToInt(@position.y);
            float z = Mathf.RoundToInt(@position.z);
            return new Vector3(x, y, z);
        }
    }
}