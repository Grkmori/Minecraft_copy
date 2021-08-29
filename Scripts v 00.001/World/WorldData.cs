using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Definitions;
using BlueBird.World.Data;

namespace BlueBird.World {
    public sealed class WorldData {
        /* Storage - Definitions Data */
        public static ConcurrentDictionary<string, VoxelDefinition> dictionaryVoxelDefinition;
        //public static ConcurrentDictionary<string, DefinitionVegetation> definitionVegetationDictionary;
        //public static ConcurrentDictionary<string, DefinitionStructure> definitionStructureDictionary;
        //public static ConcurrentDictionary<string, DefinitionAnimal> definitionAnimalDictionary;
        //public static ConcurrentDictionary<string, DefinitionMonster> definitionMonsterDictionary;
        //public static ConcurrentDictionary<string, DefinitionCharacter> definitionCharacterDictionary;
        //public static ConcurrentDictionary<string, DefinitionClimate> definitionClimateDictionary;

        /* Storage - World Data */
        public static ConcurrentDictionary<Vector3, GameObject> dictionaryChunks;
    }
}