using UnityEngine;
using System.Collections.Concurrent;

namespace BlueBird.World {
    public sealed class WorldData {
        /* Storage - Main Managers/Handlers */
        public static ConcurrentDictionary<string, GameObject> dictionaryManagerObject;
        public static ConcurrentDictionary<string, GameObject> dictionaryHandlerObject;

        /* Storage - Main GameObjects */
        public static ConcurrentDictionary<string, GameObject> dictionaryMainObject;
    }
}