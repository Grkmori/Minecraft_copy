using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Data.InvisibleData.Chunks {
    public sealed class Chunk : MonoBehaviour {
        /* Instances - For Constructor */
        public Bounds _chunkBounds { get; internal set; }

        public Region2D _region2DQuads { get; internal set; }
        public Region3D _region3DVoxels { get; internal set; }
        public ChunkMeshData _chunkMeshData { get; set; }

        /* Storage - For Constructor */
        public ConcurrentDictionary<Vector3, Voxel> dictionaryChunkVoxels { get; set; }
        public ConcurrentDictionary<Vector3, Voxel> dictionaryDrawnVoxels { get; set; }

        /* Variables - For Constructor */
        public Vector3 chunkPosition { get; internal set; }
        public Vector3 chunkObjectPosition { get; internal set; }

        public Chunk() {
            this.chunkPosition = chunkPosition;
            this.chunkObjectPosition = chunkObjectPosition;

            this._chunkBounds = _chunkBounds;

            this._region2DQuads = _region2DQuads;
            this._region3DVoxels = _region3DVoxels;
            this._chunkMeshData = new ChunkMeshData();

            this.dictionaryChunkVoxels = new ConcurrentDictionary<Vector3, Voxel>();
            this.dictionaryDrawnVoxels = new ConcurrentDictionary<Vector3, Voxel>();
        }

        /* Behaviours */
        public bool IsVisible() {
            return gameObject.activeSelf;
        }

        public void SetVisible(bool @visible) {
            gameObject.SetActive(@visible);
        }
    }
}