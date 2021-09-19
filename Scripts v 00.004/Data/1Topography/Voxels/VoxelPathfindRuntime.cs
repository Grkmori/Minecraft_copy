using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Chunks;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelPathfindRuntime {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        Chunk chunkNeighbour;
        Chunk chunkNeighbourNeighbour;
        Voxel voxelNeighbourCheck;
        Voxel voxelNeighbour;
        Voxel voxelNeighbourNeighbour;

        /* Variables - For Chunks */
        private readonly float chunkWidth = Constants_str.chunkSize.x;
        private readonly float chunkHeight = Constants_str.worldBaseVector3.y;

        /* Variables - For Voxels */
        private readonly Vector3 baseVoxelPosition = Constants_str.baseVoxelTerrain;

        private readonly float voxelHeight = Constants_str.voxelSize.y;
        private Vector3 voxelNeighbourPosition;
        private Vector3 voxelNeighbourChunkPosition;
        private Vector3 voxelNeighbourNeighbourPosition;
        private Vector3 voxelNeighbourNeighbourChunkPosition;
        private float posX;
        private float posZ;
        private int loopI;
        private int loopJ;
        private int loopK;
        private int listWalkableVoxelsCount;
        private int listVoxelNeighboursCount;
        private int listNeighbourWalkableVoxelCount;
        private int listNeighbourNeighbourWalkableVoxelCount;

        public void RemoveVoxelFromListWalkableVoxels(Voxel voxelData, Chunk chunkData) {
            if(chunkData.listWalkableVoxels.Contains(voxelData)) {
                // Removing 'Old' WalkableVoxel/'topVoxel' from listWalkableVoxels
                bool removeFalse = chunkData.listWalkableVoxels.Remove(voxelData);
                if(!removeFalse) {
                    Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>failed to remove</i></color> from <b>listWalkableVoxels</b>.");
                } else {
                    ///Debug.Log("<b>VoxelData " + topVoxel.voxelPosition + "</b> was <color=cyan><i>removed</i></color> from <b>listWalkableVoxels</b>.");
                }
            }
        }

        public Voxel UpdateListWalkableVoxelsFromVoxel(Voxel voxelData, Chunk chunkData) {
            // Search for Walkable Voxels: 'voxel'.isSolid = false; 'baseVoxel'.isSolid = true
            if(!WorldData.dictionaryVoxelDefinition[voxelData.voxelTypeName].isSolid) {
                if(WorldData.dictionaryVoxelDefinition[chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + baseVoxelPosition].voxelTypeName].isSolid) {
                    chunkData.listWalkableVoxels.Add(voxelData);

                    // Checking if listWalkableVoxels was Updated
                    if(chunkData.listWalkableVoxels.Count < chunkWidth * chunkWidth) { // Half Baked Way of Checking if this List was fully Populated
                        Debug.LogWarning("<b>Adding Voxels</b> to the  <b>listWalkableVoxels</b> had some problems and <color=yellow><i>was not fully populated</i></color>. " +
                                         "A total of: <i>" + chunkData.listWalkableVoxels.Count + " Voxels</i> were added to the <b>Chunk listWalkableVoxels</b>.");
                    } else {
                        ///Debug.Log("<b>listWalkableVoxels" + chunkData.chunkPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + chunkData.listWalkableVoxels.Count + " Voxels</i>.");
                    }

                    return voxelData;
                } else {
                    ///Debug.Log("<b>Adding Voxels</b> at <b>Voxel " + voxelData.voxelPosition + "</b> to the  <b>listWalkableVoxels</b> could not be <color=cyan><i>completed</i></color>. " +
                    ///          "<i>'baseVoxel'</i> isSolid <color=blue><b>" + WorldData.dictionaryVoxelDefinition[chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + baseVoxelPosition].voxelTypeName].isSolid + "</b></color>. " +
                    ///          "A total of: <i>" + chunkData.listWalkableVoxels.Count + " Voxels</i>.");
                    return null;
                }
            } else {
                Debug.LogWarning("<b>Adding Voxels</b> at <b>Voxel " + voxelData.voxelPosition + "</b> to the  <b>listWalkableVoxels</b> had some problems and <color=yellow><i>could not be added</i></color>. " +
                                 "<i>'Voxel'</i> isSolid <color=blue><b>" + WorldData.dictionaryVoxelDefinition[voxelData.voxelTypeName].isSolid + "</b></color>.");
                return null;
            }
        }

        public void UpdateListVoxelNeighboursOfBaseVoxel(Voxel voxelData) {
            // Updating Neighbours for the new BaseVoxel
            Chunk chunkData = voxelData._chunk;
            listWalkableVoxelsCount = chunkData.listWalkableVoxels.Count;
            for(loopJ = 0; loopJ < 8; loopJ++) {
                // Searching inside listWalkableVoxels
                posX = voxelData.voxelPosition.x + _voxelUtilities.neighbourChecks[loopJ].x;
                posZ = voxelData.voxelPosition.z + _voxelUtilities.neighbourChecks[loopJ].z;
                voxelNeighbourPosition = new Vector3(posX, chunkHeight, posZ);
                voxelNeighbourChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(voxelNeighbourPosition);
                if(chunkData.chunkPosition == voxelNeighbourChunkPosition) {
                    for(loopK = 0; loopK < listWalkableVoxelsCount; loopK++) {
                        // Adding to listVoxelNeighbours
                        voxelNeighbour = chunkData.listWalkableVoxels[loopK];
                        if(voxelNeighbour.voxelPosition.x == posX && voxelNeighbour.voxelPosition.z == posZ && Mathf.Abs(voxelNeighbour.voxelPosition.y - voxelData.voxelPosition.y) <= voxelHeight) {
                            voxelData.listVoxelNeighbours.Add(voxelNeighbour);
                        }
                    }
                } else {
                    if(WorldData.dictionaryChunkData.ContainsKey(voxelNeighbourChunkPosition)) {
                        chunkNeighbour = WorldData.dictionaryChunkData[voxelNeighbourChunkPosition];
                        listNeighbourWalkableVoxelCount = chunkNeighbour.listWalkableVoxels.Count;
                        for(loopK = 0; loopK < listNeighbourWalkableVoxelCount; loopK++) {
                            // Adding to listVoxelNeighbours
                            voxelNeighbour = chunkNeighbour.listWalkableVoxels[loopK];
                            if(voxelNeighbour.voxelPosition.x == posX && voxelNeighbour.voxelPosition.z == posZ && Mathf.Abs(voxelNeighbour.voxelPosition.y - voxelData.voxelPosition.y) <= voxelHeight) {
                                voxelNeighbourCheck.listVoxelNeighbours.Add(voxelNeighbour);
                            }
                        }
                    }
                }
            }

            ///Debug.Log("<b>listVoxelNeighbours" + voxelData.voxelPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + voxelData.listVoxelNeighbours.Count + " Voxels </i>.");
        }

        public void UpdateListVoxelNeighboursOfNeighbours(Voxel voxelData) {
            // Updating Neighbours for each Voxel of listVoxelNeighbours
            Chunk chunkData = voxelData._chunk;
            listVoxelNeighboursCount = voxelData.listVoxelNeighbours.Count;
            listWalkableVoxelsCount = chunkData.listWalkableVoxels.Count;
            for(loopI = 0; loopI < listVoxelNeighboursCount; loopI++) {
                // For each 'NeighbourPosition'
                voxelNeighbourCheck = voxelData.listVoxelNeighbours[loopI];
                voxelNeighbourCheck.listVoxelNeighbours.Clear(); // Clear 'All' VoxelNeighbours form VoxelNeighbour
                for(loopJ = 0; loopJ < 8; loopJ++) {
                    // Searching inside listWalkableVoxels
                    posX = voxelNeighbourCheck.voxelPosition.x + _voxelUtilities.neighbourChecks[loopJ].x;
                    posZ = voxelNeighbourCheck.voxelPosition.z + _voxelUtilities.neighbourChecks[loopJ].z;
                    voxelNeighbourNeighbourPosition = new Vector3(posX, chunkHeight, posZ);
                    voxelNeighbourNeighbourChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(voxelNeighbourNeighbourPosition);
                    if(chunkData.chunkPosition == voxelNeighbourNeighbourChunkPosition) {
                        for(loopK = 0; loopK < listWalkableVoxelsCount; loopK++) {
                            // Adding to listVoxelNeighbours
                            voxelNeighbourNeighbour = chunkData.listWalkableVoxels[loopK];
                            if(voxelNeighbourNeighbour.voxelPosition.x == posX && voxelNeighbourNeighbour.voxelPosition.z == posZ && Mathf.Abs(voxelNeighbourNeighbour.voxelPosition.y - voxelNeighbourCheck.voxelPosition.y) <= voxelHeight) {
                                voxelNeighbourCheck.listVoxelNeighbours.Add(voxelNeighbourNeighbour);
                            }

                        }
                    } else {
                        if(WorldData.dictionaryChunkData.ContainsKey(voxelNeighbourNeighbourChunkPosition)) {
                            chunkNeighbourNeighbour = WorldData.dictionaryChunkData[voxelNeighbourNeighbourChunkPosition];
                            listNeighbourNeighbourWalkableVoxelCount = chunkNeighbourNeighbour.listWalkableVoxels.Count;
                            for(loopK = 0; loopK < listNeighbourNeighbourWalkableVoxelCount; loopK++) {
                                // Adding to listVoxelNeighbours
                                voxelNeighbourNeighbour = chunkNeighbourNeighbour.listWalkableVoxels[loopK];
                                if(voxelNeighbourNeighbour.voxelPosition.x == posX && voxelNeighbourNeighbour.voxelPosition.z == posZ && Mathf.Abs(voxelNeighbourNeighbour.voxelPosition.y - voxelNeighbourCheck.voxelPosition.y) <= voxelHeight) {
                                    voxelNeighbourCheck.listVoxelNeighbours.Add(voxelNeighbourNeighbour);
                                }
                            }
                        }
                    }
                }

                ///Debug.Log("<b>listVoxelNeighbours" + voxelNeighbourCheck.voxelPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + voxelNeighbourCheck.listVoxelNeighbours.Count + " Voxels </i>.");
            }

            // Clear listVoxelNeighbours if VoxelData is not Walkable
            if(!WorldData.dictionaryVoxelDefinition[chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + baseVoxelPosition].voxelTypeName].isSolid) {
                voxelData.listVoxelNeighbours.Clear();
            }
        }
    }
}