using UnityEngine;
using System.Collections.Concurrent;

namespace BlueBird.World.Entity {
    public sealed class EntityData {
        /* Storage - Prefabs */
        public static ConcurrentDictionary<string, GameObject> dictionaryDynamicEntityPrefabs;
    }
}