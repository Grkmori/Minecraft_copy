using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BlueBird.World.Definitions {
    [XmlRoot("BiomesDefinitions")]
    [XmlInclude(typeof(BiomeDefinition))]
    public sealed class BiomeDefinitionContainer {
        [XmlArray("Biomes"), XmlArrayItem("Biome")]
        public List<BiomeDefinition> listBiomesDefinitions = new List<BiomeDefinition>();
    }

    [XmlType("Biome")]
    public sealed class BiomeDefinition {
        /* Variables */
        public string biomeTypeName;
        public int solidGroundHeight;
        public int terrainHeight;
        public float terrainScale;

        [XmlArray("BiomeLodes"), XmlArrayItem("BiomeLodeType")]
        public string[] lode;
    }
}