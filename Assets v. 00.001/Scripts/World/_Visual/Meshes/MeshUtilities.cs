using UnityEngine;

namespace BlueBird.World.Visual.Meshes {
    public sealed class MeshUtilities {
        public void CreateFaceUP(MeshData @meshData, Vector3 @origin) {
            Vector3[] @arrayVertices = new Vector3[4];
            @arrayVertices[0] = new Vector3(@origin.x - .5f, @origin.y, @origin.z + .5f);
            @arrayVertices[1] = new Vector3(@origin.x + .5f, @origin.y, @origin.z + .5f);
            @arrayVertices[2] = new Vector3(@origin.x + .5f, @origin.y, @origin.z - .5f);
            @arrayVertices[3] = new Vector3(@origin.x - .5f, @origin.y, @origin.z - .5f);
            @meshData.vertices.AddRange(@arrayVertices);

            int @verticesIndex = @meshData.vertices.Count;
            int[] @arrayTrianglesIndices = new int[6];
            @arrayTrianglesIndices[0] = @verticesIndex - 4;
            @arrayTrianglesIndices[1] = @verticesIndex - 3;
            @arrayTrianglesIndices[2] = @verticesIndex - 2;
            @arrayTrianglesIndices[3] = @verticesIndex - 4;
            @arrayTrianglesIndices[4] = @verticesIndex - 2;
            @arrayTrianglesIndices[5] = @verticesIndex - 1;
            @meshData.trianglesIndices.AddRange(@arrayTrianglesIndices);
        }
    }
}