using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Data.Topography.Chunks {
    public sealed class ChunkGenerator {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelGenerator _voxelGenerator = new VoxelGenerator();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        Chunk chunkNeighbour;
        Voxel voxelNeighbour;

        /* Storage - For Chunks */
        public static ConcurrentQueue<Chunk> queuePopulateListWalkableVoxels = new ConcurrentQueue<Chunk>();
        public static ConcurrentQueue<Chunk> queuePopulateListVoxelNeighbours = new ConcurrentQueue<Chunk>();

        /* Variables - For Chunks */
        private int chunkCount;
        private Vector3 position;
        private readonly float chunkWidth = Constants_str.chunkSize.x;
        private readonly float chunkHeight = Constants_str.worldBaseVector3.y;

        /* Variables - For Voxels */
        private int voxelCount;
        private int totalVoxelsChunksDictionaries = 0;
        private bool voxelTotalNumberError = false;

        private readonly Vector3 baseVoxel = Constants_str.baseVoxelTerrain;

        private readonly float voxelHeight = Constants_str.voxelSize.y;
        private Vector3 voxelNeighbourPosition;
        private Vector3 voxelNeighbourChunkPosition;
        private float posX;
        private float posZ;
        private int loopI;
        private int loopJ;
        private int loopK;
        private int listWalkableVoxelsCount;
        private int listNeighbourWalkableVoxelCount;

        public static bool completePopulateListWalkableVoxels = false;
        public static bool completePopulateListVoxelNeighbours = false;

        /* Locks - For Methods */
        private readonly object lockPopulateListWalkableVoxels = new object();
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
            bool tryAddFalseObject = WorldData.dictionaryChunkObject.TryAdd(position, chunkObject);
            if(!tryAddFalseObject) {
                Debug.LogError("<b>" + chunkObject.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkObject</b>.");
            }
            // Adding ChunkData to the dictionaryChunkData
            bool tryAddFalseData = WorldData.dictionaryChunkData.TryAdd(position, chunkData);
            if(!tryAddFalseData) {
                Debug.LogError("<b>Chunk " + chunkData.chunkPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkData</b>.");
            }
        }

        public void CreateChunksVoxels(Vector3 position, Vector2 voxelSize) {
            // Use World Region2DChunks to Create Chunks Voxels for each 'position' of the World
            voxelCount = 0;

            Chunk chunkData = WorldData.dictionaryChunkData[position];
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
            if(WorldData.dictionaryChunkObject.Count < (worldSize.x) * (worldSize.y)) {
                Debug.LogWarning("<b>dictionaryChunkObject</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Chunks Objects</b> were <color=green><i>generated</i></color>. " +
                          "A total of: <i>" + (worldSize.x) * (worldSize.y) + " ChunkObjects</i>. <b>dictionaryChunkObject</b> was <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryChunkObject.Count + " ChunkObjects</i>.");
            }

            // Check for Success on dictionaryChunkData
            if(WorldData.dictionaryChunkData.Count < worldSize.x * worldSize.y) {
                Debug.LogWarning("<b>dictionaryChunkData</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Chunks Data</b> were <color=green><i>generated</i></color>. A total of: <i>" + worldSize.x * worldSize.y + " ChunkData</i>. " +
                          "<b>dictionaryChunkData</b> was <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryChunkData.Count + " ChunkDatas</i>.");
            }

            // Final check if Voxels were successfully Added to all dictionaryChunkVoxels
            foreach(Vector2 chunk2DPosition in worldChunks) {
                position = new Vector3(chunk2DPosition.x, baseVector.y, chunk2DPosition.y);
                Chunk chunkDataCount = WorldData.dictionaryChunkData[position];
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

        public void PopulateListWalkableVoxels() {
            while(queuePopulateListWalkableVoxels.Count > 0) {
                lock(lockPopulateListWalkableVoxels) {
                    Chunk chunkData;
                    bool tryPeekTrue = queuePopulateListWalkableVoxels.TryPeek(out chunkData);
                    if(tryPeekTrue) {
                        // Search for Walkable Voxels: 'voxel'.isSolid = false; 'baseVoxel'.isSolid = true
                        foreach(Vector3 voxelPosition in chunkData.dictionaryChunkVoxels.Keys) {
                            if(!WorldData.dictionaryVoxelDefinition[chunkData.dictionaryChunkVoxels[voxelPosition].voxelTypeName].isSolid) {
                                if(WorldData.dictionaryVoxelDefinition[chunkData.dictionaryChunkVoxels[voxelPosition + baseVoxel].voxelTypeName].isSolid) {
                                    chunkData.listWalkableVoxels.Add(chunkData.dictionaryChunkVoxels[voxelPosition]);
                                }
                            }
                        }

                        // Enqueue ChunkData to queuePopulateListVoxelNeighbours
                        queuePopulateListVoxelNeighbours.Enqueue(chunkData);
                        ///Debug.Log("<b>ChunData " + chunkData.chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queuePopulateListVoxelNeighbours</b>.");

                        // Checking if listWalkableVoxels was Populated
                        if(chunkData.listWalkableVoxels.Count < chunkWidth * chunkWidth) { // Half Baked Way of Checking if this List was fully Populated
                            Debug.LogWarning("<b>Adding Voxels</b> to the  <b>listWalkableVoxels</b> had some problems and <color=yellow><i>was not fully populated</i></color>. " +
                                             "A total of: <i>" + chunkData.listWalkableVoxels.Count + " Voxels </i> were added to the <b>Chunk listWalkableVoxels</b>.");
                        } else {
                            ///Debug.Log("<b>listWalkableVoxels" + chunkData.chunkPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + chunkData.listWalkableVoxels.Count + " Voxels </i>.");
                        }

                        bool tryDequeueFalse = queuePopulateListWalkableVoxels.TryDequeue(out chunkData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>ChunkData " + chunkData.chunkPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queuePopulateListWalkableVoxels</b>.");
                        } else {
                            ///Debug.Log("<b>ChunkData " + chunkData.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queuePopulateListWalkableVoxels</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>ChunkData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queuePopulateListWalkableVoxels</b>.");
                    }
                }
            }

            completePopulateListWalkableVoxels = true;
        }

        public void PopulateListVoxelNeighbours() {
            while(queuePopulateListVoxelNeighbours.Count > 0) {
                lock(lockPopulateListVoxelNeighbours) {
                    Chunk chunkData;
                    bool tryPeekTrue = queuePopulateListVoxelNeighbours.TryPeek(out chunkData);
                    if(tryPeekTrue) {
                        // Adding Neighbours for each Voxel of listWalkableVoxels
                        listWalkableVoxelsCount = chunkData.listWalkableVoxels.Count;
                        for(loopI = 0; loopI < listWalkableVoxelsCount; loopI++) {
                            // For each 'NeighbourPosition'
                            Voxel voxelData = chunkData.listWalkableVoxels[loopI];
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
                                                voxelData.listVoxelNeighbours.Add(voxelNeighbour);
                                            }
                                        }
                                    }
                                }
                            }

                            ///Debug.Log("<b>listVoxelNeighbours" + voxelData.voxelPosition + "</b> was <color=cyan><i>populated</i></color>. A total of: <i>" + voxelData.listVoxelNeighbours.Count + " Voxels </i>.");
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