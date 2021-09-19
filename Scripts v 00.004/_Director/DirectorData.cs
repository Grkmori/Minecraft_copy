using UnityEngine;
using System.Collections.Concurrent;

namespace BlueBird.World.Director {
    public sealed class DirectorData {
        /* Storage - Main GameObjects */
        public static ConcurrentDictionary<string, GameObject> dictionaryManagerObject;
        public static ConcurrentDictionary<string, GameObject> dictionaryHandlerObject;
    }
}