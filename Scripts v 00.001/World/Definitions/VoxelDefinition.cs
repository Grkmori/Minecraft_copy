using UnityEngine;
using System.Collections.Generic;
using System.Xml;
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
        /* Variables - Properties */
        public string voxelType;
        public string description;
        public bool isSolid;

        /* Variables - Faces Textures */
        public Vector2 backFaceTexture;
        public Vector2 frontFaceTexture;
        public Vector2 topFaceTexture;
        public Vector2 bottomFaceTexture;
        public Vector2 leftFaceTexture;
        public Vector2 rightFaceTexture;

        /* Variables - Properties for GraphicInstance */
        public string materialName;
        public string textureName;
        public string colorPallet;
        public Vector2 graphicPivot;
        public Vector3 graphicPosition;
        public Vector3 graphicScale;
        public Vector3 graphicEulerAngles;
        public float drawPriority;
        public bool isInstanced;
    }
}