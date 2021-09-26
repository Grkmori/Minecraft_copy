using UnityEngine;
using System.Collections.Concurrent;

namespace BlueBird.World.Entity {
    public sealed class EntityGenerator {
        #region Dictionaries
        public void CreateEntityDictionaries() {
            // Create Prefabs Dictionaries
            EntityData.dictionaryDynamicEntityPrefabs = new ConcurrentDictionary<string, GameObject>();
        }

        public void ClearEntityDictionaries() {
            // Clearing any Old Persistent from the EntityData Dictionaries
            EntityData.dictionaryDynamicEntityPrefabs.Clear();
        }
        #endregion
    }
}