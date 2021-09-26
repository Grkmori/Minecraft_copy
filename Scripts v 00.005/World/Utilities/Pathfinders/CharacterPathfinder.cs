using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap;
using BlueBird.World.WorldMap.Topography.Chunks;
using BlueBird.World.WorldMap.Topography.Voxels;

namespace BlueBird.World.Utilities.Pathfinders {
    public sealed class CharacterPathfinder {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();

        /* Storage - For Pathfinding */
        private Dictionary<Vector3, PathfinderNode> dictionaryPathfinderNodes;
        private List<PathfinderNode> listSearching; // List with PathfinderNodes for Searching
        private List<Voxel> listFinalPath;

        /* Variables - For Pathfinding */
        private readonly Vector3 bottomVoxel = Constants_str.bottomVoxelPosition;
        private readonly float baseVoxelMovement = Constants_str.baseVoxelMovementCost;

        public readonly int maxNodesSearch = Constants_str.pathfinderMaxSize;
        public readonly float maxGCostValue = Constants_str.pathfinderMaxSize * 10;
        public readonly float moveStrait = Constants_str.moveStraitCost;
        public readonly float moveDiagonal = Constants_str.moveDiagonalCost;
        public readonly float moveVertical = Constants_str.moveVerticalCost;

        public List<Voxel> Pathfinder(Vector3 startPosition, Vector3 endPosition) {
            // Setting up
            dictionaryPathfinderNodes = new Dictionary<Vector3, PathfinderNode>();
            listSearching = new List<PathfinderNode>();
            listFinalPath = new List<Voxel>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // EndPathfinderNode
            Vector3 chunkEndPosition = _chunkUtilities.GetChunkPositionFromPosition(endPosition);
            Chunk chunkEnd = WorldMapData.dictionaryChunkData[chunkEndPosition];
            Voxel voxelEnd = chunkEnd.dictionaryChunkVoxels[endPosition];
            PathfinderNode nodeEnd = new PathfinderNode() {
                voxelReference = voxelEnd,
                nodePosition = voxelEnd.voxelPosition,
                voxelMovementCost = chunkEnd.dictionaryChunkVoxels[voxelEnd.voxelPosition + bottomVoxel].baseMovementCost,
                gCost = maxGCostValue,
                hCost = 0,
                fCost = maxGCostValue,
            };
            dictionaryPathfinderNodes.Add(nodeEnd.nodePosition, nodeEnd);
            listSearching.Add(nodeEnd);
            ///Debug.Log(nodeEnd.nodePosition);

            // StartPathfinderNode
            Vector3 chunkStartPosition = _chunkUtilities.GetChunkPositionFromPosition(startPosition);
            Chunk chunkStart = WorldMapData.dictionaryChunkData[chunkStartPosition];
            Voxel voxelStart = chunkStart.dictionaryChunkVoxels[startPosition];
            PathfinderNode nodeStart = new PathfinderNode() {
                voxelReference = voxelStart,
                nodePosition = voxelStart.voxelPosition,
                parentNode = null,
                voxelMovementCost = chunkStart.dictionaryChunkVoxels[voxelStart.voxelPosition + bottomVoxel].baseMovementCost,
                gCost = 0,
            };
            nodeStart.hCost = CalculateHCost(nodeStart.nodePosition, nodeEnd.nodePosition);
            nodeStart.CalculateFCost();
            dictionaryPathfinderNodes.Add(nodeStart.nodePosition, nodeStart);
            ///Debug.Log(nodeStart.nodePosition + " " + nodeStart.gCost + " " + nodeStart.hCost + " " + nodeStart.fCost);

            // Adding the StartPathfinderNodes Neighbours
            int listVoxelStartNeighboursCount = voxelStart.listVoxelNeighbours.Count;
            for(int loopS = 0; loopS < listVoxelStartNeighboursCount; loopS++) {
                Voxel voxelNeighbour = voxelStart.listVoxelNeighbours[loopS];
                if(!dictionaryPathfinderNodes.ContainsKey(voxelNeighbour.voxelPosition)) {
                    PathfinderNode nodeNeighbour = new PathfinderNode() {
                        voxelReference = voxelNeighbour,
                        nodePosition = voxelNeighbour.voxelPosition,
                        parentNode = nodeStart,
                        voxelMovementCost = voxelNeighbour._chunk.dictionaryChunkVoxels[voxelNeighbour.voxelPosition + bottomVoxel].baseMovementCost,
                    };
                    nodeNeighbour.gCost = CalculateGCost(nodeStart, nodeNeighbour);
                    nodeNeighbour.hCost = CalculateHCost(nodeNeighbour.nodePosition, nodeEnd.nodePosition);
                    nodeNeighbour.CalculateFCost();
                    dictionaryPathfinderNodes.Add(nodeNeighbour.nodePosition, nodeNeighbour);
                    listSearching.Add(nodeNeighbour);
                    ///Debug.Log(nodeNeighbour.nodePosition + " " + nodeNeighbour.gCost + " " + nodeNeighbour.hCost + " " + nodeNeighbour.fCost);
                }
            }

            // Main PathfinderLoop
            while(listSearching.Count > 0) {
                // Get the Lowest 'CurrentNode' on listSearching
                PathfinderNode nodeCurrent = listSearching[0];
                int listSearchingCount = listSearching.Count;
                for(int loopI = 0; loopI < listSearchingCount; loopI++) {
                    if(listSearching[loopI].fCost < nodeCurrent.fCost || (listSearching[loopI].fCost == nodeCurrent.fCost && listSearching[loopI].hCost < nodeCurrent.hCost)) {
                        nodeCurrent = listSearching[loopI];
                    }
                }
                listSearching.Remove(nodeCurrent);
                ///Debug.Log("<b>Node " + nodeCurrent.nodePosition + "</b> is currently being Searched. Node has <i>gCost: " + nodeCurrent.gCost + "</i>; <i>hCost: " + nodeCurrent.hCost + "</i>; <i>fCost: " + nodeCurrent.fCost + "</i>.");

                // If CurrentNode is 'The EndNode' or 'Nodes' > maxNodesSearch Return the listFinalPath
                if(nodeCurrent.nodePosition == nodeEnd.nodePosition) {
                    stopWatch.Stop();
                    UnityEngine.Debug.Log("Path found " + stopWatch.ElapsedMilliseconds + " ms.");

                    return RetracePath(nodeEnd, startPosition, endPosition);
                } else if(dictionaryPathfinderNodes.Count > maxNodesSearch) {
                    stopWatch.Stop();
                    UnityEngine.Debug.Log("Path found " + stopWatch.ElapsedMilliseconds + " ms.");

                    return RetracePath(nodeCurrent, startPosition, endPosition);
                }

                // Adding 'nodeNeighbour' to listSearching
                int listVoxelNeighboursCount = nodeCurrent.voxelReference.listVoxelNeighbours.Count;
                for(int loopJ = 0; loopJ < listVoxelNeighboursCount; loopJ++) {
                    Voxel voxelNeighbour = nodeCurrent.voxelReference.listVoxelNeighbours[loopJ];

                    // Check if nodeNeighbour already 'Exist', if not Create a 'New nodeNeighbour'
                    PathfinderNode nodeNeighbour;
                    if(!dictionaryPathfinderNodes.ContainsKey(voxelNeighbour.voxelPosition)) {
                        nodeNeighbour = new PathfinderNode() {
                            voxelReference = voxelNeighbour,
                            nodePosition = voxelNeighbour.voxelPosition,
                            parentNode = nodeCurrent,
                            voxelMovementCost = voxelNeighbour._chunk.dictionaryChunkVoxels[voxelNeighbour.voxelPosition + bottomVoxel].baseMovementCost,
                            gCost = maxGCostValue,
                        };
                        nodeNeighbour.hCost = CalculateHCost(nodeNeighbour.nodePosition, nodeEnd.nodePosition);
                        nodeNeighbour.CalculateFCost();
                        dictionaryPathfinderNodes.Add(nodeNeighbour.nodePosition, nodeNeighbour);
                        listSearching.Add(nodeNeighbour);
                    } else {
                        nodeNeighbour = dictionaryPathfinderNodes[voxelNeighbour.voxelPosition];
                    }

                    // Check gCost of 'currentPath' (nodeCurrent) is lower than nodeNeighbour gCost
                    float currentPathGCost = CalculateGCost(nodeCurrent, nodeNeighbour) + nodeCurrent.gCost;
                    if(nodeNeighbour.gCost > currentPathGCost) {
                        nodeNeighbour.parentNode = nodeCurrent;
                        nodeNeighbour.gCost = currentPathGCost;
                        nodeNeighbour.CalculateFCost();
                        ///Debug.Log(nodeNeighbour.nodePosition + " " + nodeNeighbour.gCost + " " + nodeNeighbour.hCost + " " + nodeNeighbour.fCost);
                    }
                }
            }

            UnityEngine.Debug.LogWarning("<b>CharacterPathfinder</b> could not find a <color=yellow><i>Path</i></color>. " +
                             "<i>StartPosition: " + startPosition + "</i>; <i>EndPosition: " + endPosition + "</i>.");
            return null;
        }

        private float CalculateHCost(Vector3 positionA, Vector3 positionB) {
            float xDistance = Mathf.Abs(positionA.x - positionB.x);
            float yDistance = Mathf.Abs(positionA.y - positionB.y);
            float zDistance = Mathf.Abs(positionA.z - positionB.z);
            float straitDistance = Mathf.Abs(xDistance - zDistance);
            return ((moveStrait * straitDistance) + (moveDiagonal * Mathf.Min(xDistance, zDistance)) + (moveVertical * yDistance)) * baseVoxelMovement;
        }

        private float CalculateGCost(PathfinderNode nodeA, PathfinderNode nodeB) {
            float xDistance = Mathf.Abs(nodeA.nodePosition.x - nodeB.nodePosition.x);
            float yDistance = Mathf.Abs(nodeA.nodePosition.y - nodeB.nodePosition.y);
            float zDistance = Mathf.Abs(nodeA.nodePosition.z - nodeB.nodePosition.z);
            float straitDistance = Mathf.Abs(xDistance - zDistance);
            return ((moveStrait * straitDistance) + (moveDiagonal * Mathf.Min(xDistance, zDistance)) + (moveVertical * yDistance)) * ((nodeA.voxelMovementCost / 2) + (nodeB.voxelMovementCost / 2));
        }

        private List<Voxel> RetracePath(PathfinderNode nodeLast, Vector3 startPosition, Vector3 endPosition) {
            listFinalPath.Add(nodeLast.voxelReference);
            ///Debug.Log("<b>VoxelData " + nodeCurrent.voxelReference.voxelPosition + "</b> was <color=cyan><i>added</i></color> to <b>listFinalPath</b>.");
            PathfinderNode nodePath = nodeLast.parentNode;
            while(nodePath != null) {
                ///Debug.Log("<b>VoxelData " + pathNode.voxelReference.voxelPosition + "</b> was <color=cyan><i>added</i></color> to <b>listFinalPath</b>.");
                listFinalPath.Add(nodePath.voxelReference);
                nodePath = nodePath.parentNode;
            }
            listFinalPath.Reverse();

            if(nodeLast.nodePosition == endPosition) {
                UnityEngine.Debug.Log("<b>CharacterPathfinder</b> has <color=olive><i>found a Path</i></color>. Returning <b>listFinalPath</b> with a total of: <i>" + listFinalPath.Count + " Voxels</i>. " +
                          "<i>StartPosition: " + startPosition + "</i>; <i>EndPosition: " + nodeLast.nodePosition + "</i>.");
            } else {
                UnityEngine.Debug.LogWarning("<b>CharacterPathfinder</b> has <color=yellow><i>not found a Path</i></color>. Returning <b>listFinalPath</b> with a total of: <i>" + listFinalPath.Count + " Voxels</i>. " +
                                 "<i>StartPosition: " + startPosition + "</i>; <i>EndPosition: " + nodeLast.nodePosition + "</i>.");
            }
            return listFinalPath;
        }
    }
}