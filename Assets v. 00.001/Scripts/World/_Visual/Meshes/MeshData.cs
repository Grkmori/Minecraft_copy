using System.Collections.Generic;
using UnityEngine;

namespace BlueBird.World.Visual.Meshes {
    public sealed class MeshData {
        /* Variables */
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> trianglesIndices = new List<int>();
        public List<Vector2> uvs = new List<Vector2>();

        private void AddVertices(List<Vector3> @listVertices) {
            vertices.AddRange(@listVertices);
        }

        public void AddTriangles(List<int> @listTriangles) {
            trianglesIndices.AddRange(@listTriangles);
        }

        public void AddUVs(List<Vector2> @listuvs) {
            uvs.AddRange(@listuvs);
        }
    }
}