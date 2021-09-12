using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Player.UI.Utilities;

namespace BlueBird.World.Player {
    public sealed class PlayerData {
        /* Storage - Player GameObjects */
        public static ConcurrentDictionary<string, GameObject> dictionaryPlayerObject;
        public static ConcurrentDictionary<string, GameObject> dictionaryPlayerUIObject;

        /* Storage - For Inventory */
        public static ConcurrentDictionary<Vector2, ItemSlot> dictionaryToolbarItemSlots;
    }
}
