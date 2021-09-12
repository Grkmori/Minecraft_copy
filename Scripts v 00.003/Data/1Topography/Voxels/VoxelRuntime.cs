using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Data.InvisibleData.Chunks;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelRuntime {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelMeshRuntime _voxelMeshRuntime = new VoxelMeshRuntime();
        VoxelMeshData _voxelMeshData = new VoxelMeshData();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Storage - For Voxels */
        private static ConcurrentQueue<Voxel> queueUpdateVoxelMeshData = new ConcurrentQueue<Voxel>();
        private static ConcurrentQueue<Voxel> queueUpdateFaceVoxelMeshData = new ConcurrentQueue<Voxel>();

        /* Locks - For Methods */
        private readonly object lockUpdateVoxelMeshData = new object();
        private readonly object lockUpdateFaceVoxelMeshData = new object();

        public void DestroyVoxel(Voxel @cursorVoxel, string @typeName) {
            @cursorVoxel.voxelTypeName = @typeName;

            queueUpdateVoxelMeshData.Enqueue(@cursorVoxel);
            ///Debug.Log("<b>Voxel " + @cursorVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateVoxelMeshData</b>.");
            Vector3 @cursorVoxelPosition = @cursorVoxel.voxelPosition;
            for(int i = 0; i < 4; i++) {
                Voxel @cursorFaceVoxel;
                if(_voxelUtilities.CheckFacesOutOfChunk(@cursorVoxelPosition + _voxelMeshData.outChunkFaceChecks[i], @cursorVoxel, out @cursorFaceVoxel)) {
                    queueUpdateFaceVoxelMeshData.Enqueue(@cursorFaceVoxel);
                    ///Debug.Log("<b>Voxel " + @cursorFaceVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateFaceVoxelMeshData</b>.");
                }
            }
        }

        public void PlaceVoxel(Voxel @cursorVoxel, string @typeName) {
            @cursorVoxel.voxelTypeName = @typeName;

            queueUpdateVoxelMeshData.Enqueue(@cursorVoxel);
            ///Debug.Log("<b>Voxel " + @cursorVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateVoxelMeshData</b>.");
            Vector3 @cursorVoxelPosition = @cursorVoxel.voxelPosition;
            for(int i = 0; i < 4; i++) {
                Voxel @cursorFaceVoxel;
                if(_voxelUtilities.CheckFacesOutOfChunk(@cursorVoxelPosition + _voxelMeshData.outChunkFaceChecks[i], @cursorVoxel, out @cursorFaceVoxel)) {
                    queueUpdateFaceVoxelMeshData.Enqueue(@cursorFaceVoxel);
                    ///Debug.Log("<b>Voxel " + @cursorFaceVoxel.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateFaceVoxelMeshData</b>.");
                }
            }
        }

        public void UpdateVoxelMeshData() {
            while(queueUpdateVoxelMeshData.Count > 0) {
                lock(lockUpdateVoxelMeshData) {
                    // Generate/Regenerate ChunkMeshData from 'this Voxel' and 'its faces Voxels' only the Drawn Voxels
                    Voxel @voxelData;
                    bool @tryPeekTrue = queueUpdateVoxelMeshData.TryPeek(out @voxelData);
                    if(@tryPeekTrue) {
                        _voxelMeshRuntime.UpdateChunkMeshDataFromVoxel(@voxelData);
                        bool @tryDequeueFalse = queueUpdateVoxelMeshData.TryDequeue(out @voxelData);
                        if(!@tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + @voxelData.voxelPosition + "</b> <color=yellow><i>failed to TryDequeue</i></color> from <b>queueUpdateVoxelMeshData</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + @voxelData.chunkPosition + "</b> <color=green><i>successfully to TryDequeue</i></color> and was <color=cyan><i>removed</i></color> <b>queueUpdateVoxelMeshData</b>.");
                        }

                        // Enqueue ChunkMesh to queueUpdateChunkMeshPosition
                        Vector3 @chunkPosition = _chunkUtilities.GetChunkPositionFromPosition(@voxelData.voxelPosition);
                        ChunkRuntime.queueUpdateChunkMeshPosition.Enqueue(@chunkPosition);
                        ///Debug.Log("<b>ChunkMesh " + @chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateChunkMeshPosition</b>.");
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateVoxelMeshData</b>.");
                    }
                }
            }
        }

        public void UpdateFaceVoxelMeshData() {
            while(queueUpdateFaceVoxelMeshData.Count > 0) {
                lock(lockUpdateFaceVoxelMeshData) {
                    // Generate/Regenerate ChunkMeshData from 'this Voxel' and 'its faces Voxels' only the Drawn Voxels
                    Voxel @voxelData;
                    bool @tryPeekTrue = queueUpdateFaceVoxelMeshData.TryPeek(out @voxelData);
                    if(@tryPeekTrue) {
                        _voxelMeshRuntime.UpdateChunkMeshDataFromFaceVoxel(@voxelData);
                        bool @tryDequeueFalse = queueUpdateFaceVoxelMeshData.TryDequeue(out @voxelData);
                        if(!@tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + @voxelData.voxelPosition + "</b> <color=yellow><i>failed to TryDequeue</i></color> from <b>queueUpdateFaceVoxelMeshData</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + @voxelData.chunkPosition + "</b> <color=green><i>successfully to TryDequeue</i></color> and was <color=cyan><i>removed</i></color> <b>queueUpdateFaceVoxelMeshData</b>.");
                        }

                        // Enqueue ChunkMesh to queueUpdateChunkMeshPosition
                        Vector3 @chunkPosition = _chunkUtilities.GetChunkPositionFromPosition(@voxelData.voxelPosition);
                        ChunkRuntime.queueUpdateChunkMeshPosition.Enqueue(@chunkPosition);
                        ///Debug.Log("<b>ChunkMesh " + @chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateChunkMeshPosition</b>.");
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateFaceVoxelMeshData</b>.");
                    }
                }
            }
        }
    }
}