using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.WorldMap.Topography.Chunks;

namespace BlueBird.World.WorldMap.Topography.Voxels {
    public sealed class Voxel {
        /* Instances - For Constructor */
        public Chunk _chunk { get; internal set; }

        /* Variables - For Constructor */
        public Vector3 voxelPosition { get; internal set; }
        public string voxelTypeName { get; internal set; }
        public bool isSolid { get; internal set; } // Needs to be Updated when Voxel is Updated
        public bool isTransparent { get; internal set; } // Needs to be Updated when Voxel is Updated

        /* Storage - For PathFinding */
        public List<Voxel> listVoxelNeighbours { get; internal set; } // Needs to be Updated when Voxel or Neighbours are Updated

        /* Variables - For PathFinding */
        public bool isWalkable { get; set; } // Needs to be Updated when Voxel is Updated
        public float voxelMovementCost { get; set; }
        public float baseMovementCost { get; set; } // Needs to be Updated when Voxel is Updated
        public float modifiersMovementCost { get; set; }

        public Voxel() {
            this._chunk = _chunk;

            this.voxelPosition = voxelPosition;
            this.voxelTypeName = voxelTypeName;
            this.isSolid = isSolid;
            this.isTransparent = isTransparent;

            this.listVoxelNeighbours = new List<Voxel>();
            this.isWalkable = isWalkable;
            this.voxelMovementCost = voxelMovementCost;
            this.baseMovementCost = baseMovementCost;
            this.modifiersMovementCost = modifiersMovementCost;
        }
    }
}