using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap.Topography.Voxels;
using BlueBird.World.Visual;

namespace BlueBird.World.WorldMap.Topography.Chunks {
    public sealed class ChunkMeshRuntime {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelMeshData _voxelMeshData = new VoxelMeshData();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Variables - For Chunks */
        private readonly float chunkWidth = Constants_str.chunkSize.x;

        /* Variables - For Texture UV Map */
        private readonly float normalizedTextureWidth = Constants_str.faceVoxelTextureSizePixels.x / Constants_str.voxelsTextureAtlasSizePixels.x;
        private readonly float normalizedTextureHeight = Constants_str.faceVoxelTextureSizePixels.y / Constants_str.voxelsTextureAtlasSizePixels.y;
        private readonly float maxTextureIDX = Constants_str.voxelsTextureAtlasSizePixels.x / Constants_str.faceVoxelTextureSizePixels.x;
        private readonly float maxTextureIDY = Constants_str.voxelsTextureAtlasSizePixels.y / Constants_str.faceVoxelTextureSizePixels.y;

        public void GenerateChunkMeshData(Chunk chunkData) {
            // Setting Up
            _chunkUtilities.ClearChunkMeshData(chunkData);
            Vector3 chunkPosition = chunkData.chunkObjectPosition;
            chunkData._chunkMeshData.vertexIndex = 0;

            // Use Region3D to Get MeshData for each 'Position' of the Chunk
            foreach(Vector3 voxelPosition in chunkData._region3DVoxels) {
                Vector3 localPosition = voxelPosition - chunkPosition;
                if(chunkData.dictionaryChunkVoxels[voxelPosition].isSolid) {
                    AddVoxelMeshDataToChunk(localPosition, chunkPosition, chunkData, chunkData.dictionaryChunkVoxels[voxelPosition]);
                }
            }

            // Checking if listDrawnVoxels was populated
            if(chunkData.listDrawnVoxels.Count < chunkWidth * chunkWidth) { // Half Baked Way of Checking if this List was fully Populated
                Debug.LogWarning("<b>Adding Voxels</b> to the  <b>listDrawnVoxel</b> had some problems and <color=yellow><i>was not fully populated</i></color>. " +
                                 "A total of: <i>" + chunkData.listDrawnVoxels.Count + " Voxels </i> were added to the <b>Chunk listDrawnVoxels</b>.");
            } else {
                ///Debug.Log("<b>listDrawnVoxels</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + chunkData.listDrawnVoxels.Count + " Voxels </i>.");
            }

            ///Debug.Log("<b>ChunkMeshData</b> for the <b>" + chunkData.chunkPosition + "</b> was <color=green><i>successfully generated</i></color>.");
        }

        public void AddVoxelMeshDataToChunk(Vector3 localPosition, Vector3 chunkPosition, Chunk chunkData, Voxel voxelData) {
            // Setting up
            bool voxelIsDrawn = false;
            bool voxelIsTransparent = voxelData.isTransparent;

            // Generating and Storing ChunkMeshData
            for(int i = 0; i < 6; i++) {
                if(_voxelUtilities.CheckVoxelFace(localPosition + _voxelMeshData.faceChecks[i], chunkPosition, chunkData, voxelData)) {
                    // Add Values to listVertices, listUVs and listTriangles
                    chunkData._chunkMeshData.listVertices.Add(localPosition + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 0]]);
                    chunkData._chunkMeshData.listVertices.Add(localPosition + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 1]]);
                    chunkData._chunkMeshData.listVertices.Add(localPosition + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 2]]);
                    chunkData._chunkMeshData.listVertices.Add(localPosition + _voxelMeshData.voxelVertices[_voxelMeshData.voxelTrianglesIndices[i, 3]]);
                    if(!voxelIsTransparent) {
                        chunkData._chunkMeshData.listTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex);
                        chunkData._chunkMeshData.listTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 1);
                        chunkData._chunkMeshData.listTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 2);
                        chunkData._chunkMeshData.listTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 2);
                        chunkData._chunkMeshData.listTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 1);
                        chunkData._chunkMeshData.listTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 3);
                    } else {
                        chunkData._chunkMeshData.listTransparentTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex);
                        chunkData._chunkMeshData.listTransparentTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 1);
                        chunkData._chunkMeshData.listTransparentTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 2);
                        chunkData._chunkMeshData.listTransparentTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 2);
                        chunkData._chunkMeshData.listTransparentTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 1);
                        chunkData._chunkMeshData.listTransparentTrianglesIndices.Add(chunkData._chunkMeshData.vertexIndex + 3);
                    }
                    AddTexture(chunkData, _voxelUtilities.GetTextureID(i, voxelData), localPosition);
                    chunkData._chunkMeshData.vertexIndex += 4;
                    voxelIsDrawn = true;
                }
            }

            if(voxelIsDrawn) {
                chunkData.listDrawnVoxels.Add(voxelData);
            }
        }

        private void AddTexture(Chunk chunkData, Vector2 textureID, Vector3 position) {
            // Check if TextureID Values are Valid
            if(textureID.x >= 1 && textureID.x <= maxTextureIDX &&
               textureID.y >= 1 && textureID.y <= maxTextureIDY) {
                // Normalize TextureID Values and Add to listUVs MeshData
                float x = (textureID.x - 1) * normalizedTextureWidth;
                float y = (textureID.y - 1) * normalizedTextureHeight;
                chunkData._chunkMeshData.listUVs.Add(new Vector2(x, y));
                chunkData._chunkMeshData.listUVs.Add(new Vector2(x, y + normalizedTextureHeight));
                chunkData._chunkMeshData.listUVs.Add(new Vector2(x + normalizedTextureWidth, y));
                chunkData._chunkMeshData.listUVs.Add(new Vector2(x + normalizedTextureWidth, y + normalizedTextureHeight));
            } else {
                Debug.LogWarning("<b>TextureID value</b> is <color=yellow><i>invalid</i></color>, probably <color=yellow><i>there are errors</i></color> on this Voxel generation: " + position + ".");
            }
        }

        public void CreateChunkMesh(GameObject chunkObject, Chunk chunkData) {
            Mesh chunkMesh;
            // Generating a Mesh for the ChunkObject
            if(!VisualData.dictionaryChunkMesh.ContainsKey(chunkData.chunkPosition)) {
                chunkMesh = new Mesh();
                chunkMesh.name = chunkObject.name;
                chunkMesh.subMeshCount = 2;
                chunkMesh.SetVertices(chunkData._chunkMeshData.listVertices.ToArray());
                chunkMesh.SetTriangles(chunkData._chunkMeshData.listTrianglesIndices.ToArray(), 0);
                chunkMesh.SetTriangles(chunkData._chunkMeshData.listTransparentTrianglesIndices.ToArray(), 1);
                chunkMesh.SetUVs(0, chunkData._chunkMeshData.listUVs.ToArray());

                // Setting the 'Chunk materials' Array
                chunkData._chunkMeshData.materials[0] = VisualData.dictionaryMaterials["Blocks"];
                chunkData._chunkMeshData.materials[1] = VisualData.dictionaryMaterials["BlocksTransparent"];

                // Linking the Mesh to the Chunk MeshFilter and MeshRenderer
                chunkObject.GetComponent<MeshFilter>().mesh = chunkMesh;
                chunkObject.GetComponent<MeshRenderer>().materials = chunkData._chunkMeshData.materials;
                chunkObject.GetComponent<MeshCollider>().sharedMesh = chunkMesh;

                // Adding Mesh to the dictionaryChunkMesh
                bool tryAddFalse = VisualData.dictionaryChunkMesh.TryAdd(chunkData.chunkPosition, chunkMesh);
                if(!tryAddFalse) {
                    Debug.LogError("<b>Mesh " + chunkData.chunkPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkMesh</b>.");
                } else {
                   ///Debug.Log("<b>Mesh</b> for the <b>" + chunkObject.name + "</b> was <color=green><i>generated</i></color>, and <color=cyan><i>added</i></color> to <b>dictionaryChunkMesh</b>.");
                }
            } else {
                chunkMesh = VisualData.dictionaryChunkMesh[chunkData.chunkPosition];
                chunkMesh.Clear();
                chunkMesh.subMeshCount = 2;
                chunkMesh.SetVertices(chunkData._chunkMeshData.listVertices.ToArray());
                chunkMesh.SetTriangles(chunkData._chunkMeshData.listTrianglesIndices.ToArray(), 0);
                chunkMesh.SetTriangles(chunkData._chunkMeshData.listTransparentTrianglesIndices.ToArray(), 1);
                chunkMesh.SetUVs(0, chunkData._chunkMeshData.listUVs.ToArray());

                chunkObject.GetComponent<MeshCollider>().sharedMesh = null;
                chunkObject.GetComponent<MeshCollider>().sharedMesh = chunkMesh;
            }

            chunkMesh.RecalculateBounds();
            chunkMesh.RecalculateNormals();
            chunkMesh.Optimize();
        }
    }
}