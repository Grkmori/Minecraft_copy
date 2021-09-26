using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BlueBird.World.Definitions {
    [XmlRoot("VoxelsDefinitions")]
    [XmlInclude(typeof(VoxelDefinition))]
    public sealed class VoxelDefinitionContainer {
        [XmlArray("Voxels"), XmlArrayItem("Voxel")]
        public List<VoxelDefinition> listVoxelsDefinitions = new List<VoxelDefinition>();
    }

    [XmlType("Voxel")]
    public sealed class VoxelDefinition {
        /* Variables */
        public string voxelType;
        public string description;

        public bool isSolid;
        public bool isTransparent;
        public bool hasFertility;

        /* Variables - For Pathfinding (will be copied to Voxel) */
        public bool isWalkable;
        public float baseMovementCost;

        /* Variables - Faces Textures */
        public Vector2 backFaceTexture;
        public Vector2 frontFaceTexture;
        public Vector2 topFaceTexture;
        public Vector2 bottomFaceTexture;
        public Vector2 leftFaceTexture;
        public Vector2 rightFaceTexture;
    }
}