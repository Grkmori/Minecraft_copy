using UnityEngine;
using UnityEngine.UI;

namespace BlueBird.World.Player.UI.Utilities {
    public sealed class ItemSlot : MonoBehaviour {
        /* Instances */
        public Image slotItem { get; internal set; }

        /* Variables - For Constructor */
        public Vector2 itemSlotPosition { get; internal set; }

        public ItemSlot() {
            this.itemSlotPosition = itemSlotPosition;

            this.slotItem = slotItem;
        }
    }
}