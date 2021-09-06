using UnityEngine;
using BlueBird.World;
using BlueBird.World.Parameters;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;
using BlueBird.World.Visual;

namespace BlueBird.World.Data.InvisibleData.Chunks {
    public sealed class ChunkMeshGenerator {
        /* Instances */
        VoxelMeshData _voxelMeshData = new VoxelMeshData();
        ChunkUtilities _chunkUtilities = new ChunkUtilities();

        /* Variables - For Voxels */
        private int voxelVertexIndex;

        /* Variables - For Texture UV Map */
        private readonly float normalizedTextureWidth = Constants_str.faceVoxelTextureSizePixels.x / Constants_str.voxelsTextureAtlasSizePixels.x;
        private readonly float normalizedTextureHeight = Constants_str.faceVoxelTextureSizePixels.y / Constants_str.voxelsTextureAtlasSizePixels.y;
        private readonly float maxTextureIDX = Constants_str.voxelsTextureAtlasSizePixels.x / Constants_str.faceVoxelTextureSizePixels.x;
        private readonly float maxTextureIDY = Constants_str.voxelsTextureAtlasSizePixels.y / Constants_str.faceVoxelTextureSizePixels.y;

        public void GenerateChunkMeshData(Chunk @chunkData) {
            // Setting Up
            ClearChunkMeshData(@chunkData);
            Vector3 @chunkPosition = @chunkData.chunkObjectPosition;
            voxelVertexIndex = 0;

            // Use Region3D to MeshData for each 'position' of the Chunk
            foreach(Vector3 @voxelPosition in @chunkData._region3DVoxels) {
                Vector3 @position = voxelPosition - @chunkPosition;
                if(WorldData.dictionaryVoxelDefinition[@chunkData.dictionaryChunkVoxels[@voxelPosition].voxelTypeName].isSolid) {
                    AddVoxelMeshDataToChunk(@position, @chunkPosition, @chunkData, @chunkData.dictionaryChunkVoxels[@voxelPosition]);
                }
            }
            ///Debug.Log("<b>ChunkMeshData</b> for the <b>" + @chunkData.chunkPosition + "</b> was <color=green><i>successfully generated</i></color>.");
        }

        private void ClearChunkMeshData(Chunk @chunkData) {
            @chunkData._chunkMeshData.isPopulated = false;
            @chunkData._chunkMeshData.listVertices.Clear();
            @chunkData._chunkMeshData.listTrianglesIndices.Clear();
            @chunkData._chunkMeshData.listUVs.Clear();
        }

        private void AddVoxelMeshDataToChunk(Vector3 @position, Vector3 @chunkPosition, Chunk @chunkData, Voxel @voxelData) {
            for(int i = 0; i < 6; i++) {
                if(CheckVoxelFace(@position + _voxelMeshData.faceChecks[i], @chunkPosition, @chunkData, @voxelData)) {
                    // Add Values to listVertices, listUVs and listTriangles
                    @chunkData._chunkMeshData.listVertices.Add(@position + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 0]]);
                    @chunkData._chunkMeshData.listVertices.Add(@position + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 1]]);
                    @chunkData._chunkMeshData.listVertices.Add(@position + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 2]]);
                    @chunkData._chunkMeshData.listVertices.Add(@position + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 3]]);
                    AddTexture(@chunkData, GetTextureID(i, @voxelData), @position);
                    @chunkData._chunkMeshData.listTrianglesIndices.Add(voxelVertexIndex);
                    @chunkData._chunkMeshData.listTrianglesIndices.Add(voxelVertexIndex + 1);
                    @chunkData._chunkMeshData.listTrianglesIndices.Add(voxelVertexIndex + 2);
                    @chunkData._chunkMeshData.listTrianglesIndices.Add(voxelVertexIndex + 2);
                    @chunkData._chunkMeshData.listTrianglesIndices.Add(voxelVertexIndex + 1);
                    @chunkData._chunkMeshData.listTrianglesIndices.Add(voxelVertexIndex + 3);
                    voxelVertexIndex += 4;
                    @chunkData._chunkMeshData.isPopulated = true;
                }
            }
        }

        private bool CheckVoxelFace(Vector3 @facePosition, Vector3 @chunkPosition, Chunk @chunkData, Voxel @voxelData) {
            // Setting up
            Vector3 @voxelFacePosition = @facePosition + @chunkPosition;
            Vector3 @chunkFacePosition = _chunkUtilities.GetChunkPositionFromPosition(@voxelFacePosition);

            // Checking if Voxel "Faces"(Voxel next to the current Voxel) to be Rendered or Not
            if(WorldData.dictionaryChunkData.ContainsKey(@chunkFacePosition)) {
                if(@chunkData.dictionaryChunkVoxels.ContainsKey(@voxelFacePosition)) {
                    ///Debug.Log("If <b>'VoxelFace' " + @voxelFacePosition + "<b> is <color=orange><i>inside of 'Chunk Bounds'</i></color>, return <i>opposite of <b>'Voxel'</b> isSolid</i>");
                    return !WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[@chunkFacePosition].dictionaryChunkVoxels[@voxelFacePosition].voxelTypeName].isSolid;
                } else {
                    ///Debug.Log("If <b>'VoxelFace' " + @voxelFacePosition + "<b> is <color=orange><i>out of 'Chunk Bounds'</i></color>, return if <i><b>'Voxel'</b> isSolid</i>");
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].isSolid;
                }
            } else {
                ///Debug.Log("If <b>'VoxelFace' " + @voxelFacePosition + "<b> is <color=orange><i>out of 'World Bounds'</i></color>, return if <i><b>'Voxel'</b> isSolid</i>");
                return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].isSolid;
            }
        }

        private void AddTexture(Chunk @chunkData, Vector2 @textureID, Vector3 @position) {
            // Check if TextureID Values are Valid
            if(@textureID.x >= 1 && @textureID.x <= maxTextureIDX &&
               @textureID.y >= 1 && @textureID.y <= maxTextureIDY) {
                // Normalize TextureID Values and Add to listUVs MeshData
                float x = (@textureID.x - 1) * normalizedTextureWidth;
                float y = (@textureID.y - 1) * normalizedTextureHeight;
                @chunkData._chunkMeshData.listUVs.Add(new Vector2(x, y));
                @chunkData._chunkMeshData.listUVs.Add(new Vector2(x, y + normalizedTextureHeight));
                @chunkData._chunkMeshData.listUVs.Add(new Vector2(x + normalizedTextureWidth, y));
                @chunkData._chunkMeshData.listUVs.Add(new Vector2(x + normalizedTextureWidth, y + normalizedTextureHeight));
            } else {
                Debug.LogWarning("<b>TextureID value</b> is <color=yellow><i>invalid</i></color>, <color=yellow><i>probably there are errors</i></color> on this Voxel generation: " + @position + ".");
            }
        }

        private Vector2 GetTextureID(int @faceIndex, Voxel @voxelData) {
            switch(@faceIndex) {
                case 0:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].backFaceTexture;
                case 1:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].frontFaceTexture;
                case 2:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].topFaceTexture;
                case 3:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].bottomFaceTexture;
                case 4:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].leftFaceTexture;
                case 5:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].rightFaceTexture;
                default:
                    Debug.LogWarning("<b>GetTextureID</b> couldnt be <color=yellow><i>found</i></color>, <color=yellow><i>invalid Face Index</i></color>, returning <i>topFaceTexture</i> as <i>default</i>.");
                    return new Vector2(1, 1);
            }
        }

        public void CreateChunkMesh(GameObject @chunkObject, Chunk @chunkData) {
            // Generating a Mesh for the ChunkObject
            Mesh @chunkMesh;
            if(VisualData.dictionaryChunkMesh.ContainsKey(@chunkData.chunkPosition)) {
                @chunkMesh = VisualData.dictionaryChunkMesh[@chunkData.chunkPosition];
                @chunkMesh.Clear();
                @chunkMesh.vertices = @chunkData._chunkMeshData.listVertices.ToArray();
                @chunkMesh.triangles = @chunkData._chunkMeshData.listTrianglesIndices.ToArray();
                @chunkMesh.uv = @chunkData._chunkMeshData.listUVs.ToArray();
            } else {
                @chunkMesh = new Mesh() {
                    name = @chunkObject.name,
                    vertices = @chunkData._chunkMeshData.listVertices.ToArray(),
                    triangles = @chunkData._chunkMeshData.listTrianglesIndices.ToArray(),
                    uv = @chunkData._chunkMeshData.listUVs.ToArray()
                };

                // Linking the Mesh to the Chunk MeshFilter and MeshRenderer
                @chunkObject.GetComponent<MeshFilter>().mesh = @chunkMesh;
                @chunkObject.GetComponent<MeshRenderer>().material = VisualData.dictionaryMaterials["Blocks"];

                // Adding Mesh to the dictionaryChunkMesh
                bool @tryAddFalse = VisualData.dictionaryChunkMesh.TryAdd(@chunkData.chunkPosition, @chunkMesh);
                if(!@tryAddFalse) {
                    Debug.LogError("<b>Mesh " + @chunkData.chunkPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkMesh</b>.");
                } else {
                    ///Debug.Log("<b>Mesh</b> for the <b>" + @chunkObject.name + "</b> was <color=green><i>generated</i></color>, and <color=cyan><i>added</i></color> to <b>dictionaryChunkMesh</b>.");
                }
            }

            @chunkMesh.RecalculateNormals();
            @chunkMesh.RecalculateBounds();
            @chunkMesh.Optimize();
        }
    }
}