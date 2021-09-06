using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Player;

namespace BlueBird.World.Visual {
    public sealed class VisualRuntimeChunk {
        /* Instances */
        Transform playerTransform;

        VisualGenerator _visualGenerator = new VisualGenerator();

        ChunkMeshGenerator _chunkMeshGenerator = new ChunkMeshGenerator();

        /* Storage - For Chunks */
        private List<Vector3> listVisibleChunks = new List<Vector3>();
        private ConcurrentQueue<Chunk> queueChunkMeshDataUpdate = new ConcurrentQueue<Chunk>();
        private ConcurrentQueue<Vector3> queueChunkMeshPositionUpdate = new ConcurrentQueue<Vector3>();

        /* Variables - For Chunks */
        private readonly Vector3 baseVector = Constants_str.worldBaseVector3;
        private readonly float widthNormalized = Constants_str.chunkSize.x;
        private readonly float visibleDistanceChunk = Constants_str.viewDistanceChunk;
        private Vector3 playerChunkPosition = Vector3.zero;
        private Vector3 lastPlayerChunkPosition = Vector3.one;

        /* Locks - For Methods */
        private readonly object lockUpdateChunkMeshData = new object();

        public bool onTickUpdateVisibleChunks = true;

        public void ManualStart() {
            VisualEventHandler.eventOnVisualTick_1 += UpdateVisibleChunks;
            playerTransform = PlayerData.dictionaryPlayerObject["Character"].transform;
        }

        public void UpdateVisibleChunks() {
            // Setting up
            playerChunkPosition.x = Mathf.RoundToInt(playerTransform.position.x / widthNormalized);
            playerChunkPosition.y = baseVector.y;
            playerChunkPosition.z = Mathf.RoundToInt(playerTransform.position.z / widthNormalized);

            // Checking if Player has Moved
            if(playerChunkPosition.Equals(lastPlayerChunkPosition)) {
                return;
            } else {
                // Setting up
                lastPlayerChunkPosition = playerChunkPosition;
                List<Vector3> @listLastVisibleChunks = new List<Vector3>(listVisibleChunks);
                listVisibleChunks.Clear();

                // Setting to Visible Chunks in View Distance
                for(float OffsetX = lastPlayerChunkPosition.x - visibleDistanceChunk; OffsetX <= lastPlayerChunkPosition.x + visibleDistanceChunk; OffsetX++) {
                    for(float OffsetZ = lastPlayerChunkPosition.z - visibleDistanceChunk; OffsetZ <= lastPlayerChunkPosition.z + visibleDistanceChunk; OffsetZ++) {
                        Vector3 @viewDistanceChunkPosition = new Vector3(OffsetX, baseVector.y, OffsetZ);
                        if(!WorldData.dictionaryChunkData.ContainsKey(@viewDistanceChunkPosition)) {
                            continue;
                        } else {
                            listVisibleChunks.Add(@viewDistanceChunkPosition);
                            WorldData.dictionaryChunkData[@viewDistanceChunkPosition].SetVisible(true);

                            // Enqueue ChunkMeshData to queueChunkMeshDataUpdate if the ChunkMesh does not exists
                            if(VisualData.dictionaryChunkMesh.ContainsKey(@viewDistanceChunkPosition)) {
                                continue;
                            } else {
                                queueChunkMeshDataUpdate.Enqueue(WorldData.dictionaryChunkData[@viewDistanceChunkPosition]);
                                ///Debug.Log("<b>ChunkData " + @viewDistanceChunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueChunkMeshDataUpdate</b>.");
                            }
                        }
                    }
                }

                // Checking if Chunks in LastVisible are out of range and Setting to Invisible
                foreach(Vector3 @vector in @listLastVisibleChunks) {
                    if(listVisibleChunks.Contains(@vector)) {
                        continue;
                    } else {
                        WorldData.dictionaryChunkData[@vector].SetVisible(false);
                    }
                }
                @listLastVisibleChunks.Clear();
            }
        }

        public void UpdateChunkMeshData() {
            while(queueChunkMeshDataUpdate.Count > 0) {
                lock(lockUpdateChunkMeshData) {
                    // Generate/Regenerate ChunkMeshData for ChunkData
                    Chunk @chunkData;
                    bool @tryPeekTrue = queueChunkMeshDataUpdate.TryPeek(out @chunkData);
                    if(@tryPeekTrue) {
                        _chunkMeshGenerator.GenerateChunkMeshData(@chunkData);
                        bool @tryDequeueFalse = queueChunkMeshDataUpdate.TryDequeue(out @chunkData);
                        if(!@tryDequeueFalse) {
                            Debug.LogWarning("<b>ChunkData " + @chunkData.chunkPosition + "</b> <color=yellow><i>failed to TryDequeue</i></color> from <b>queueChunkMeshDataUpdate</b>.");
                        } else {
                            ///Debug.Log("<b>ChunkData " + @chunkData.chunkPosition + "</b> <color=green><i>successfully to TryDequeue</i></color> and was <color=cyan><i>removed</i></color> <b>queueChunkMeshUpdate</b>.");
                        }

                        // Enqueue ChunkMesh to queueChunkMeshUpdate
                        queueChunkMeshPositionUpdate.Enqueue(@chunkData.chunkPosition);
                        ///Debug.Log("<b>ChunkMesh " + @chunkData.chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueChunkMeshDataUpdate</b>.");
                    } else {
                        Debug.LogWarning("<b>ChunkData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueChunkMeshDataUpdate</b>.");
                    }
                }
            }
        }

        public void UpdateChunkMesh() {
            while(queueChunkMeshPositionUpdate.Count > 0) {
                // Generate/Regenerate ChunkMeshData for ChunkData
                Vector3 @chunkMeshPosition;
                bool @tryPeekTrue = queueChunkMeshPositionUpdate.TryPeek(out @chunkMeshPosition);
                if(@tryPeekTrue) {
                    _chunkMeshGenerator.CreateChunkMesh(WorldData.dictionaryChunkObject[@chunkMeshPosition], WorldData.dictionaryChunkData[@chunkMeshPosition]);
                    bool @tryDequeueFalse = queueChunkMeshPositionUpdate.TryDequeue(out @chunkMeshPosition);
                    if(!@tryDequeueFalse) {
                        Debug.LogWarning("<b>ChunkMeshPosition " + @chunkMeshPosition + "</b> <color=yellow><i>failed to TryDequeue</i></color> from <b>queueChunkMeshPositionUpdate</b>.");
                    } else {
                        ///Debug.Log("<b>ChunkMeshPosition " + @chunkMeshPosition + "</b> <color=green><i>successfully to TryDequeue</i></color> and was <color=cyan><i>removed</i></color> <b>queueChunkMeshPositionUpdate</b>.");
                    }
                } else {
                    Debug.LogWarning("<b>ChunkMeshPosition</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueChunkMeshPositionUpdate</b>.");
                }
            }
        }
    }
}