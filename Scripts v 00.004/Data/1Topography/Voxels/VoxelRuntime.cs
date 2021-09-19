using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Chunks;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelRuntime {
        /* Instances */
        VoxelMeshRuntime _voxelMeshRuntime = new VoxelMeshRuntime();
        VoxelPathfindRuntime _voxelPathfindRuntime = new VoxelPathfindRuntime();
        VoxelMeshData _voxelMeshData = new VoxelMeshData();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Storage - For Voxels */
        private static ConcurrentQueue<Voxel> queueUpdateVoxelMeshData = new ConcurrentQueue<Voxel>();
        private static ConcurrentQueue<Voxel> queueUpdateFaceVoxelMeshData = new ConcurrentQueue<Voxel>();

        private static ConcurrentQueue<Voxel> queueUpdateListWalkableVoxels = new ConcurrentQueue<Voxel>();
        private static ConcurrentQueue<Voxel> queueUpdateListVoxelNeighboursOfBaseVoxel = new ConcurrentQueue<Voxel>();
        private static ConcurrentQueue<Voxel> queueUpdateListVoxelNeighboursOfNeighbours = new ConcurrentQueue<Voxel>();

        /* Variables - For Voxels */
        private readonly Vector3 topVoxelPosition = Constants_str.topVoxelAir;

        private Voxel changeVoxel;
        private Voxel topChangeVoxel;
        private Voxel baseVoxelChange;

        /* Locks - For Methods */
        private readonly object lockUpdateVoxelMeshData = new object();
        private readonly object lockUpdateFaceVoxelMeshData = new object();

        private readonly object lockUpdateListWalkableVoxels = new object();
        private readonly object lockUpdateListVoxelNeighbours = new object();

        public void DestroyVoxel(Voxel cursorVoxel, string typeName) {
            cursorVoxel.voxelTypeName = typeName;
            cursorVoxel.baseMovementCost = WorldData.dictionaryVoxelDefinition[cursorVoxel.voxelTypeName].baseMovementCost;

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
            cursorVoxel.voxelTypeName = typeName;
            cursorVoxel.baseMovementCost = WorldData.dictionaryVoxelDefinition[cursorVoxel.voxelTypeName].baseMovementCost;

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

                        // Enqueue VoxelData to queueUpdateListWalkableVoxels
                        queueUpdateListWalkableVoxels.Enqueue(voxelData);
                        ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListWalkableVoxels</b>.");
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

        public void QueueListWalkableVoxels() {
            while(queueUpdateListWalkableVoxels.Count > 0) {
                lock(lockUpdateListWalkableVoxels) {
                    Voxel voxelData;
                    bool tryPeekTrue = queueUpdateListWalkableVoxels.TryPeek(out voxelData);
                    if(tryPeekTrue) {
                        Chunk chunkData = voxelData._chunk;
                        if(WorldData.dictionaryVoxelDefinition[voxelData.voxelTypeName].isSolid) {
                            changeVoxel = chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + topVoxelPosition];
                            voxelData.listVoxelNeighbours.Clear(); // Clear 'All' VoxelNeighbours form VoxelData
                            _voxelPathfindRuntime.RemoveVoxelFromListWalkableVoxels(voxelData, chunkData);
                            baseVoxelChange = _voxelPathfindRuntime.UpdateListWalkableVoxelsFromVoxel(changeVoxel, chunkData);
                        } else {
                            changeVoxel = voxelData;
                            topChangeVoxel = chunkData.dictionaryChunkVoxels[voxelData.voxelPosition + topVoxelPosition];
                            _voxelPathfindRuntime.RemoveVoxelFromListWalkableVoxels(topChangeVoxel, chunkData); // Remove TopVoxel from WalkableVoxels
                            baseVoxelChange = _voxelPathfindRuntime.UpdateListWalkableVoxelsFromVoxel(changeVoxel, chunkData);
                        }

                        if(baseVoxelChange != null) {
                            // Enqueue VoxelData to queueUpdateListVoxelNeighboursOfBaseVoxel
                            queueUpdateListVoxelNeighboursOfBaseVoxel.Enqueue(baseVoxelChange);
                            ///Debug.Log("<b>VoxelData " + baseVoxelChange.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListVoxelNeighboursOfbaseVoxel</b>.");
                        } else {
                            // Enqueue VoxelData to queueUpdateListVoxelNeighboursOfNeighbours
                            queueUpdateListVoxelNeighboursOfNeighbours.Enqueue(topChangeVoxel);
                            ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");
                        }

                        bool tryDequeueFalse = queueUpdateListWalkableVoxels.TryDequeue(out voxelData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueUpdateListWalkableVoxels</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueUpdateListWalkableVoxels</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateListWalkableVoxels</b>.");
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

                        // Enqueue VoxelData to queueUpdateListVoxelNeighboursOfNeighbours
                        queueUpdateListVoxelNeighboursOfNeighbours.Enqueue(voxelData);
                        ///Debug.Log("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateListVoxelNeighboursOfNeighbours</b>.");


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