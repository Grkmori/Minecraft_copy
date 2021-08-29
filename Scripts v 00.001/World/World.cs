using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Visual;

namespace BlueBird.World {
    public sealed class World : MonoBehaviour {
        /* Variables - For Constructor */
        public Region2D _region2D { get; internal set; }

        public World() {
            this._region2D = _region2D;
        }

        private void Start() {
            DontDestroyOnLoad(this.gameObject); // Dont destroy this GameObject on Scenes changes
        }
    }
}