using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap.Topography.Chunks;

namespace BlueBird.World.WorldMap.Topography.Voxels {
    public sealed class VoxelRuntime {
        /* Instances */
        VoxelMeshRuntime _voxelMeshRuntime = new VoxelMeshRuntime();
        VoxelPathfindRuntime _voxelPathfindRuntime = new VoxelPathfindRuntime();
        VoxelMeshData _voxelMeshData = new VoxelMeshData();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Instances - For Voxels */
        Voxel changeVoxel;
        Voxel topChangeVoxel;
        Voxel baseVoxelChange;

        /* Storage - For Voxels */
        private static ConcurrentQueue<Voxel> queueUpdateVoxelMeshData = new ConcurrentQueue<Voxel>();
        private static ConcurrentQueue<Voxel> queueUpdateFaceVoxelMeshData = new ConcurrentQueue<Voxel>();

        private static ConcurrentQueue<Voxel> queueUpdateDictionaryWalkableVoxels = new ConcurrentQueue<Voxel>();
        private static ConcurrentQueue<Voxel> queueUpdateListVoxelNeighboursOfBaseVoxel = new ConcurrentQueue<Voxel>();
        private static ConcurrentQueue<Voxel> queueUpdateListVoxelNeighboursOfNeighbours = new ConcurrentQueue<Voxel>();

        /* Variables - For Voxels */
        private readonly Vector3 topVoxel = Constants_str.topVoxelPosition;

        /* Locks - For Queues */
        private readonly object lockUpdateVoxelMeshData = new object();
        private readonly object lockUpdateFaceVoxelMeshData = new object();

        private readonly object lockUpdateDictionaryWalkableVoxels = new object();
        private readonly object lockUpdateListVoxelNeighbours = new object();

        private void UpdateVoxelInformation(Voxel @voxelData, string typeName) {
            voxelData.voxelTypeName = typeName;
            voxelData.isSolid = WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].isSolid;
            voxelData.isTransparent = WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].isTransparent;
            voxelData.isWalkable = WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].isWalkable;
            voxelData.baseMovementCost = WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].baseMovementCost;
        }

        public void DestroyVoxel(Voxel cursorVoxel, string typeName) {
            // Updating cursorVoxel Information
            UpdateVoxelInformation(cursorVoxel, typeName);

            queueUpdateVoxelMeshData.Enqueue(cursorVoxel);
            ///Debug.Log("<b>Voxel " + cursorVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateVoxelMeshData</b>.");
            Vector3 cursorVoxelPosition = cursorVoxel.voxelPosition;
            for(int i = 0; i < 4; i++) {
                Voxel cursorFaceVoxel;
                if(_voxelUtilities.CheckFacesOutOfChunk(cursorVoxelPosition + _voxelMeshData.outChunkFaceChecks[i], cursorVoxel, out cursorFaceVoxel)) {
                    queueUpdateFaceVoxelMeshData.Enqueue(cursorFaceVoxel);
                    ///Debug.Log("<b>Voxel " + cursorFaceVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateFaceVoxelMeshData</b>.");
                }
            }
        }

        public void PlaceVoxel(Voxel cursorVoxel, string typeName) {
            // Updating cursorVoxel Information
            UpdateVoxelInformation(cursorVoxel, typeName);

            queueUpdateVoxelMeshData.Enqueue(cursorVoxel);
            ///Debug.Log("<b>Voxel " + cursorVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateVoxelMeshData</b>.");
            Vector3 cursorVoxelPosition = cursorVoxel.voxelPosition;
            for(int i = 0; i < 4; i++) {
                Voxel cursorFaceVoxel;
                if(_voxelUtilities.CheckFacesOutOfChunk(cursorVoxelPosition + _voxelMeshData.outChunkFaceChecks[i], cursorVoxel, out cursorFaceVoxel)) {
                    queueUpdateFaceVoxelMeshData.Enqueue(cursorFaceVoxel);
                    ///Debug.Log("<b>Voxel " + cursorFaceVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateFaceVoxelMeshData</b>.");
                }
            }
        }

        public void QueueVoxelMeshData() {
            while(queueUpdateVoxelMeshData.Count > 0) {
                lock(lockUpdateVoxelMeshData) {
                    // Generate/Regenerate ChunkMeshData from 'this Voxel' and 'its faces Voxels' only the Drawn Voxels
                    Voxel voxelData;
                    bool tryPeekTrue = queueUpdateVoxelMeshData.TryPeek(out voxelData);
                    if(tryPeekTrue) {
                        _voxelMeshRuntime.UpdateChunkMeshDataFromVoxel(voxelData);

                        bool tryDequeueFalse = queueUpdateVoxelMeshData.TryDequeue(out voxelData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueUpdateVoxelMeshData</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + voxelData.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueUpdateVoxelMeshData</b>.");
                        }

                        // Enqueue ChunkMesh to queueUpdateChunkMeshPosition
                        Vector3 chunkPosition = voxelData._chunk.chunkPosition;
                        ChunkRuntime.queueUpdateChunkMeshPosition.Enqueue(chunkPosition);
                        ///Debug.Log("<b>ChunkMesh " + chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateChunkMeshPosition</b>.");

                        // Enqueue VoxelData to queueUpdateDictionaryWalkableVoxels
                        queueUpdateDictionaryWalkableVoxels.Enqueue(voxelData);
                        ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateDictionaryWalkableVoxels</b>.");
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateVoxelMeshData</b>.");
                    }
                }
            }
        }

        public void QueueFaceVoxelMeshData() {
            while(queueUpdateFaceVoxelMeshData.Count > 0) {
                lock(lockUpdateFaceVoxelMeshData) {
                    // Generate/Regenerate ChunkMeshData from 'this Voxel' and 'its faces Voxels' only the Drawn Voxels
                    Voxel voxelData;
                    bool tryPeekTrue = queueUpdateFaceVoxelMeshData.TryPeek(out voxelData);
                    if(tryPeekTrue) {
                        _voxelMeshRuntime.UpdateChunkMeshDataFromFaceVoxel(voxelData);

                        bool tryDequeueFalse = queueUpdateFaceVoxelMeshData.TryDequeue(out voxelData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueUpdateFaceVoxelMeshData</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + voxelData.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueUpdateFaceVoxelMeshData</b>.");
                        }

                        // Enqueue ChunkMesh to queueUpdateChunkMeshPosition
                        Vector3 chunkPosition = voxelData._chunk.chunkPosition;
                        ChunkRuntime.queueUpdateChunkMeshPosition.Enqueue(chunkPosition);
                        ///Debug.Log("<b>ChunkMesh " + chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateChunkMeshPosition</b>.");
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateFaceVoxelMeshData</b>.");
                    }
                }
            }
        }

        public void QueueDictionaryWalkableVoxels() {
            while(queueUpdateDictionaryWalkableVoxels.Count > 0) {
                lock(lockUpdateDictionaryWalkableVoxels) {
                    Voxel voxelData;
                    bool destroyVoxel; // if false is 'placeVoxel'
                    bool tryPeekTrue = queueUpdateDictionaryWalkableVoxels.TryPeek(out voxelData);
                    if(tryPeekTrue) {
                        Chunk chunkData = voxelData._chunk;
                        if(WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].isSolid) {
                            changeVoxel = chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + topVoxel];
                            //voxelData.listVoxelNeighbours.Clear(); // Clear 'All' VoxelNeighbours form VoxelData
                            _voxelPathfindRuntime.RemoveVoxelFromDictionaryWalkableVoxels(voxelData, chunkData);
                            baseVoxelChange = _voxelPathfindRuntime.UpdateDictionaryWalkableVoxelsFromVoxel(changeVoxel, chunkData);
                            destroyVoxel = true;
                        } else {
                            changeVoxel = voxelData;
                            topChangeVoxel = chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + topVoxel];
                            _voxelPathfindRuntime.RemoveVoxelFromDictionaryWalkableVoxels(topChangeVoxel, chunkData); // Remove TopVoxel from WalkableVoxels
                            baseVoxelChange = _voxelPathfindRuntime.UpdateDictionaryWalkableVoxelsFromVoxel(changeVoxel, chunkData);
                            destroyVoxel = false;
                        }

                        if(baseVoxelChange != null) {
                            // Enqueue VoxelData to queueUpdateListVoxelNeighboursOfBaseVoxel
                            queueUpdateListVoxelNeighboursOfBaseVoxel.Enqueue(baseVoxelChange);
                            ///Debug.Log("<b>VoxelData " + baseVoxelChange.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListVoxelNeighboursOfbaseVoxel</b>.");

                            // Enqueue VoxelData to queueUpdateListVoxelNeighboursOfNeighbours
                            if(destroyVoxel) {
                                queueUpdateListVoxelNeighboursOfNeighbours.Enqueue(voxelData);
                                ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");
                            } else {
                                queueUpdateListVoxelNeighboursOfNeighbours.Enqueue(topChangeVoxel);
                                ///Debug.Log("<b>VoxelData " + topChangeVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");
                            }
                        } else {
                            // Enqueue VoxelData to queueUpdateListVoxelNeighboursOfNeighbours
                            queueUpdateListVoxelNeighboursOfNeighbours.Enqueue(topChangeVoxel);
                            ///Debug.Log("<b>VoxelData " + topChangeVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");
                        }

                        bool tryDequeueFalse = queueUpdateDictionaryWalkableVoxels.TryDequeue(out voxelData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueUpdateDictionaryWalkableVoxels</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueUpdateDictionaryWalkableVoxels</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateDictionaryWalkableVoxels</b>.");
                    }
                }
            }
        }

        public void QueueListVoxelNeighboursOfBaseVoxel() {
            while(queueUpdateListVoxelNeighboursOfBaseVoxel.Count > 0) {
                lock(lockUpdateListVoxelNeighbours) {
                    Voxel voxelData;
                    bool tryPeekTrue = queueUpdateListVoxelNeighboursOfBaseVoxel.TryPeek(out voxelData);
                    if(tryPeekTrue) {
                        _voxelPathfindRuntime.UpdateListVoxelNeighboursOfBaseVoxel(voxelData);

                        bool tryDequeueFalse = queueUpdateListVoxelNeighboursOfBaseVoxel.TryDequeue(out voxelData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueUpdateListVoxelNeighboursOfBaseVoxel</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueUpdateListVoxelNeighboursOfBaseVoxel</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateListVoxelNeighboursOfBaseVoxel</b>.");
                    }
                }
            }
        }

        public void QueueListVoxelNeighboursOfNeighbours() {
            while(queueUpdateListVoxelNeighboursOfNeighbours.Count > 0) {
                lock(lockUpdateListVoxelNeighbours) {
                    Voxel voxelData;
                    bool tryPeekTrue = queueUpdateListVoxelNeighboursOfNeighbours.TryPeek(out voxelData);
                    if(tryPeekTrue) {
                        _voxelPathfindRuntime.UpdateListVoxelNeighboursOfNeighbours(voxelData);

                        bool tryDequeueFalse = queueUpdateListVoxelNeighboursOfNeighbours.TryDequeue(out voxelData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");
                    }
                }
            }
        }
    }
}