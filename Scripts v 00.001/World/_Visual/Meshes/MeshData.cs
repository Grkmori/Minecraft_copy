using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Visual {
    public sealed class MeshData {
        /* Variables - For Constructor */
        public List<Vector3> listVertices { get; internal set; }
        public List<int> listTrianglesIndices { get; internal set; }
        public List<Vector2> listUVs { get; internal set; }

        public MeshData() {
            this.listVertices = new List<Vector3>();
            this.listTrianglesIndices = new List<int>();
            this.listUVs = new List<Vector2>();
        }
    }
}