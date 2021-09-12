using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.Visual;
using BlueBird.World.Player;

namespace BlueBird.World.Data.InvisibleData.Chunks {
    public sealed class ChunkRuntime {
        /* Instances */
        Transform characterTransform;

        ChunkMeshRuntime _chunkMeshRuntime = new ChunkMeshRuntime();

        /* Storage - For Chunks */
        private List<Vector3> listLastVisibleChunks = new List<Vector3>();
        private List<Vector3> listVisibleChunks = new List<Vector3>();
        private static ConcurrentQueue<Chunk> queueUpdateChunkMeshData = new ConcurrentQueue<Chunk>();
        public static ConcurrentQueue<Vector3> queueUpdateChunkMeshPosition = new ConcurrentQueue<Vector3>();

        /* Variables - For Chunks */
        private readonly Vector3 baseVector = Constants_str.worldBaseVector3;
        private readonly float chunkWidth = Constants_str.chunkSize.x;
        private readonly float visibleDistanceChunk = Constants_str.viewDistanceChunk;
        private Vector3 playerChunkPosition = Vector3.zero;
        private Vector3 lastPlayerChunkPosition = Vector3.one;

        /* Locks - For Methods */
        private readonly object lockUpdateChunkMeshData = new object();

        public void ManualStart() {
            VisualEventHandler.eventOnVisualTick_1 += UpdateVisibleChunks;
            characterTransform = PlayerData.dictionaryPlayerObject["Character"].transform;
        }

        public void UpdateVisibleChunks() {
            // Setting up
            playerChunkPosition.x = Mathf.RoundToInt(characterTransform.position.x / chunkWidth);
            playerChunkPosition.y = baseVector.y;
            playerChunkPosition.z = Mathf.RoundToInt(characterTransform.position.z / chunkWidth);

            // Checking if Player has Moved
            if(playerChunkPosition.Equals(lastPlayerChunkPosition)) {
                return;
            } else {
                // Setting up
                lastPlayerChunkPosition = playerChunkPosition;
                listLastVisibleChunks = listVisibleChunks;
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

                            // Enqueue ChunkMeshData to queueUpdateChunkMeshData if the ChunkMesh does not exists
                            if(VisualData.dictionaryChunkMesh.ContainsKey(@viewDistanceChunkPosition)) {
                                continue;
                            } else {
                                queueUpdateChunkMeshData.Enqueue(WorldData.dictionaryChunkData[@viewDistanceChunkPosition]);
                                ///Debug.Log("<b>ChunkData " + @viewDistanceChunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateChunkMeshData</b>.");
                            }
                        }
                    }
                }

                // Checking if Chunks in LastVisible are out of range and Setting to Invisible
                foreach(Vector3 @vector in listLastVisibleChunks) {
                    if(listVisibleChunks.Contains(@vector)) {
                        continue;
                    } else {
                        WorldData.dictionaryChunkData[@vector].SetVisible(false);
                    }
                }
                listLastVisibleChunks.Clear();
            }
        }

        public void UpdateChunkMeshData() {
            while(queueUpdateChunkMeshData.Count > 0) {
                lock(lockUpdateChunkMeshData) {
                    // Generate/Regenerate ChunkMeshData for ChunkData
                    Chunk @chunkData;
                    bool @tryPeekTrue = queueUpdateChunkMeshData.TryPeek(out @chunkData);
                    if(@tryPeekTrue) {
                        _chunkMeshRuntime.GenerateChunkMeshData(@chunkData);
                        bool @tryDequeueFalse = queueUpdateChunkMeshData.TryDequeue(out @chunkData);
                        if(!@tryDequeueFalse) {
                            Debug.LogWarning("<b>ChunkData " + @chunkData.chunkPosition + "</b> <color=yellow><i>failed to TryDequeue</i></color> from <b>queueUpdateChunkMeshData</b>.");
                        } else {
                            ///Debug.Log("<b>ChunkData " + @chunkData.chunkPosition + "</b> <color=green><i>successfully to TryDequeue</i></color> and was <color=cyan><i>removed</i></color> <b>queueChunkMeshUpdate</b>.");
                        }

                        // Enqueue ChunkMesh to queueChunkMeshUpdate
                        queueUpdateChunkMeshPosition.Enqueue(@chunkData.chunkPosition);
                        ///Debug.Log("<b>ChunkMesh " + @chunkData.chunkPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueUpdateChunkMeshData</b>.");
                    } else {
                        Debug.LogWarning("<b>ChunkData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateChunkMeshData</b>.");
                    }
                }
            }
        }

        public void UpdateChunkMesh() {
            while(queueUpdateChunkMeshPosition.Count > 0) {
                // Generate/Regenerate ChunkMeshData for ChunkData
                Vector3 @chunkMeshPosition;
                bool @tryPeekTrue = queueUpdateChunkMeshPosition.TryPeek(out @chunkMeshPosition);
                if(@tryPeekTrue) {
                    _chunkMeshRuntime.CreateChunkMesh(WorldData.dictionaryChunkObject[@chunkMeshPosition], WorldData.dictionaryChunkData[@chunkMeshPosition]);
                    bool @tryDequeueFalse = queueUpdateChunkMeshPosition.TryDequeue(out @chunkMeshPosition);
                    if(!@tryDequeueFalse) {
                        Debug.LogWarning("<b>ChunkMeshPosition " + @chunkMeshPosition + "</b> <color=yellow><i>failed to TryDequeue</i></color> from <b>queueUpdateChunkMeshPosition</b>.");
                    } else {
                        ///Debug.Log("<b>ChunkMeshPosition " + @chunkMeshPosition + "</b> <color=green><i>successfully to TryDequeue</i></color> and was <color=cyan><i>removed</i></color> <b>queueUpdateChunkMeshPosition</b>.");
                    }
                } else {
                    Debug.LogWarning("<b>ChunkMeshPosition</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateChunkMeshPosition</b>.");
                }
            }
        }
    }
}