using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap.Topography.Chunks;

namespace BlueBird.World.WorldMap.Topography.Voxels {
    public sealed class VoxelPathfindRuntime {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Storage - For Voxels */
        List<Voxel> listVoxelNeighbourXCheck;
        List<Voxel> listVoxelNeighbourZCheck;
        List<Voxel> listVoxelNeighbourCheckRemove;

        /* Variables - For Chunks */
        private readonly float chunkWidth = Constants_str.chunkSize.x;

        /* Variables - For Voxels */
        private readonly Vector3 bottomVoxel = Constants_str.bottomVoxelPosition;

        private readonly Vector3 topVoxel = Constants_str.topVoxelPosition;
        private readonly float voxelHeight = Constants_str.voxelSize.y;

        public void RemoveVoxelFromDictionaryWalkableVoxels(Voxel voxelData, Chunk chunkData) {
            if(chunkData.dictionaryWalkableVoxels.ContainsKey(voxelData.voxelPosition)) {
                // Removing 'Old' WalkableVoxel/'topVoxel' from dictionaryWalkableVoxels
                bool removeFalse = chunkData.dictionaryWalkableVoxels.Remove(voxelData.voxelPosition);
                if(!removeFalse) {
                    Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>failed to remove</i></color> from <b>dictionaryWalkableVoxels</b>.");
                } else {
                    ///Debug.Log("<b>VoxelData " + topVoxel.voxelPosition + "</b> was <color=cyan><i>removed</i></color> from <b>dictionaryWalkableVoxels</b>.");
                }
            }
        }

        public Voxel UpdateDictionaryWalkableVoxelsFromVoxel(Voxel voxelData, Chunk chunkData) {
            // Search for Walkable Voxels: 'voxel'.isWalkable = true; 'baseVoxel'.isSolid = true
            if(chunkData.dictionaryChunkVoxels[voxelData.voxelPosition].isWalkable) {
                if(chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + bottomVoxel].isSolid) {
                    if(!chunkData.dictionaryWalkableVoxels.ContainsKey(voxelData.voxelPosition)) {
                        chunkData.dictionaryWalkableVoxels.Add(voxelData.voxelPosition, voxelData);
                    }

                    // Checking if dictionaryWalkableVoxels was Updated
                    if(chunkData.dictionaryWalkableVoxels.Count < chunkWidth * chunkWidth) { // Half Baked Way of Checking if this Dictionary was fully Populated
                        Debug.LogWarning("<b>Adding Voxels</b> to the  <b>dictionaryWalkableVoxels</b> had some problems and <color=yellow><i>was not fully populated</i></color>. " +
                                         "A total of: <i>" + chunkData.dictionaryWalkableVoxels.Count + " Voxels</i> were added to the <b>Chunk dictionaryWalkableVoxels</b>.");
                    } else {
                        ///Debug.Log("<b>dictionaryWalkableVoxels" + chunkData.chunkPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + chunkData.dictionaryWalkableVoxels.Count + " Voxels</i>.");
                    }

                    return voxelData;
                } else {
                    ///Debug.Log("<b>Adding Voxels</b> at <b>Voxel " + voxelData.voxelPosition + "</b> to the  <b>dictionaryWalkableVoxels</b> could not be <color=cyan><i>completed</i></color>. " +
                    ///          "<i>'baseVoxel'</i> isSolid <color=blue><b>" + chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + baseVoxelPosition].isSolid + "</b></color>. " +
                    ///          "A total of: <i>" + chunkData.dictionaryWalkableVoxels.Count + " Voxels</i>.");
                    return null;
                }
            } else {
                Debug.LogWarning("<b>Adding Voxels</b> at <b>Voxel " + voxelData.voxelPosition + "</b> to the  <b>dictionaryWalkableVoxels</b> had some problems and <color=yellow><i>could not be added</i></color>. " +
                                 "<i>'Voxel'</i> isSolid <color=blue><b>" + voxelData.isSolid + "</b></color>.");
                return null;
            }
        }

        public void UpdateListVoxelNeighboursOfBaseVoxel(Voxel voxelData) {
            // Updating Neighbours for the new BaseVoxel
            Chunk chunkData = voxelData._chunk;
            int neighbourChecksLenght = _voxelUtilities.neighbourChecks.Length;
            for(int loopI = 0; loopI < neighbourChecksLenght; loopI++) {
                // Searching inside dictionaryWalkableVoxels
                Vector3 voxelNeighbourPosition = voxelData.voxelPosition + _voxelUtilities.neighbourChecks[loopI];
                Vector3 voxelNeighbourChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(voxelNeighbourPosition);
                if(chunkData.chunkPosition == voxelNeighbourChunkPosition) {
                    // Adding to listVoxelNeighbours
                    if(chunkData.dictionaryWalkableVoxels.ContainsKey(voxelNeighbourPosition)) {
                        Voxel voxelNeighbour = chunkData.dictionaryWalkableVoxels[voxelNeighbourPosition];
                        voxelData.listVoxelNeighbours.Add(voxelNeighbour);
                        ///Debug.Log(voxelData.voxelPosition + " " + voxelNeighbour.voxelPosition);
                    }
                } else {
                    if(WorldMapData.dictionaryChunkData.ContainsKey(voxelNeighbourChunkPosition)) {
                        Chunk chunkNeighbour = WorldMapData.dictionaryChunkData[voxelNeighbourChunkPosition];
                        // Adding to listVoxelNeighbours
                        if(chunkNeighbour.dictionaryWalkableVoxels.ContainsKey(voxelNeighbourPosition)) {
                            Voxel voxelNeighbour = chunkNeighbour.dictionaryWalkableVoxels[voxelNeighbourPosition];
                            voxelData.listVoxelNeighbours.Add(voxelNeighbour);
                            ///Debug.Log(voxelData.voxelPosition + " " + voxelNeighbour.voxelPosition);
                        }
                    }
                }
            }

            // Check if voxelNeighbour has a 'Walkable' Path, if not Remove from listVoxelNeighbours
            listVoxelNeighbourCheckRemove = new List<Voxel>();
            int listVoxelNeighboursCount = voxelData.listVoxelNeighbours.Count;
            for(int loopL = 0; loopL < listVoxelNeighboursCount; loopL++) {
                // Setting up
                Voxel voxelNeighbourCheck = voxelData.listVoxelNeighbours[loopL];
                bool removeVoxelNeighbour = false;
                bool removeVoxelNeighbourControl = false;

                // Check for Vertical 'Blockage', if there is a Vertical Block impeding the Movement
                if((voxelNeighbourCheck.voxelPosition.y - voxelData.voxelPosition.y == voxelHeight && voxelData._chunk.dictionaryChunkVoxels[voxelData.voxelPosition + topVoxel].isSolid) ||
                   (voxelNeighbourCheck.voxelPosition.y - voxelData.voxelPosition.y == -voxelHeight && voxelNeighbourCheck._chunk.dictionaryChunkVoxels[voxelNeighbourCheck.voxelPosition + topVoxel].isSolid)) {
                    listVoxelNeighbourCheckRemove.Add(voxelNeighbourCheck);
                    continue;
                }

                // Check for Diagonal 'Blockage', if there is a Diagonal Block impeding the Movement
                if(voxelNeighbourCheck.voxelPosition.x != voxelData.voxelPosition.x && voxelNeighbourCheck.voxelPosition.z != voxelData.voxelPosition.z) {
                    listVoxelNeighbourXCheck = new List<Voxel>();
                    listVoxelNeighbourZCheck = new List<Voxel>();

                    for(int loopM = 0; loopM < listVoxelNeighboursCount; loopM++) {
                        if(voxelData.listVoxelNeighbours[loopM].voxelPosition.x == voxelNeighbourCheck.voxelPosition.x && voxelData.listVoxelNeighbours[loopM].voxelPosition.z == voxelData.voxelPosition.z) {
                            listVoxelNeighbourXCheck.Add(voxelData.listVoxelNeighbours[loopM]);
                        }
                    }
                    for(int loopN = 0; loopN < listVoxelNeighboursCount; loopN++) {
                        if(voxelData.listVoxelNeighbours[loopN].voxelPosition.z == voxelNeighbourCheck.voxelPosition.z && voxelData.listVoxelNeighbours[loopN].voxelPosition.x == voxelData.voxelPosition.x) {
                            listVoxelNeighbourZCheck.Add(voxelData.listVoxelNeighbours[loopN]);
                        }
                    }

                    if(listVoxelNeighbourXCheck.Count > 0 && listVoxelNeighbourZCheck.Count > 0) {
                        for(int loopO = 0; loopO < listVoxelNeighbourXCheck.Count; loopO++) {
                            for(int loopP = 0; loopP < listVoxelNeighbourZCheck.Count; loopP++) {
                                Voxel voxelNeighbourXCheck = listVoxelNeighbourXCheck[loopO];
                                Voxel voxelNeighbourZCheck = listVoxelNeighbourZCheck[loopP];

                                // Check for any 'Unreachable' Neighbour for voxelNeighbourCheck
                                if(Mathf.Abs(voxelNeighbourXCheck.voxelPosition.y - voxelNeighbourCheck.voxelPosition.y) > voxelHeight ||
                                   Mathf.Abs(voxelNeighbourZCheck.voxelPosition.y - voxelNeighbourCheck.voxelPosition.y) > voxelHeight) {
                                    removeVoxelNeighbour = true;
                                } else {
                                    removeVoxelNeighbourControl = true;
                                }
                            }
                        }
                    } else {
                        // Check if voxelNeighbourXCheck or voxelNeighbourZCheck are null
                        listVoxelNeighbourCheckRemove.Add(voxelNeighbourCheck);
                        continue;
                    }
                }

                // Remove voxelNeighbour due to 'Blockage'
                if(removeVoxelNeighbour && !removeVoxelNeighbourControl) {
                    listVoxelNeighbourCheckRemove.Add(voxelNeighbourCheck);
                }
            }

            for(int loopQ = 0; loopQ < listVoxelNeighbourCheckRemove.Count; loopQ++) {
                Voxel voxelNeighbourCheckRemove = listVoxelNeighbourCheckRemove[loopQ];
                if(voxelData.listVoxelNeighbours.Contains(voxelNeighbourCheckRemove)) {
                    voxelData.listVoxelNeighbours.Remove(voxelNeighbourCheckRemove);
                    ///Debug.Log("<b>voxelNeighbourCheck " + voxelNeighbourCheckRemove.voxelPosition + "</b> has been <color=cyan><i>removed</i></color> from <b>listVoxelNeighbours " + voxelData.voxelPosition + "</b>. " +
                    ///          "A total of: <i>" + voxelData.listVoxelNeighbours.Count + " Voxels remained</i>.");
                }
            }

            ///Debug.Log("<b>listVoxelNeighbours" + voxelData.voxelPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + voxelData.listVoxelNeighbours.Count + " Voxels </i>.");
        }

        public void UpdateListVoxelNeighboursOfNeighbours(Voxel voxelData) {
            // Updating Neighbours for each Voxel of listVoxelNeighbours
            Chunk chunkData = voxelData._chunk;
            int listVoxelNeighboursCount = voxelData.listVoxelNeighbours.Count;
            for(int loopI = 0; loopI < listVoxelNeighboursCount; loopI++) {
                // For each 'NeighbourPosition'
                Voxel voxelNeighbour = voxelData.listVoxelNeighbours[loopI];
                voxelNeighbour.listVoxelNeighbours.Clear(); // Clear 'All' VoxelNeighbours form VoxelNeighbour
                int neighbourChecksLenght = _voxelUtilities.neighbourChecks.Length;
                for(int loopJ = 0; loopJ < neighbourChecksLenght; loopJ++) {
                    // Searching inside dictionaryWalkableVoxels
                    Vector3 voxelNeighbourNeighbourPosition = voxelNeighbour.voxelPosition + _voxelUtilities.neighbourChecks[loopJ];
                    Vector3 voxelNeighbourNeighbourChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(voxelNeighbourNeighbourPosition);
                    if(voxelNeighbour._chunk.chunkPosition == voxelNeighbourNeighbourChunkPosition) {
                        // Adding to listVoxelNeighbours
                        if(voxelNeighbour._chunk.dictionaryWalkableVoxels.ContainsKey(voxelNeighbourNeighbourPosition)) {
                            Voxel voxelNeighbourNeighbour = voxelNeighbour._chunk.dictionaryWalkableVoxels[voxelNeighbourNeighbourPosition];
                            voxelNeighbour.listVoxelNeighbours.Add(voxelNeighbourNeighbour);
                            ///Debug.Log(voxelNeighbour.voxelPosition + " " + voxelNeighbourNeighbour.voxelPosition);
                        }
                    } else {
                        if(WorldMapData.dictionaryChunkData.ContainsKey(voxelNeighbourNeighbourChunkPosition)) {
                            Chunk chunkNeighbourNeighbour = WorldMapData.dictionaryChunkData[voxelNeighbourNeighbourChunkPosition];
                            // Adding to listVoxelNeighbours
                            if(chunkNeighbourNeighbour.dictionaryWalkableVoxels.ContainsKey(voxelNeighbourNeighbourPosition)) {
                                Voxel voxelNeighbourNeighbour = chunkNeighbourNeighbour.dictionaryWalkableVoxels[voxelNeighbourNeighbourPosition];
                                voxelNeighbour.listVoxelNeighbours.Add(voxelNeighbourNeighbour);
                                ///Debug.Log(voxelNeighbour.voxelPosition + " " + voxelNeighbourNeighbour.voxelPosition);
                            }
                        }
                    }
                }

                // Check if voxelNeighbour has a 'Walkable' Path, if not Remove from listVoxelNeighbours
                listVoxelNeighbourCheckRemove = new List<Voxel>();
                int listVoxelNeighboursNeighboursCount = voxelNeighbour.listVoxelNeighbours.Count;
                for(int loopL = 0; loopL < listVoxelNeighboursNeighboursCount; loopL++) {
                    // Setting up
                    Voxel voxelNeighbourCheck = voxelNeighbour.listVoxelNeighbours[loopL];
                    bool removeVoxelNeighbour = false;
                    bool removeVoxelNeighbourControl = false;

                    // Check for Vertical 'Blockage', if there is a Vertical Block impeding the Movement
                    if((voxelNeighbourCheck.voxelPosition.y - voxelNeighbour.voxelPosition.y == voxelHeight && voxelNeighbour._chunk.dictionaryChunkVoxels[voxelNeighbour.voxelPosition + topVoxel].isSolid) ||
                       (voxelNeighbourCheck.voxelPosition.y - voxelNeighbour.voxelPosition.y == -voxelHeight && voxelNeighbourCheck._chunk.dictionaryChunkVoxels[voxelNeighbourCheck.voxelPosition + topVoxel].isSolid)) {
                        listVoxelNeighbourCheckRemove.Add(voxelNeighbourCheck);
                        continue;
                    }

                    // Check for Diagonal 'Blockage', if there is a Diagonal Block impeding the Movement
                    if(voxelNeighbourCheck.voxelPosition.x != voxelNeighbour.voxelPosition.x && voxelNeighbourCheck.voxelPosition.z != voxelNeighbour.voxelPosition.z) {
                        listVoxelNeighbourXCheck = new List<Voxel>();
                        listVoxelNeighbourZCheck = new List<Voxel>();

                        for(int loopM = 0; loopM < listVoxelNeighboursNeighboursCount; loopM++) {
                            if(voxelNeighbour.listVoxelNeighbours[loopM].voxelPosition.x == voxelNeighbourCheck.voxelPosition.x && voxelNeighbour.listVoxelNeighbours[loopM].voxelPosition.z == voxelNeighbour.voxelPosition.z) {
                                listVoxelNeighbourXCheck.Add(voxelNeighbour.listVoxelNeighbours[loopM]);
                            }
                        }
                        for(int loopN = 0; loopN < listVoxelNeighboursNeighboursCount; loopN++) {
                            if(voxelNeighbour.listVoxelNeighbours[loopN].voxelPosition.z == voxelNeighbourCheck.voxelPosition.z && voxelNeighbour.listVoxelNeighbours[loopN].voxelPosition.x == voxelNeighbour.voxelPosition.x) {
                                listVoxelNeighbourZCheck.Add(voxelNeighbour.listVoxelNeighbours[loopN]);
                            }
                        }

                        if(listVoxelNeighbourXCheck.Count > 0 && listVoxelNeighbourZCheck.Count > 0) {
                            for(int loopO = 0; loopO < listVoxelNeighbourXCheck.Count; loopO++) {
                                for(int loopP = 0; loopP < listVoxelNeighbourZCheck.Count; loopP++) {
                                    Voxel voxelNeighbourXCheck = listVoxelNeighbourXCheck[loopO];
                                    Voxel voxelNeighbourZCheck = listVoxelNeighbourZCheck[loopP];

                                    // Check for any 'Unreachable' Neighbour for voxelNeighbourCheck
                                    if(Mathf.Abs(voxelNeighbourXCheck.voxelPosition.y - voxelNeighbourCheck.voxelPosition.y) > voxelHeight ||
                                       Mathf.Abs(voxelNeighbourZCheck.voxelPosition.y - voxelNeighbourCheck.voxelPosition.y) > voxelHeight) {
                                        removeVoxelNeighbour = true;
                                    } else {
                                        removeVoxelNeighbourControl = true;
                                    }
                                }
                            }
                        } else {
                            // Check if voxelNeighbourXCheck or voxelNeighbourZCheck are null
                            listVoxelNeighbourCheckRemove.Add(voxelNeighbourCheck);
                            continue;
                        }
                    }

                    // Remove voxelNeighbour due to 'Blockage'
                    if(removeVoxelNeighbour && !removeVoxelNeighbourControl) {
                        listVoxelNeighbourCheckRemove.Add(voxelNeighbourCheck);
                    }
                }

                for(int loopQ = 0; loopQ < listVoxelNeighbourCheckRemove.Count; loopQ++) {
                    Voxel voxelNeighbourCheckRemove = listVoxelNeighbourCheckRemove[loopQ];
                    if(voxelNeighbour.listVoxelNeighbours.Contains(voxelNeighbourCheckRemove)) {
                        voxelNeighbour.listVoxelNeighbours.Remove(voxelNeighbourCheckRemove);
                        ///Debug.Log("<b>voxelNeighbourCheck " + voxelNeighbourCheckRemove.voxelPosition + "</b> has been <color=cyan><i>removed</i></color> from <b>listVoxelNeighbours " + voxelNeighbour.voxelPosition + "</b>. " +
                        ///          "A total of: <i>" + voxelNeighbour.listVoxelNeighbours.Count + " Voxels remained</i>.");
                    }
                }

                ///Debug.Log("<b>listVoxelNeighbours" + voxelNeighbour.voxelPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + voxelNeighbour.listVoxelNeighbours.Count + " Voxels </i>.");
            }

            // Clear listVoxelNeighbours if VoxelData isSolid or does not have a Voxel under it
            if(voxelData.isSolid || !chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + bottomVoxel].isSolid) {
                voxelData.listVoxelNeighbours.Clear();
            }
        }
    }
}