using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Data.Topography.Chunks {
    public sealed class ChunkMeshData {
        /* Storage - For Constructor */
        public List<Vector3> listVertices { get; internal set; }
        public List<int> listTrianglesIndices { get; internal set; }
        public List<int> listTransparentTrianglesIndices { get; internal set; }
        public List<Vector2> listUVs { get; internal set; }

        public Material[] materials = new Material[2];

        /* Variables - For Constructor */
        public int vertexIndex { get; internal set; }

        public ChunkMeshData() {
            this.vertexIndex = 0;

            this.listVertices = new List<Vector3>();
            this.listTrianglesIndices = new List<int>();
            this.listTransparentTrianglesIndices = new List<int>();
            this.listUVs = new List<Vector2>();
        }
    }
}