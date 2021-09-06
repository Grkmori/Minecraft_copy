using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Data.InvisibleData.Chunks {
    public sealed class ChunkMeshData {
        /* Variables - For Constructor */
        public bool isPopulated { get; set; }

        /* Storage - For Constructor */
        public List<Vector3> listVertices { get; internal set; }
        public List<int> listTrianglesIndices { get; internal set; }
        public List<Vector2> listUVs { get; internal set; }

        public ChunkMeshData() {
            this.isPopulated = isPopulated;

            this.listVertices = new List<Vector3>();
            this.listTrianglesIndices = new List<int>();
            this.listUVs = new List<Vector2>();
        }
    }
}