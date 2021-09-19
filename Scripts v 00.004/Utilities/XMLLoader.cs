using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using BlueBird.World.Definitions;

namespace BlueBird.World.Utilities {
    public sealed class XMLLoader {
        /* Variables */
        private readonly string xmlDataFolder = Application.dataPath + "/StreamingAssets/Data";

        public void DeserializeVoxelDefinition() {
            // Setting up
            VoxelDefinitionContainer voxelDefinitionContainer = new VoxelDefinitionContainer();
            VoxelDefinition voxelDefinition = new VoxelDefinition();
            DirectoryInfo rootFolder = new DirectoryInfo(xmlDataFolder);

            // Deserialize VoxelDefinition
            XmlSerializer deserializerVoxelDefinition = new XmlSerializer(typeof(VoxelDefinitionContainer));
            deserializerVoxelDefinition.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
            deserializerVoxelDefinition.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);
            FileStream streamVoxelDefinition = new FileStream(rootFolder + "/Voxels.xml", FileMode.Open);
            voxelDefinitionContainer = deserializerVoxelDefinition.Deserialize(streamVoxelDefinition) as VoxelDefinitionContainer;
            streamVoxelDefinition.Close();

            // Populating dictionaryVoxelDefinition with the values Deserialized
            for(int index = 0; index < voxelDefinitionContainer.listVoxelsDefinitions.Count; index++) {
                string voxelType = voxelDefinitionContainer.listVoxelsDefinitions[index].voxelType;
                bool tryAddFalse = WorldData.dictionaryVoxelDefinition.TryAdd(voxelType, voxelDefinitionContainer.listVoxelsDefinitions[index]);
                if(!tryAddFalse) {
                    Debug.LogError("<b>VoxelDefinition</b> <color=red><i>failed to add</i></color> to <b>dictionaryVoxelDefinition</b>.");
                }
            }
            if(WorldData.dictionaryVoxelDefinition.Count < voxelDefinitionContainer.listVoxelsDefinitions.Count) {
                Debug.LogWarning("<b>dictionaryVoxelDefinition</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
            } else {
                Debug.Log("<b>dictionaryVoxelDefinition</b> was <color=green><i>generated</i></color> and <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryVoxelDefinition.Count + " VoxelDefinitions</i>.");
            }
        }

        public void DeserializeBiomeDefinition() {
            // Setting up
            BiomeDefinitionContainer biomeDefinitionContainer = new BiomeDefinitionContainer();
            BiomeDefinition biomeDefinition = new BiomeDefinition();
            DirectoryInfo rootFolder = new DirectoryInfo(xmlDataFolder);

            // Deserialize BiomeDefinition
            XmlSerializer deserializerBiomeDefinition = new XmlSerializer(typeof(BiomeDefinitionContainer));
            deserializerBiomeDefinition.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
            deserializerBiomeDefinition.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);
            FileStream streamBiomeDefinition = new FileStream(rootFolder + "/Biomes.xml", FileMode.Open);
            biomeDefinitionContainer = deserializerBiomeDefinition.Deserialize(streamBiomeDefinition) as BiomeDefinitionContainer;
            streamBiomeDefinition.Close();

            // Populating dictionaryBiomeDefinition with the values Deserialized
            for(int index = 0; index < biomeDefinitionContainer.listBiomesDefinitions.Count; index++) {
                string biomeType = biomeDefinitionContainer.listBiomesDefinitions[index].biomeTypeName;
                bool tryAddFalse = WorldData.dictionaryBiomeDefinition.TryAdd(biomeType, biomeDefinitionContainer.listBiomesDefinitions[index]);
                if(!tryAddFalse) {
                    Debug.LogError("<b>BiomeDefinition</b> <color=red><i>failed to add</i></color> to <b>dictionaryBiomeDefinition</b>.");
                }
            }
            if(WorldData.dictionaryBiomeDefinition.Count < biomeDefinitionContainer.listBiomesDefinitions.Count) {
                Debug.LogWarning("<b>dictionaryBiomeDefinition</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
            } else {
                Debug.Log("<b>dictionaryBiomeDefinition</b> was <color=green><i>generated</i></color> and <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryBiomeDefinition.Count + " BiomeDefinitions</i>.");
            }
        }

        public void DeserializeBiomeLodeDefinition() {
            // Setting up
            BiomeLodeDefinitionContainer biomeLodeDefinitionContainer = new BiomeLodeDefinitionContainer();
            BiomeLodeDefinition biomeLodeDefinition = new BiomeLodeDefinition();
            DirectoryInfo rootFolder = new DirectoryInfo(xmlDataFolder);

            // Deserialize BiomeLodeDefinition
            XmlSerializer deserializerBiomeLodeDefinition = new XmlSerializer(typeof(BiomeLodeDefinitionContainer));
            deserializerBiomeLodeDefinition.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
            deserializerBiomeLodeDefinition.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);
            FileStream streamBiomeLodeDefinition = new FileStream(rootFolder + "/BiomeLodes.xml", FileMode.Open);
            biomeLodeDefinitionContainer = deserializerBiomeLodeDefinition.Deserialize(streamBiomeLodeDefinition) as BiomeLodeDefinitionContainer;
            streamBiomeLodeDefinition.Close();

            // Populating dictionaryBiomeLodeDefinition with the values Deserialized
            for(int index = 0; index < biomeLodeDefinitionContainer.listBiomeLodesDefinitions.Count; index++) {
                string biomeLodeType = biomeLodeDefinitionContainer.listBiomeLodesDefinitions[index].biomeLodeTypeName;
                bool tryAddFalse = WorldData.dictionaryBiomeLodeDefinition.TryAdd(biomeLodeType, biomeLodeDefinitionContainer.listBiomeLodesDefinitions[index]);
                if(!tryAddFalse) {
                    Debug.LogError("<b>BiomeLodeDefinition</b> <color=red><i>failed to add</i></color> to <b>dictionaryBiomeLodeDefinition</b>.");
                }
            }
            if(WorldData.dictionaryBiomeLodeDefinition.Count < biomeLodeDefinitionContainer.listBiomeLodesDefinitions.Count) {
                Debug.LogWarning("<b>dictionaryBiomeLodeDefinition</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
            } else {
                Debug.Log("<b>dictionaryBiomeLodeDefinition</b> was <color=green><i>generated</i></color> and <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryBiomeLodeDefinition.Count + " BiomeLodeDefinitions</i>.");
            }
        }

        //  If the XML document has been altered with unknown nodes or attributes, handle them with the UnknownNode and UnknownAttribute events.
        private void Serializer_UnknownNode(object sender, XmlNodeEventArgs e) {
            Debug.LogWarning("<color=yellow><b>Unknown Node: " + e.Name + "</b></color> was found when <i>deserializing</i>.");
        }
        private void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e) {
            Debug.LogWarning("<color=yellow><b>Unknown Attribute: " + e.Attr.Name + "</b></color> was found when <i>deserializing</i>.");
        }
    }
}

//var listToDictionary = entriesVoxelDefinition.Distinct().ToDictionary(x => x.groundType, x => x);

//public static string XmlSerialize<T>(T entity) where T : class {
//    // removes version
//    XmlWriterSettings settings = new XmlWriterSettings();
//    settings.OmitXmlDeclaration = true;
//    XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
//    using(StringWriter sw = new StringWriter())
//    using(XmlWriter writer = XmlWriter.Create(sw, settings)) {
//        // removes namespace
//        var xmlns = new XmlSerializerNamespaces();
//        xmlns.Add(string.Empty, string.Empty);
//        xsSubmit.Serialize(writer, entity, xmlns);
//        return sw.ToString(); // Your XML
//    }
//}