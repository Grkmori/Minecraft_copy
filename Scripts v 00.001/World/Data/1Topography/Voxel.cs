using UnityEngine;

namespace BlueBird.World.Data {
    public sealed class Voxel {
        /* Variables - For Constructor */
        public Vector3 position { get; set; }
        public string voxelTypeName { get; set; }

        public Voxel() {
            this.position = position;
            this.voxelTypeName = voxelTypeName;        }
    }
}