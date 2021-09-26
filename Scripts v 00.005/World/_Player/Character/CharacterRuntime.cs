using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using BlueBird.World.Utilities.Pathfinders;
using BlueBird.World.WorldMap.Topography.Voxels;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterRuntime {
        /* Instances */
        CharacterPathfinder _characterPathfinder = new CharacterPathfinder();

        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Instances - For Character */
        Transform characterTransform;

        /* Storage - For Character */
        private static ConcurrentQueue<Voxel> queueUpdateCharacterPathfinder = new ConcurrentQueue<Voxel>();

        /* Locks - For Queues */
        private readonly object lockUpdateCharacterPath = new object();
        private readonly object lockUpdateCharacterPathfinder = new object();

        public void ManualStart() {
            characterTransform = PlayerData.dictionaryPlayerObject["Character"].transform; // TODO: place this in a better place
        }

        public void UpdateCharacterPath() {
            while(Character.queueCharacterPath.Count > 0 && !Character.isMoving) {
                lock(lockUpdateCharacterPath) {
                    // Move Character based on voxelPath
                    Voxel voxelPath;
                    bool tryPeekTrue = Character.queueCharacterPath.TryPeek(out voxelPath);
                    if(tryPeekTrue) {
                        // TODO: Move Character

                        bool tryDequeueFalse = Character.queueCharacterPath.TryDequeue(out voxelPath);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelPath " + voxelPath.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueCharacterPath</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelPath " + voxelPath.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueCharacterPath</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>VoxelPath</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueCharacterPath</b>.");
                    }
                }
            }
        }

        public void QueueVoxelCharacterPathfinder() {
            while(queueUpdateCharacterPathfinder.Count > 0) {
                lock(lockUpdateCharacterPathfinder) {
                    // Generate CharacterPath from 'voxelData' and Update
                    Voxel voxelData;
                    List<Voxel> listPathfinderResult = new List<Voxel>();
                    bool tryPeekTrue = queueUpdateCharacterPathfinder.TryPeek(out voxelData);
                    if(tryPeekTrue) {
                        Vector3 characterVoxelPosition = _voxelUtilities.GetVoxelPositionFromPosition(characterTransform.position);
                        listPathfinderResult = _characterPathfinder.Pathfinder(characterVoxelPosition, voxelData.voxelPosition);

                        // Clear queueCharacterPath
                        Voxel voxelClear;
                        int queueCharacterPathCount = Character.queueCharacterPath.Count;
                        for(int loopI = 0; loopI < queueCharacterPathCount; loopI++) {
                            bool tryClearFalse = Character.queueCharacterPath.TryDequeue(out voxelClear);
                            if(!tryClearFalse) {
                                Debug.LogWarning("<b>VoxelData " + voxelClear.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueCharacterPath</b>.");
                            } else {
                                ///Debug.Log("<b>VoxelData " + voxelClear.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueCharacterPath</b>.");
                            }
                        }

                        // Copy 'entries' from listPathfinderResult to queueCharacterPath
                        int listPathfinderResultCount = listPathfinderResult.Count;
                        for(int loopJ = 0; loopJ < listPathfinderResultCount; loopJ++) {
                            Voxel voxelPathfinder = listPathfinderResult[loopJ];
                            Character.queueCharacterPath.Enqueue(voxelPathfinder);
                            ///Debug.Log("<b>VoxelData " + voxelPath.voxelPosition + "</b> <color=green><i>Enqueue successfully</i></color> and was <color=cyan><i>added</i></color> to <b>queueCharacterPath</b>.");
                        }

                        bool tryDequeueFalse = queueUpdateCharacterPathfinder.TryDequeue(out voxelData);
                        if(!tryDequeueFalse) {
                            Debug.LogWarning("<b>VoxelData " + voxelData.voxelPosition + "</b> <color=yellow><i>TryDequeue failed</i></color> from <b>queueUpdateCharacterPathfinder</b>.");
                        } else {
                            ///Debug.Log("<b>VoxelData " + voxelData.chunkPosition + "</b> <color=green><i>TryDequeue successfully</i></color> and was <color=cyan><i>removed</i></color> from <b>queueUpdateCharacterPathfinder</b>.");
                        }
                    } else {
                        Debug.LogWarning("<b>VoxelData</b> has <color=yellow><i>failed to TryPeek/Extract</i></color> from <b>queueUpdateCharacterPathfinder</b>.");
                    }
                }
            }
        }
    }
}