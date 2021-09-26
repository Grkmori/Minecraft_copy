using UnityEngine;
using System.Collections.Concurrent;
using System.Threading;
using BlueBird.World.Parameters;
using BlueBird.World.Definitions;
using BlueBird.World.WorldMap.Topography.Chunks;
using BlueBird.World.WorldMap.Topography.Noises;

namespace BlueBird.World.WorldMap {
    public sealed class WorldMapGenerator {
        /* Instances */
        ChunkGenerator _chunkGenerator = new ChunkGenerator();
        NoiseGenerator _noiseGenerator = new NoiseGenerator();

        /* Instances - For World Generation */
        GameObject worldObject;

        XMLCreator _xmlCreator;
        XMLLoader _xmlLoader;

        /* Instances - For Threading */
        Thread threadWorldGenerator;

        /* Variables - For World/Chunks */
        private readonly Vector3 baseVector = Constants_str.worldBaseVector3;
        private readonly Vector2 worldSize = Settings_str.worldSizeChunks;
        private readonly Vector2 chunkSize = Constants_str.chunkSize;
        private readonly Vector2 voxelSize = Constants_str.voxelSize;

        #region Dictionaries
        public void CreateWorldMapDictionaries() {
            // Create Main GameObjects Dictionary
            WorldData.dictionaryMainObject = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>World Main Dictionary</b> was successfully <color=green><i>created</i></color>.");

            // Create Definitions Data Dictionaries
            WorldMapData.dictionaryVoxelDefinition = new ConcurrentDictionary<string, VoxelDefinition>();
            WorldMapData.dictionaryBiomeDefinition = new ConcurrentDictionary<string, BiomeDefinition>();
            WorldMapData.dictionaryBiomeLodeDefinition = new ConcurrentDictionary<string, BiomeLodeDefinition>();
            Debug.Log("<b>Definitions Data Dictionaries</b> were successfully <color=green><i>created</i></color>.");

            // Create Prefabs Dictionaries
            WorldMapData.dictionaryStaticWorldPrefabs = new ConcurrentDictionary<string, GameObject>();

            // Create World GameObjects Dictionaries
            WorldMapData.dictionaryChunkObject = new ConcurrentDictionary<Vector3, GameObject>();
            Debug.Log("<b>World GameObjects Dictionaries</b> were successfully <color=green><i>created</i></color>.");

            // Create World Data Dictionaries
            WorldMapData.dictionaryChunkData = new ConcurrentDictionary<Vector3, Chunk>();
            Debug.Log("<b>World Data Dictionaries</b> were successfully <color=green><i>created</i></color>.");
        }

        public void ClearWorldMapDictionaries() {
            // Clearing any Old Persistent from the WorldMapData Dictionaries
            WorldData.dictionaryMainObject.Clear();

            WorldMapData.dictionaryVoxelDefinition.Clear();
            WorldMapData.dictionaryBiomeDefinition.Clear();
            WorldMapData.dictionaryBiomeLodeDefinition.Clear();

            WorldMapData.dictionaryStaticWorldPrefabs.Clear();

            WorldMapData.dictionaryChunkObject.Clear();

            WorldMapData.dictionaryChunkData.Clear();
            Debug.Log("<b>Old Persistent Data</b> from <b>WorldMapData Dictionaries</b> were successfully <color=cyan><i>cleared</i></color> if they had any.");
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
            Debug.Log("<b>World Data Dictionaries</b> were successfully <color=cyan><i>populated</i></color>.");
        }
        #endregion

        #region World Generation
        public void SetupWorldGameObject(GameObject worldObject) {
            // Setup the GameObject for the World
            worldObject.name = "WorldMap";
            bool tryAddFalse = WorldData.dictionaryMainObject.TryAdd(worldObject.name, worldObject);
            if(!tryAddFalse) {
                Debug.LogError("<b>WorldMap Object</b> <color=red><i>failed to add</i></color> to <b>dictionaryMainObject</b>.");
            } else {
                Debug.Log("<b>" + worldObject.name + " GameObject</b> was <color=green><i>generated</i></color>, and <color=cyan><i>stored</i></color> in <i>dictionaryMainObject</i>.");
            }

            // Create Regions2D for the WorldMap and Setup WorldMapData
            Region2D worldQuads = new Region2D(baseVector, worldSize, chunkSize.x, voxelSize.x);
            Region2D worldChunks = new Region2D(baseVector, worldSize, voxelSize.x);
            Region3D worldVoxels = new Region3D(baseVector, worldSize, chunkSize, voxelSize);
            WorldMap worldData = worldObject.GetComponent<WorldMap>();
            worldData._worldMapObject = worldObject;
            worldData._worldBounds = new Bounds(new Vector3(baseVector.x, chunkSize.y / 2, baseVector.z), new Vector3((worldSize.x * chunkSize.x) + voxelSize.x, chunkSize.y + voxelSize.y, (worldSize.x * chunkSize.x) + voxelSize.x));
            worldData._region2DQuads = worldQuads;
            worldData._region2DChunks = worldChunks;
            worldData._region3DVoxels = worldVoxels;
            Debug.Log("<b>World Data</b> was successfully <color=green><i>generated</i></color>. " +
                      "Size: <i>" + worldQuads.regionSize + "</i> Quads; Size: <i>" + worldChunks.regionSize + "</i> Chunks; Size: <i>" + worldVoxels.regionSize + "</i> Voxels; " +
                      "CenterVector2: <i>" + worldData._worldBounds.center + "</i>; MinVector2: <i>" + worldData._worldBounds.min + "</i>; MaxVector2: <i>" + worldData._worldBounds.max + "</i>.");
        }

        public void GenerateNoiseMaps() {
            // Initialize World Seed
            _noiseGenerator.InitializeSeed();
        }

        public void GenerateStaticEntities() {
            // Setting up
            worldObject = WorldData.dictionaryMainObject["WorldMap"];
            WorldMap worldData = worldObject.GetComponent<WorldMap>();
            Region2D worldChunks = worldData._region2DChunks;
            GameObject worldGeneratorAssistant = new GameObject("WorldGeneratorAssistant", typeof(WorldMapGeneratorAssistant)) { // Create a MonoBehaviour to assist on Creating/Instantiating GameObjects
                tag = "Assistant",
                layer = 7
            };

            // Use World Region2DChunks to Create Chunks GameObject for each 'position' of the World
            foreach(Vector2 chunk2DPosition in worldChunks) {
                // Setting up
                Vector3 position = new Vector3(chunk2DPosition.x, baseVector.y, chunk2DPosition.y);

                // Create Chunks GameObjects, Datas and Voxels
                GameObject chunkObject = worldGeneratorAssistant.GetComponent<WorldMapGeneratorAssistant>().InstantiateChunkObject(WorldMapData.dictionaryStaticWorldPrefabs["Chunk"], worldObject.transform, position, baseVector, chunkSize);
                Chunk chunkData;
                _chunkGenerator.SetupChunkGameObject(chunkObject, position, chunkSize, voxelSize, out chunkData);
                _chunkGenerator.CreateChunksVoxels(position, voxelSize);
                ChunkGenerator.queuePopulateDictionaryWalkableVoxels.Enqueue(chunkData);
                ///Debug.Log("<b>ChunkData " + chunkData.chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueGenerateWalkableVoxelsData</b>.");
            }
            _chunkGenerator.ChunksFinalCheck(worldChunks, baseVector, worldSize);

            if(Settings_str.enableThreading) {
                if(WorldDirector.activeThreads < Settings_str.maxThreads) {
                    threadWorldGenerator = new Thread(new ThreadStart(ThreadedGeneration));
                    threadWorldGenerator.Start();
                    WorldDirector.activeThreads++;
                    Debug.Log("<b>threadWorldGenerator</b> function has <color=magenta><i>started</i></color>.");
                } else {
                    Debug.LogWarning("<b>maxThreads Limit</b> has been <color=orange><i>reached</i></color>. Couldnt <b><color=yellow><i>Start</i></color> a new Thread</b>. Threading is <color=cyan><i>disabled</i></color>.");
                    Settings_str.enableThreading = false;
                }
            } else {
                _chunkGenerator.PopulateDictionaryWalkableVoxels();
                _chunkGenerator.PopulateListVoxelNeighbours();
            }

            worldGeneratorAssistant.SetActive(false);  // Disable/Destroy Assistant GameObject that is no longer useful
        }

        private void ThreadedGeneration() {
            while(Settings_str.enableThreading) {
                _chunkGenerator.PopulateDictionaryWalkableVoxels();
                _chunkGenerator.PopulateListVoxelNeighbours();

                Thread.Sleep(1000);

                if(ChunkGenerator.queuePopulateDictionaryWalkableVoxels.Count == 0 && ChunkGenerator.completePopulateDictionaryWalkableVoxels &&
                   ChunkGenerator.queuePopulateListVoxelNeighbours.Count == 0 && ChunkGenerator.completePopulateListVoxelNeighbours) {
                    WorldDirector.activeThreads--;
                    threadWorldGenerator.Abort();
                    Debug.Log("<b>threadWorldGenerator</b> function has been<color=magenta><i>aborted</i></color>.");
                }
            }
        }
        #endregion
    }
}