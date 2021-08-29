using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Visual;

namespace BlueBird.World.Data {
    public sealed class Chunk : MonoBehaviour {
        /* Variables - For Constructor */
        public Vector3 position { get; internal set; }
        //public bool isActive {
        //    get { return this.gameObject.activeSelf; }
        //    set { gameObject.SetActive(value); }
        //}

        /* Instances - For Constructor */
        public Region2D _region2D { get; internal set; }
        public MeshData _meshData { get; set; }

        /* Storage - For Constructor */
        public ConcurrentDictionary<Vector3, Voxel> dictionaryChunkVoxels { get; set; }

        public Chunk() {
            this.position = position;
            //this.isActive = isActive;

            this._region2D = _region2D;
            this._meshData = new MeshData();

            this.dictionaryChunkVoxels = new ConcurrentDictionary<Vector3, Voxel>();
        }
    }
}