using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Visual;

namespace BlueBird.World.WorldMap {
    public sealed class WorldMap : MonoBehaviour {
        /* Instances - For Constructor */
        public GameObject _worldMapObject { get; internal set; }
        public Bounds _worldBounds { get; internal set; }

        public Region2D _region2DQuads { get; internal set; }
        public Region2D _region2DChunks { get; internal set; }
        public Region3D _region3DVoxels { get; internal set; }

        public WorldMap() {
            this._worldMapObject = _worldMapObject;
            this._worldBounds = _worldBounds;

            this._region2DQuads = _region2DQuads;
            this._region2DChunks = _region2DChunks;
            this._region3DVoxels = _region3DVoxels;
        }
    }
}