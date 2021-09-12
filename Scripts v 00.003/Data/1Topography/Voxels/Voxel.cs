using UnityEngine;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class Voxel {
        /* Variables - For Constructor */
        public Vector3 voxelPosition { get; internal set; }
        public string voxelTypeName { get; internal set; }

        public Voxel() {
            this.voxelPosition = voxelPosition;
            this.voxelTypeName = voxelTypeName;
        }
    }
}