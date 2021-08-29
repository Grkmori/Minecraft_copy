using UnityEngine;
using BlueBird.World;
using BlueBird.World.Parameters;
using BlueBird.World.Data;

namespace BlueBird.World.Visual {
    public sealed class StaticMeshesGenerator {
        /* Variables - For Voxels */
        private Vector3 baseVector = Constants_str.worldBaseVector3;
        private float maxWidth = Constants_str.chunkWidth;
        private float maxHeight = Constants_str.chunkHeight;
        private float addWidth = Constants_str.voxelWidth;
        private float addHeight = Constants_str.voxelHeight;
        private int voxelVertexIndex;

        /* Variables - For Texture UV Map */
        private float normalizedTextureWidth = Constants_str.voxelTextureWidthPixels / VisualData.dictionaryTextures["Blocks"].width;
        private float normalizedTextureHeight = Constants_str.voxelTextureHeightPixels / VisualData.dictionaryTextures["Blocks"].height;
        private float maxTextureIDX = VisualData.dictionaryTextures["Blocks"].width / Constants_str.voxelTextureWidthPixels;
        private float maxTextureIDY = VisualData.dictionaryTextures["Blocks"].height / Constants_str.voxelTextureHeightPixels;

        public void CreateChunksData(Chunk @chunkData) {
            // Setting Up
            voxelVertexIndex = 0;

            // Add MeshData of each Voxel to the Chunk Mesh
            for(float y = baseVector.y; y <= maxHeight; y = y + addHeight) {
                for(float x = baseVector.x; x <= maxWidth; x = x + addWidth) {
                    for(float z = baseVector.z; z <= maxWidth; z = z + addWidth) {
                        Vector3 @position = new Vector3(x, y, z);
                        Voxel @voxel = @chunkData.dictionaryChunkVoxels[@position];
                        AddVoxelMeshDataToChunk(@position, @chunkData._meshData, @voxel);
                    }
                }
            }
        }

        private void AddVoxelMeshDataToChunk(Vector3 @position, MeshData @meshData, Voxel @voxel) {
            for(int i = 0; i < 6; i++) {
                if(!VoxelFaceCheck(@position + VoxelMeshData.faceChecks[i], @voxel)) {
                    // Add Values to listVertices, listUVs and listTriangles
                    @meshData.listVertices.Add(@position + VoxelMeshData.voxelVertices[VoxelMeshData.voxelTrianglesIndices[i, 0]]);
                    @meshData.listVertices.Add(@position + VoxelMeshData.voxelVertices[VoxelMeshData.voxelTrianglesIndices[i, 1]]);
                    @meshData.listVertices.Add(@position + VoxelMeshData.voxelVertices[VoxelMeshData.voxelTrianglesIndices[i, 2]]);
                    @meshData.listVertices.Add(@position + VoxelMeshData.voxelVertices[VoxelMeshData.voxelTrianglesIndices[i, 3]]);
                    AddTexture(meshData, GetTextureID(i, @voxel), @position);
                    @meshData.listTrianglesIndices.Add(voxelVertexIndex);
                    @meshData.listTrianglesIndices.Add(voxelVertexIndex + 1);
                    @meshData.listTrianglesIndices.Add(voxelVertexIndex + 2);
                    @meshData.listTrianglesIndices.Add(voxelVertexIndex + 2);
                    @meshData.listTrianglesIndices.Add(voxelVertexIndex + 1);
                    @meshData.listTrianglesIndices.Add(voxelVertexIndex + 3);
                    voxelVertexIndex += 4;
                }
            }
        }

        private bool VoxelFaceCheck(Vector3 @position, Voxel @voxel) {
            // Checking Voxel "faces" to be Rendered or not
            int x = (int)@position.x;
            int y = (int)@position.y;
            int z = (int)@position.z;
            if(!VoxelInChunk(@position)) {
                return false;
            }
            else {
                return WorldData.dictionaryVoxelDefinition[@voxel.voxelTypeName].isSolid;
            }
        }

        private bool VoxelInChunk(Vector3 @position) {
            if(@position.x < baseVector.x || @position.x > maxWidth ||
               @position.y < baseVector.y || @position.y > maxHeight ||
               @position.z < baseVector.z || @position.z > maxWidth) {
                return false;
            }
            else {
                return true;
            }
        }

        private void AddTexture(MeshData @meshData, Vector2 @textureID, Vector3 @position) {
            // Check if TextureID Values are Valid
            if(@textureID.x >= 1 && @textureID.x <= maxTextureIDX &&
               @textureID.y >= 1 && @textureID.y <= maxTextureIDY) {
                // Normalize TextureID Values and Add to listUVs MeshData
                float x = (@textureID.x - 1) * normalizedTextureWidth;
                float y = (@textureID.y - 1) * normalizedTextureHeight;
                @meshData.listUVs.Add(new Vector2(x, y));
                @meshData.listUVs.Add(new Vector2(x, y + normalizedTextureHeight));
                @meshData.listUVs.Add(new Vector2(x + normalizedTextureWidth, y));
                @meshData.listUVs.Add(new Vector2(x + normalizedTextureWidth, y + normalizedTextureHeight));
            }
            else {
                Debug.LogWarning("<b>TextureID value</b> is <color=yellow><i>invalid</i></color>, <color=yellow><i>probably there are errors</i></color> on this Voxel generation: " + @position + ".");
            }
        }

        private Vector2 GetTextureID(int @faceIndex, Voxel @voxel) {
            switch(@faceIndex) {
                case 0:
                    return WorldData.dictionaryVoxelDefinition[@voxel.voxelTypeName].backFaceTexture;
                case 1:
                    return WorldData.dictionaryVoxelDefinition[@voxel.voxelTypeName].frontFaceTexture;
                case 2:
                    return WorldData.dictionaryVoxelDefinition[@voxel.voxelTypeName].topFaceTexture;
                case 3:
                    return WorldData.dictionaryVoxelDefinition[@voxel.voxelTypeName].bottomFaceTexture;
                case 4:
                    return WorldData.dictionaryVoxelDefinition[@voxel.voxelTypeName].leftFaceTexture;
                case 5:
                    return WorldData.dictionaryVoxelDefinition[@voxel.voxelTypeName].rightFaceTexture;
                default:
                    Debug.LogWarning("<b>GetTextureID</b> couldnt be <color=yellow><i>found</i></color>, <color=yellow><i>invalid Face Index</i></color>, returning <i>topFaceTexture</i> as <i>default</i>.");
                    return new Vector2(1, 1);
            }
        }

        public void CreateChunksMesh(GameObject @chunkObject, MeshData @meshData) {
            // Generating a new Mesh for WorldMap
            Mesh @mesh = new Mesh() {
                name = @chunkObject.name,
                vertices = @meshData.listVertices.ToArray(),
                triangles = @meshData.listTrianglesIndices.ToArray(),
                uv = @meshData.listUVs.ToArray()
            };
            @mesh.RecalculateNormals();
            @mesh.RecalculateBounds();
            @mesh.Optimize();

            // Linking the Mesh to the Chunk MeshFilter and MeshRenderer
            @chunkObject.GetComponent<MeshFilter>().mesh = @mesh;
            @chunkObject.GetComponent<MeshRenderer>().material = VisualData.dictionaryMaterials["Blocks"];
            Debug.Log("<b>Mesh</b> for the <b>" + @chunkObject.name + "</b> was <color=green><i>generated</i></color>.");
        }
    }
}