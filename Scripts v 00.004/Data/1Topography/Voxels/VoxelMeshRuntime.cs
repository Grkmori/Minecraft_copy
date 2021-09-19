using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Chunks;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelMeshRuntime {
        /* Instances */
        ChunkMeshRuntime _chunkMeshRuntime = new ChunkMeshRuntime();
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelMeshData _voxelMeshData = new VoxelMeshData();

        /* Variables - For Chunks */
        private readonly float chunkWidth = Constants_str.chunkSize.x;

        /* Variables - For Voxels */
        private static Dictionary<Vector3, Voxel> dictionaryVoxelAndFacesToDraw = new Dictionary<Vector3, Voxel>();
        private static Dictionary<Vector3, Voxel> dictionaryVoxelToDraw = new Dictionary<Vector3, Voxel>();

        #region Voxel
        public void UpdateChunkMeshDataFromVoxel(Voxel voxelData) {
            // Setting up
            Chunk chunkData = voxelData._chunk;
            Vector3 chunkPosition = chunkData.chunkObjectPosition;
            _chunkUtilities.ClearChunkMeshData(chunkData);
            chunkData._chunkMeshData.vertexIndex = 0;

            // Add 'The Voxel' and its 'Faces' to a temporary dictionaryVoxelAndFacesToDraw
            dictionaryVoxelAndFacesToDraw.Clear();
            AddVoxelAndFacesToDrawDictionary(voxelData, chunkData);

            // Use dictionaryDrawnVoxels Get to MeshData for each 'Drawn Position' of the Chunk
            foreach(Vector3 voxelPosition in dictionaryVoxelAndFacesToDraw.Keys) {
                Vector3 localPosition = voxelPosition - chunkPosition;
                if(WorldData.dictionaryVoxelDefinition[chunkData.dictionaryChunkVoxels[voxelPosition].voxelTypeName].isSolid) {
                    _chunkMeshRuntime.AddVoxelMeshDataToChunk(localPosition, chunkPosition, chunkData, chunkData.dictionaryChunkVoxels[voxelPosition]);
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

        private void AddVoxelAndFacesToDrawDictionary(Voxel voxelData, Chunk chunkData) {
            // Cloning listDrawnVoxels to dictionaryVoxelToDraw
            int listDrawnVoxelCount = chunkData.listDrawnVoxels.Count;
            for(int i = 0; i < listDrawnVoxelCount; i++) {
                dictionaryVoxelAndFacesToDraw.Add(chunkData.listDrawnVoxels[i].voxelPosition, chunkData.listDrawnVoxels[i]);
            }

            // Adding 'The Voxel' and its 'Faces' to dictionaryVoxelAndFacesToDraw
            Vector3 theVoxelPosition = voxelData.voxelPosition;
            if(!dictionaryVoxelAndFacesToDraw.ContainsKey(theVoxelPosition)) {
                dictionaryVoxelAndFacesToDraw.Add(theVoxelPosition, voxelData);
            }
            for(int i = 0; i < 6; i++) {
                if(!dictionaryVoxelAndFacesToDraw.ContainsKey(theVoxelPosition + _voxelMeshData.faceChecks[i])) {
                    if(chunkData.dictionaryChunkVoxels.ContainsKey(theVoxelPosition + _voxelMeshData.faceChecks[i])) {
                        dictionaryVoxelAndFacesToDraw.Add(theVoxelPosition + _voxelMeshData.faceChecks[i], chunkData.dictionaryChunkVoxels[theVoxelPosition + _voxelMeshData.faceChecks[i]]);
                    }
                }
            }

            chunkData.listDrawnVoxels.Clear();
            ///Debug.Log("<b>dictionaryVoxelAndFacesToDraw</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + chunkData.dictionaryVoxelAndFacesToDraw.Count + " Voxels </i>.");
        }
        #endregion

        #region FaceVoxel
        public void UpdateChunkMeshDataFromFaceVoxel(Voxel voxelData) {
            // Setting up
            Chunk chunkData = voxelData._chunk;
            Vector3 chunkPosition = chunkData.chunkObjectPosition;
            _chunkUtilities.ClearChunkMeshData(chunkData);
            chunkData._chunkMeshData.vertexIndex = 0;

            // Add 'The Voxel' and its 'Faces' to a temporary dictionaryVoxelToDraw
            dictionaryVoxelToDraw.Clear();
            AddVoxelToDrawDictionary(voxelData, chunkData);

            // Use dictionaryDrawnVoxels Get to MeshData for each 'Drawn Position' of the Chunk
            foreach(Vector3 voxelPosition in dictionaryVoxelToDraw.Keys) {
                Vector3 localPosition = voxelPosition - chunkPosition;
                if(WorldData.dictionaryVoxelDefinition[chunkData.dictionaryChunkVoxels[voxelPosition].voxelTypeName].isSolid) {
                    _chunkMeshRuntime.AddVoxelMeshDataToChunk(localPosition, chunkPosition, chunkData, chunkData.dictionaryChunkVoxels[voxelPosition]);
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

        private void AddVoxelToDrawDictionary(Voxel voxelData, Chunk chunkData) {
            // Cloning listDrawnVoxels to dictionaryVoxelToDraw
            int listDrawnVoxelCount = chunkData.listDrawnVoxels.Count;
            for(int i = 0; i < listDrawnVoxelCount; i++) {
                dictionaryVoxelToDraw.Add(chunkData.listDrawnVoxels[i].voxelPosition, chunkData.listDrawnVoxels[i]);
            }

            // Adding 'The Voxel' and its 'Faces' to dictionaryVoxelToDraw
            Vector3 theVoxelPosition = voxelData.voxelPosition;
            if(!dictionaryVoxelToDraw.ContainsKey(theVoxelPosition)) {
                dictionaryVoxelToDraw.Add(theVoxelPosition, voxelData);
            }

            chunkData.listDrawnVoxels.Clear();
            ///Debug.Log("<b>dictionaryVoxelToDraw</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + chunkData.dictionaryVoxelToDraw.Count + " Voxels </i>.");
        }
        #endregion
    }
}
