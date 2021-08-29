using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Definitions;
using BlueBird.World.Data;
using BlueBird.World.Visual;

namespace BlueBird.World {
    public sealed class WorldGenerator {
        /* Instances */
        Region2D _region2D;
        XMLCreator _xmlCreator;
        XMLLoader _xmlLoader;

        ChunkGenerator _chunkGenerator;

        public void CreateWorldDictionaries() {
            // Create Definitions Data Dictionaries
            WorldData.dictionaryVoxelDefinition = new ConcurrentDictionary<string, VoxelDefinition>();
            Debug.Log("<b>Definitions Data Dictionaries</b> were successful <color=green><i>generated</i></color>.");

            // Create World Data Dictionaries
            WorldData.dictionaryChunks = new ConcurrentDictionary<Vector3, GameObject>();
            Debug.Log("<b>World Data Dictionaries</b> were successful <color=green><i>generated</i></color>.");

            // Clearing any Old Persistent Data from the Dictionaries
            WorldData.dictionaryVoxelDefinition.Clear();
            WorldData.dictionaryChunks.Clear();
            Debug.Log("<b>Old Persitent Data</b> from <b>WorldData Dictionaries</b> were successful <color=cyan><i>cleared</i></color> if they had any.");
        }

        public void CreateWorldGameObject() {
            // Create a GameObject for the World
            GameObject @worldObject = new GameObject("WorldMap", typeof(World)) {
                tag = "World",
                layer = 9,
            };
            @worldObject.transform.localPosition = Vector3.zero;
            @worldObject.transform.localScale = Vector3.one;
            @worldObject.transform.localEulerAngles = Vector3.zero;
            Debug.Log("<b>" + @worldObject.name + " GameObject</b> was <color=green><i>created</i></color>.");

            // Create a Region2D for the World
            Region2D @worldRegion = new Region2D(Constants_str.worldBaseVector3, Settings_str.worldSizeChunks, Constants_str.chunkWidth);
            World @world = @worldObject.GetComponent<World>();
            @world._region2D = @worldRegion;
            Debug.Log("<b>World Region2D</b> was successful <color=green><i>generated</i></color>. Size: <i>" + @worldRegion.regionSize + "</i>.");
        }

        public void LoadDefinitionsDictionaries() {
            // Setting up
            _xmlCreator = new XMLCreator(); // Used to create base XML files
            _xmlLoader = new XMLLoader();

            // Generate Definitions Data Dictionaries
            //_xmlCreator.SerializeVoxelDefinition(); // Used to create base XML files

            // Load Definitions Data from XML files
            _xmlLoader.DeserializeVoxelDefinition();
            Debug.Log("<b>World Data Dictionaries</b> were successful <color=cyan><i>populated</i></color>.");
        }

        public void GenerateStaticEntities() {
            // Create Chunks GameObjects, Datas, Voxels and Meshes
            _chunkGenerator = new ChunkGenerator();
            _chunkGenerator.CreateChunks(Constants_str.worldBaseVector3, Settings_str.worldSizeChunks, Constants_str.chunkWidth);
        }
    }
}