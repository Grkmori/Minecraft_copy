using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Definitions;
using BlueBird.World.WorldMap.Topography.Chunks;

namespace BlueBird.World.WorldMap {
    public sealed class WorldMapData {
        /* Storage - Definitions Data */
        public static ConcurrentDictionary<string, VoxelDefinition> dictionaryVoxelDefinition;
        public static ConcurrentDictionary<string, BiomeDefinition> dictionaryBiomeDefinition;
        public static ConcurrentDictionary<string, BiomeLodeDefinition> dictionaryBiomeLodeDefinition;

        /* Storage - Prefabs */
        public static ConcurrentDictionary<string, GameObject> dictionaryStaticWorldPrefabs;

        /* Storage - World GameObjects */
        public static ConcurrentDictionary<Vector3, GameObject> dictionaryChunkObject;

        /* Storage - World Data */
        public static ConcurrentDictionary<Vector3, Chunk> dictionaryChunkData;
    }
}