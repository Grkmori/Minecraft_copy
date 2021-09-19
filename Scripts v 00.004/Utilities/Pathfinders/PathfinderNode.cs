using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Utilities.Pathfinders {
    public sealed class PathfinderNode {
        /* Instances */
        public Voxel voxelReference { get; internal set; }
        public PathfinderNode fromNode { get; internal set; }

        /* Variables - For Constructor */
        public Vector3 nodePosition { get; internal set; }
        public bool isSearched { get; internal set; }

        /* Variables - For Pathfinder */
        public float gCost { get; internal set; } // Walking Cost from the Starting Node
        public float hCost { get; internal set; } // Walking Cost to reach the End Node
        public float fCost { get; internal set; } // gCost + hCost
        public float voxelMovementCost { get; internal set; } // Movement Cost in the 'Voxel'

        public PathfinderNode() {
            this.voxelReference = voxelReference;
            this.nodePosition = nodePosition;
            this.isSearched = isSearched;
            this.fromNode = fromNode;

            this.voxelMovementCost = voxelMovementCost;
            this.gCost = gCost;
            this.hCost = hCost;
            this.fCost = fCost;
        }

        public void CalculateFCost() {
            fCost = gCost + hCost;
        }
    }
}