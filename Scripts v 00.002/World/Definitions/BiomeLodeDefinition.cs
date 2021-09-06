using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace BlueBird.World.Definitions {
    [XmlRoot("BiomeLodesDefinitions")]
    [XmlInclude(typeof(BiomeLodeDefinition))]
    public sealed class BiomeLodeDefinitionContainer {
        [XmlArray("BiomeLodes"), XmlArrayItem("BiomeLode")]
        public List<BiomeLodeDefinition> listBiomeLodesDefinitions = new List<BiomeLodeDefinition>();
    }

    [XmlType("BiomeLode")]
    public sealed class BiomeLodeDefinition {
        public string biomeLodeTypeName;
        public string blockType;
        public int minHeight;
        public int maxHeight;
        public float scale;
        public float threshold;
    }
}
