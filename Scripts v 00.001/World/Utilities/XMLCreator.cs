using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BlueBird.World.Definitions;

namespace BlueBird.World.Utilities {
    public sealed class XMLCreator {
        /* Instances */
        VoxelDefinitionContainer _voxelDefinitionContainer;
        VoxelDefinition _voxelDefintion;

        /* Variables */
        private string xmlDataFolder = Application.dataPath + "/StreamingAssets/Data";

        // Create new XML files. This will be used to facilitated the handling of new Data Definition Dictionaries
        public void SerializeVoxelDefinition() {
            // Setting up
            VoxelDefinitionContainer @voxelDefinitionContainer = new VoxelDefinitionContainer();
            VoxelDefinition @voxelDefinition = new VoxelDefinition();
            @voxelDefinitionContainer.listVoxelsDefinitions.Add(new VoxelDefinition());
            DirectoryInfo rootFolder = new DirectoryInfo(xmlDataFolder);

            // Serialize VoxelDefinition
            XmlSerializerNamespaces @nameSpace = new XmlSerializerNamespaces();
            @nameSpace.Add(string.Empty, string.Empty); // Hide 'default' namespaces for the XML output
            XmlSerializer @serializer = new XmlSerializer(typeof(VoxelDefinitionContainer));
            FileStream @stream = new FileStream(rootFolder + "/Voxels.xml", FileMode.Create);
            @serializer.Serialize(@stream, @voxelDefinitionContainer, @nameSpace); // Add @nameSpace "(@stream, _voxelDefinition, @nameSpace)"
            @stream.Close();
            Debug.Log("<b>VoxelDefintion XMLfile</b> was successful <color=green><i>created</i></color>.");
        }
    }
}