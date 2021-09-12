using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Definitions;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World {
    public sealed class WorldData {
        /* Storage - Main GameObjects */
        public static ConcurrentDictionary<string, GameObject> dictionaryMainObject;

        /* Storage - Definitions Data */
        public static ConcurrentDictionary<string, VoxelDefinition> dictionaryVoxelDefinition;
        public static ConcurrentDictionary<string, BiomeDefinition> dictionaryBiomeDefinition;
        public static ConcurrentDictionary<string, BiomeLodeDefinition> dictionaryBiomeLodeDefinition;

        /* Storage - Prefabs */
        public static ConcurrentDictionary<string, GameObject> dictionaryStaticWorldPrefabs;
        public static ConcurrentDictionary<string, GameObject> dictionaryDynamicPrefabs;
        public static ConcurrentDictionary<string, GameObject> dictionaryPlayerPrefabs;
        public static ConcurrentDictionary<string, GameObject> dictionaryPlayerUIPrefabs;

        /* Storage - World GameObjects */
        public static ConcurrentDictionary<Vector3, GameObject> dictionaryChunkObject;

        /* Storage - World Data */
        public static ConcurrentDictionary<Vector3, Chunk> dictionaryChunkData;
    }
}