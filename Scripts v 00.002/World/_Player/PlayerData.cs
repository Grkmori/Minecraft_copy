using UnityEngine;
using System.Collections.Concurrent;

namespace BlueBird.World.Player {
    public sealed class PlayerData {
        /* Storage - Player GameObjects */
        public static ConcurrentDictionary<string, GameObject> dictionaryPlayerObject;
    }
}
