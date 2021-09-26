using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap.Topography.Voxels;

namespace BlueBird.World.WorldMap.Topography.Chunks {
    public sealed class ChunkGenerator {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelGenerator _voxelGenerator = new VoxelGenerator();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Storage - For Chunks */
        public static ConcurrentQueue<Chunk> queuePopulateDictionaryWalkableVoxels = new ConcurrentQueue<Chunk>();
        public static ConcurrentQueue<Chunk> queuePopulateListVoxelNeighbours = new ConcurrentQueue<Chunk>();

        /* Storage - For Voxels */
        List<Voxel> listVoxelNeighbourXCheck;
        List<Voxel> listVoxelNeighbourZCheck;
        List<Voxel> listVoxelNeighbourCheckRemove;

        /* Variables - For Chunks */
        private int chunkCount;
        private Vector3 position;
        private readonly float chunkWidth = Constants_str.chunkSize.x;

        /* Variables - For Voxels */
        private int voxelCount;
        private int totalVoxelsChunksDictionaries = 0;
        private bool voxelTotalNumberError = false;

        private readonly Vector3 bottomVoxel = Constants_str.bottomVoxelPosition;

        private readonly Vector3 topVoxel = Constants_str.topVoxelPosition;
        private readonly float voxelHeight = Constants_str.voxelSize.y;

        public static bool completePopulateDictionaryWalkableVoxels = false;
        public static bool completePopulateListVoxelNeighbours = false;

        /* Locks - For Methods */
        private readonly object lockPopulateDictionaryWalkableVoxels = new object();
        private readonly object lockPopulateListVoxelNeighbours = new object();

        public void SetupChunkGameObject(GameObject chunkObject, Vector3 position, Vector2 chunkSize, Vector2 voxelSize, out Chunk chunkData) {
            // Setting up
            chunkCount++;

            // Setup Chunk and Create Region2D and Region3D for ChunkData
            chunkObject.name = "Chunk " + position;
            Region2D region2D = new Region2D(new Vector2(chunkObject.transform.position.x, chunkObject.transform.position.z), chunkSize.x, voxelSize.x);
            Region3D region3D = new Region3D(chunkObject.transform.position, chunkSize, voxelSize);
            chunkData = chunkObject.GetComponent<Chunk>();
            chunkData.chunkPosition = position;
            chunkData.chunkObjectPosition = chunkObject.transform.localPosition;
            chunkData._chunkBounds = new Bounds(new Vector3(chunkData.chunkObjectPosition.x, chunkSize.y / 2, chunkData.chunkObjectPosition.y), new Vector3(chunkSize.x + voxelSize.x, chunkSize.y + voxelSize.y, chunkSize.x + voxelSize.x));
            chunkData._region2DQuads = region2D;
            chunkData._region3DVoxels = region3D;
            chunkData.SetVisible(false);
            ///Debug.Log("<b>Chunk Data " + chunkData.chunkPosition + "</b> was successfully <color=green><i>generated</i></color>. " +
            ///          "Size: <i>" + region2D.regionSize + " Quads</i>; Size: <i>" + region3D.regionSize + " Voxels</i>; " +
            ///          "CenterVector2: <i>" + chunkData._chunkBounds.center + "</i>; MinVector2: <i>" + chunkData._chunkBounds.min + "</i>; MaxVector2: <i>" + chunkData._chunkBounds.max + "</i>.");

            // Adding ChunkObject to the dictionaryChunkObject
            bool tryAddFalseObject = WorldMapData.dictionaryChunkObject.TryAdd(position, chunkObject);
            if(!tryAddFalseObject) {
                Debug.LogError("<b>" + chunkObject.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkObject</b>.");
            }
            // Adding ChunkData to the dictionaryChunkData
            bool tryAddFalseData = WorldMapData.dictionaryChunkData.TryAdd(position, chunkData);
            if(!tryAddFalseData) {
                Debug.LogError("<b>Chunk " + chunkData.chunkPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkData</b>.");
            }
        }

        public void CreateChunksVoxels(Vector3 position, Vector2 voxelSize) {
            // Use World Region2DChunks to Create Chunks Voxels for each 'position' of the World
            voxelCount = 0;

            Chunk chunkData = WorldMapData.dictionaryChunkData[position];
            Region3D chunkRegion3D = chunkData._region3DVoxels;

            // Use Region3D to Create Voxels for each 'position' of the Chunk
            foreach(Vector3 voxelPosition in chunkRegion3D) {
                _voxelGenerator.CreateVoxelData(voxelPosition, chunkData, voxelSize);
                voxelCount++;
            }
            if(chunkData.dictionaryChunkVoxels.Count < voxelCount) {
                Debug.LogWarning("<b>dictionaryChunkVoxels</b> from <b>Chunk " + chunkData.chunkPosition + "</b>  had some problems and <color=yellow><i>was not fully populated</i></color>.");
                voxelTotalNumberError = true;
            } else {
                ///Debug.Log("<b>Voxels</b> from <b>Chunk " + chunkData.chunkPosition + "</b>  were <color=green><i>generated</i></color>. " +
                ///          "A total of: <i>" + voxelCount + "</i> Voxels. <b>dictionaryChunkVoxels</b> was <color=cyan><i>populated</i></color> with a total of <i>" + chunkData.dictionaryChunkVoxels.Count + " Voxels</i>.");
            }
        }

        public void ChunksFinalCheck(Region2D worldChunks, Vector3 baseVector, Vector2 worldSize) {
            // Check for Success on dictionaryChunkObject
            if(WorldMapData.dictionaryChunkObject.Count < (worldSize.x) * (worldSize.y)) {
                Debug.LogWarning("<b>dictionaryChunkObject</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Chunks Objects</b> were <color=green><i>generated</i></color>. " +
                          "A total of: <i>" + (worldSize.x) * (worldSize.y) + " ChunkObjects</i>. <b>dictionaryChunkObject</b> was <color=cyan><i>populated</i></color> with a total of <i>" + WorldMapData.dictionaryChunkObject.Count + " ChunkObjects</i>.");
            }

            // Check for Success on dictionaryChunkData
            if(WorldMapData.dictionaryChunkData.Count < worldSize.x * worldSize.y) {
                Debug.LogWarning("<b>dictionaryChunkData</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Chunks Data</b> were <color=green><i>generated</i></color>. A total of: <i>" + worldSize.x * worldSize.y + " ChunkData</i>. " +
                          "<b>dictionaryChunkData</b> was <color=cyan><i>populated</i></color> with a total of <i>" + WorldMapData.dictionaryChunkData.Count + " ChunkDatas</i>.");
            }

            // Final check if Voxels were successfully Added to all dictionaryChunkVoxels
            foreach(Vector2 chunk2DPosition in worldChunks) {
                position = new Vector3(chunk2DPosition.x, baseVector.y, chunk2DPosition.y);
                Chunk chunkDataCount = WorldMapData.dictionaryChunkData[position];
                totalVoxelsChunksDictionaries += chunkDataCount.dictionaryChunkVoxels.Count;
            }
            if(voxelTotalNumberError) {
                Debug.LogWarning("<b>Generating Voxels</b> had some problems and <color=yellow><i>were not fully generated</i></color>. " +
                                 "A total of: <i>" + voxelCount * chunkCount + " Voxels </i> were expected but only: <i>" + totalVoxelsChunksDictionaries + " Voxels</i> were added to the <b>Chunk Dictionaries dictionaryChunkVoxels</b>.");
            } else {
                Debug.Log("<b>Generating Voxels</b> was <color=green><i>successful</i></color>. " +
                          "A total of: <i>" + voxelCount * chunkCount + " Voxels </i>. <b>Chunk Voxels Dictionaries</b> were <color=cyan><i>populated</i></color> with a total of <i>" + totalVoxelsChunksDictionaries + " Voxels</i>.");
            }
        }

        public void PopulateDictionaryWalkableVoxels() {
            while(queuePopulateDictionaryWalkableVoxels.Count > 0) {
                lock(lockPopulateDictionaryWalkableVoxels) {
                    Chunk chunkData;
                    bool tryPeekTrue = queuePopulateDictionaryWalkableVoxels.TryPeek(out chunkData);
                    if(tryPeekTrue) {
                        // Search for Walkable Voxels: 'voxel'.isWalkable = true; 'baseVoxel'.isSolid = true
                        foreach(Vector3 voxelPosition in chunkData.dictionaryChunkVoxels.Keys) {
                            if(chunkData.dictionaryChunkVoxels[voxelPosition].isWalkable) {
                                if(chunkData.dictionaryChunkVoxels[voxelPosition + bottomVoxel].isSolid) {
                                    chunkData.dictionaryWalkableVoxels.Add(voxelPosition, chunkData.dictionaryChunkVoxels[voxelPosition]);
                                }
                            }
                        }

                        // Enqueue ChunkData to queuePopulateDictionaryVoxelNeighbours
                        queuePopulateListVoxelNeighbours.Enqueue(chunkData);
                        ///Debug.Log("<b>ChunData " + chunkData.chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queuePopulateDictionaryVoxelNeighbours</b>.");

                        // Checking if dictionaryWalkableVoxels was Populated
                        if(chunkData.dictionaryWalkableVoxels.Count < chunkWidth * chunkWidth) { // Half Baked Way of Checking if this Dictionary was fully Populated
                            Debug.LogWarning("<b>Adding Voxels</b> to the  <b>dictionaryWalkableVoxels</b> had some problems and <color=yellow><i>was not fully populated</i></color>. " +
                                             "A total of: <i>" + chunkData.dictionaryWalkableVoxels.Count + " Voxels </i> were added to the <b>Chunk dictionaryWalkableVoxels</b>.");
                        } else {
                            ///Debug.Log("<b>dictionaryWalkableVoxels" + chunkData.chunkPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + chunkData.dictionaryWalkableVoxels.Count + " Voxels </i>.");
                        }

                        bool tryDequeueFalse = queuePopulateDictionaryWalkableVoxels.TryDequeue(out chunkData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>ChunkData " + chunkData.chunkPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queuePopulateDictionaryWalkableVoxels</b>.");
                        } else {
                            ///Debug.Log("<b>ChunkData " + chunkData.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queuePopulateDictionaryWalkableVoxels</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>ChunkData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queuePopulateDictionaryWalkableVoxels</b>.");
                    }
                }
            }

            completePopulateDictionaryWalkableVoxels = true;
        }

        public void PopulateListVoxelNeighbours() {
            while(queuePopulateListVoxelNeighbours.Count > 0) {
                lock(lockPopulateListVoxelNeighbours) {
                    Chunk chunkData;
                    bool tryPeekTrue = queuePopulateListVoxelNeighbours.TryPeek(out chunkData);
                    if(tryPeekTrue) {
                        // Adding Neighbours for each Voxel of dictionaryWalkableVoxels
                        foreach(Vector3 voxelPosition in chunkData.dictionaryWalkableVoxels.Keys) {
                            // For each 'NeighbourPosition'
                            Voxel voxelData = chunkData.dictionaryWalkableVoxels[voxelPosition];
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

                            ///Debug.Log("<b>listVoxelNeighbours " + voxelData.voxelPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + voxelData.listVoxelNeighbours.Count + " Voxels </i>.");
                        }

                        bool tryDequeueFalse = queuePopulateListVoxelNeighbours.TryDequeue(out chunkData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>ChunkData " + chunkData.chunkPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queuePopulateListVoxelNeighbours</b>.");
                        } else {
                            ///Debug.Log("<b>ChunkData " + chunkData.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queuePopulateListVoxelNeighbours</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>ChunkData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queuePopulateListVoxelNeighbours</b>.");
                    }
                }
            }

            completePopulateListVoxelNeighbours = true;
        }
    }
}