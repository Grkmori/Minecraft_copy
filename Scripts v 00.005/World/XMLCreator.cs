using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using BlueBird.World.Definitions;

namespace BlueBird.World {
    public sealed class XMLCreator {
        /* Variables */
        private readonly string xmlDataFolder = Application.dataPath + "/StreamingAssets/Data";

        // Create new XML files. This will be used to facilitated the handling of new Data Definition Dictionaries
        public void SerializeVoxelDefinition() {
            // Setting up
            VoxelDefinitionContainer voxelDefinitionContainer = new VoxelDefinitionContainer();
            VoxelDefinition voxelDefinition = new VoxelDefinition();
            voxelDefinitionContainer.listVoxelsDefinitions.Add(new VoxelDefinition());
            DirectoryInfo rootFolder = new DirectoryInfo(xmlDataFolder);

            // Serialize VoxelDefinition
            XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty); // Hide 'default' namespaces for the XML output
            XmlSerializer serializer = new XmlSerializer(typeof(VoxelDefinitionContainer));
            FileStream stream = new FileStream(rootFolder + "/Voxels.xml", FileMode.Create);
            serializer.Serialize(stream, voxelDefinitionContainer, nameSpace);
            stream.Close();
            Debug.Log("<b>VoxelDefintion XMLfile</b> was successfully <color=green><i>created</i></color>.");
        }

        public void SerializeBiomeDefinition() {
            // Setting up
            BiomeDefinitionContainer biomeDefinitionContainer = new BiomeDefinitionContainer();
            BiomeDefinition biomeDefinition = new BiomeDefinition();
            biomeDefinitionContainer.listBiomesDefinitions.Add(new BiomeDefinition());
            DirectoryInfo rootFolder = new DirectoryInfo(xmlDataFolder);

            // Serialize BiomeDefinition
            XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty); // Hide 'default' namespaces for the XML output
            XmlSerializer serializer = new XmlSerializer(typeof(BiomeDefinitionContainer));
            FileStream stream = new FileStream(rootFolder + "/Biomes.xml", FileMode.Create);
            serializer.Serialize(stream, biomeDefinitionContainer, nameSpace);
            stream.Close();
            Debug.Log("<b>BiomeDefintion XMLfile</b> was successfully <color=green><i>created</i></color>.");
        }

        public void SerializeBiomeLodeDefinition() {
            // Setting up
            BiomeLodeDefinitionContainer biomeLodeDefinitionContainer = new BiomeLodeDefinitionContainer();
            BiomeLodeDefinition biomeLodeDefinition = new BiomeLodeDefinition();
            biomeLodeDefinitionContainer.listBiomeLodesDefinitions.Add(new BiomeLodeDefinition());
            DirectoryInfo rootFolder = new DirectoryInfo(xmlDataFolder);

            // Serialize BiomeLodeDefinition
            XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
            nameSpace.Add(string.Empty, string.Empty); // Hide 'default' namespaces for the XML output
            XmlSerializer serializer = new XmlSerializer(typeof(BiomeLodeDefinitionContainer));
            FileStream stream = new FileStream(rootFolder + "/BiomeLodes.xml", FileMode.Create);
            serializer.Serialize(stream, biomeLodeDefinitionContainer, nameSpace);
            stream.Close();
            Debug.Log("<b>BiomeLodeDefintion XMLfile</b> was successfully <color=green><i>created</i></color>.");
        }
    }
}