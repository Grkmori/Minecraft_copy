using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Definitions;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;
using BlueBird.World.Data.Topography.Noises;

namespace BlueBird.World {
    public sealed class WorldGenerator {
        /* Instances */
        XMLCreator _xmlCreator;
        XMLLoader _xmlLoader;

        ChunkGenerator _chunkGenerator = new ChunkGenerator();
        NoiseGenerator _noiseGenerator = new NoiseGenerator();

        /* Variables - For World/Chunks */
        private readonly Vector3 baseVector = Constants_str.worldBaseVector3;
        private readonly Vector2 worldSize = Settings_str.worldSizeChunks;
        private readonly Vector2 chunkSize = Constants_str.chunkSize;
        private readonly Vector2 voxelSize = Constants_str.voxelSize;

        public void CreateWorldDictionaries() {
            // Create Main GameObjects Dictionary
            WorldData.dictionaryMainObject = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>World Main Dictionary</b> was successful <color=green><i>created</i></color>.");

            // Create Definitions Data Dictionaries
            WorldData.dictionaryVoxelDefinition = new ConcurrentDictionary<string, VoxelDefinition>();
            WorldData.dictionaryBiomeDefinition = new ConcurrentDictionary<string, BiomeDefinition>();
            WorldData.dictionaryBiomeLodeDefinition = new ConcurrentDictionary<string, BiomeLodeDefinition>();
            Debug.Log("<b>Definitions Data Dictionaries</b> were successful <color=green><i>created</i></color>.");

            // Create World GameObjects Dictionaries
            WorldData.dictionaryChunkObject = new ConcurrentDictionary<Vector3, GameObject>();
            Debug.Log("<b>World GameObjects Dictionaries</b> were successful <color=green><i>created</i></color>.");

            // Create World Data Dictionaries
            WorldData.dictionaryChunkData = new ConcurrentDictionary<Vector3, Chunk>();
            Debug.Log("<b>World Data Dictionaries</b> were successful <color=green><i>created</i></color>.");
        }

        public void ClearWorldDictionaries() {
            // Clearing any Old Persistent from the WorldData Dictionaries
            WorldData.dictionaryMainObject.Clear();

            WorldData.dictionaryVoxelDefinition.Clear();
            WorldData.dictionaryBiomeDefinition.Clear();
            WorldData.dictionaryBiomeLodeDefinition.Clear();

            WorldData.dictionaryChunkObject.Clear();

            WorldData.dictionaryChunkData.Clear();
            Debug.Log("<b>Old Persitent Data</b> from <b>WorldData Dictionaries</b> were successful <color=cyan><i>cleared</i></color> if they had any.");
        }

        public void LoadDefinitionsDictionaries() {
            // Setting up
            _xmlCreator = new XMLCreator();
            _xmlLoader = new XMLLoader();

            // Generate Definitions Data Dictionaries
            //_xmlCreator.SerializeVoxelDefinition(); // Used to create base XML files
            //_xmlCreator.SerializeBiomeDefinition(); // Used to create base XML files
            //_xmlCreator.SerializeBiomeLodeDefinition(); // Used to create base XML files

            // Load Definitions Data from XML files
            _xmlLoader.DeserializeVoxelDefinition();
            _xmlLoader.DeserializeBiomeDefinition();
            _xmlLoader.DeserializeBiomeLodeDefinition();
            Debug.Log("<b>World Data Dictionaries</b> were successful <color=cyan><i>populated</i></color>.");
        }

        public void CreateWorldGameObject() {
            // Create a GameObject for the World
            GameObject @worldObject = new GameObject("WorldMap", typeof(World)) {
                tag = "World",
                layer = 9,
            };
            @worldObject.transform.localPosition = baseVector;
            @worldObject.transform.localScale = Vector3.one;
            @worldObject.transform.localEulerAngles = Vector3.zero;
            bool @tryAddFalse = WorldData.dictionaryMainObject.TryAdd(@worldObject.name, @worldObject);
            if(!@tryAddFalse) {
                Debug.LogError("<b>WorldMap Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryMainObject</b>.");
            } else {
                Debug.Log("<b>" + @worldObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryMainObject</i>.");
            }

            // Create Regions2D for the WorldMap and Setup WorldData
            Region2D @worldQuads = new Region2D(baseVector, worldSize, chunkSize.x, voxelSize.x);
            Region2D @worldChunks = new Region2D(baseVector, worldSize, voxelSize.x);
            Region3D @worldVoxels = new Region3D(baseVector, worldSize, chunkSize, voxelSize);
            World @worldData = @worldObject.GetComponent<World>();
            @worldData._worldMapObject = @worldObject;
            @worldData._worldBounds = new Bounds(new Vector3(baseVector.x, chunkSize.y / 2, baseVector.z), new Vector3((worldSize.x * chunkSize.x) + voxelSize.x, chunkSize.y + voxelSize.y, (worldSize.x * chunkSize.x) + voxelSize.x));
            @worldData._region2DQuads = @worldQuads;
            @worldData._region2DChunks = @worldChunks;
            @worldData._region3DVoxels = @worldVoxels;
            Debug.Log("<b>World Data</b> was successful <color=green><i>generated</i></color>. " +
                      "Size: <i>" + @worldQuads.regionSize + "</i> Quads; Size: <i>" + @worldChunks.regionSize + "</i> Chunks; Size: <i>" + @worldVoxels.regionSize + "</i> Voxels; " +
                      "CenterVector2: <i>" + @worldData._worldBounds.center + "</i>; MinVector2: <i>" + @worldData._worldBounds.min + "</i>; MaxVector2: <i>" + @worldData._worldBounds.max + "</i>.");
        }

        public void GenerateStaticEntities() {
            // Setting up
            GameObject @parentObject = WorldData.dictionaryMainObject["WorldMap"];
            World @worldData = @parentObject.GetComponent<World>();
            Region2D @worldChunks = @worldData._region2DChunks;
            Region3D @worldVoxels = @worldData._region3DVoxels;
            _noiseGenerator.InitializeSeed();

            // Create Chunks GameObjects, Datas and Voxels
            _chunkGenerator.CreateChunksGameObject(@parentObject, @worldChunks, baseVector, worldSize, chunkSize);
            _chunkGenerator.CreateChunksData(@worldChunks, baseVector, worldSize, chunkSize, voxelSize);
            _chunkGenerator.CreateChunksVoxels(@worldChunks, @worldVoxels, baseVector, voxelSize);
        }
    }
}