using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Chunks;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Utilities.Pathfinders {
    public sealed class CharacterPathfinder {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();

        Chunk chunkStart;
        Chunk chunkEnd;
        Voxel voxelStart;
        Voxel voxelEnd;
        PathfinderNode nodeStart;
        PathfinderNode nodeEnd;

        Voxel voxelNeighbour;
        PathfinderNode nodeNeighbour;
        PathfinderNode nodeCurrent;

        PathfinderNode pathNode;

        /* Storage - For Pathfinding */
        private Dictionary<Vector3, PathfinderNode> dictionaryPathfinderNodes;
        private List<PathfinderNode> listSearching; // List with PathfinderNodes for Searching
        private List<Voxel> listFinalPath;

        /* Variables - For Pathfinding */
        private readonly Vector3 baseVoxelPosition = Constants_str.baseVoxelTerrain;
        private readonly float baseVoxelMovement = Constants_str.baseVoxelMovementCost;
        private Vector3 chunkStartPosition;
        private Vector3 chunkEndPosition;
        private int loopS;
        private int listVoxelStartNeighboursCount;

        private float currentPathGCost;
        private int loopI;
        private int loopJ;
        private int loopK;
        private int listSearchingCount;
        private int listVoxelNeighboursCount;

        public readonly float moveStrait = Constants_str.moveStraitCost;
        public readonly float moveDiagonal = Constants_str.moveDiagonalCost;
        public readonly float moveVertical = Constants_str.moveVerticalCost;
        private float xDistance;
        private float yDistance;
        private float zDistance;
        private float straitDistance;

        public List<Voxel> Pathfinder(Vector3 startPosition, Vector3 endPosition) {
            dictionaryPathfinderNodes = new Dictionary<Vector3, PathfinderNode>();
            listSearching = new List<PathfinderNode>();
            listFinalPath = new List<Voxel>();

            // EndPathfinderNode
            chunkEndPosition = _chunkUtilities.GetChunkPositionFromPosition(endPosition);
            chunkEnd = WorldData.dictionaryChunkData[chunkEndPosition];
            voxelEnd = chunkEnd.dictionaryChunkVoxels[endPosition];
            nodeEnd = new PathfinderNode() {
                voxelReference = voxelEnd,
                nodePosition = voxelEnd.voxelPosition,
                isSearched = false,
                fromNode = nodeStart,
                voxelMovementCost = chunkEnd.dictionaryChunkVoxels[voxelEnd.voxelPosition + baseVoxelPosition].baseMovementCost,
                gCost = float.MaxValue,
                hCost = 0,
                fCost = float.MaxValue,
            };
            dictionaryPathfinderNodes.Add(nodeEnd.nodePosition, nodeEnd);
            listSearching.Add(nodeEnd);
            ///Debug.Log(nodeEnd.nodePosition);

            // StartPathfinderNode
            chunkStartPosition = _chunkUtilities.GetChunkPositionFromPosition(startPosition);
            chunkStart = WorldData.dictionaryChunkData[chunkStartPosition];
            voxelStart = chunkStart.dictionaryChunkVoxels[startPosition];
            nodeStart = new PathfinderNode() {
                voxelReference = voxelStart,
                nodePosition = voxelStart.voxelPosition,
                isSearched = true,
                fromNode = null,
                voxelMovementCost = chunkStart.dictionaryChunkVoxels[voxelStart.voxelPosition + baseVoxelPosition].baseMovementCost,
                gCost = 0,
            };
            nodeStart.hCost = CalculateHCost(nodeStart.nodePosition, nodeEnd.nodePosition);
            nodeStart.CalculateFCost();
            dictionaryPathfinderNodes.Add(nodeStart.nodePosition, nodeStart);
            ///Debug.Log(nodeStart.nodePosition + " " + nodeStart.gCost + " " + nodeStart.hCost + " " + nodeStart.fCost);

            // Adding the StartPathfinderNodes Neighbours
            listVoxelStartNeighboursCount = voxelStart.listVoxelNeighbours.Count;
            for(loopS = 0; loopS < listVoxelStartNeighboursCount; loopS++) {
                voxelNeighbour = voxelStart.listVoxelNeighbours[loopS];
                if(!dictionaryPathfinderNodes.ContainsKey(voxelNeighbour.voxelPosition)) {
                    nodeNeighbour = new PathfinderNode() {
                        voxelReference = voxelNeighbour,
                        nodePosition = voxelNeighbour.voxelPosition,
                        isSearched = false,
                        fromNode = nodeStart,
                        voxelMovementCost = voxelNeighbour._chunk.dictionaryChunkVoxels[voxelNeighbour.voxelPosition + baseVoxelPosition].baseMovementCost,
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
                nodeCurrent = listSearching[0];
                listSearchingCount = listSearching.Count;
                for(loopI = 0; loopI < listSearchingCount; loopI++) {
                    if(listSearching[loopI].fCost < nodeCurrent.fCost) {
                        nodeCurrent = listSearching[loopI];
                        for(loopJ = 0; loopJ < listSearchingCount; loopJ++) {
                            if(listSearching[loopJ].fCost < nodeCurrent.fCost && listSearching[loopJ].hCost < nodeCurrent.hCost) {
                                nodeCurrent = listSearching[loopJ];
                            }
                        }
                    }
                }

                // Set CurrentNode as 'Searched'
                nodeCurrent.isSearched = true;
                listSearching.Remove(nodeCurrent);
                ///Debug.Log(nodeCurrent.nodePosition + " " + nodeCurrent.gCost + " " + nodeCurrent.hCost + " " + nodeCurrent.fCost);

                // If CurrentNode is 'The EndNode' Return the listFinalPath
                if(nodeCurrent.nodePosition == nodeEnd.nodePosition) {
                    listFinalPath.Add(nodeEnd.voxelReference);
                    ///Debug.Log(nodeEnd.voxelReference.voxelPosition);
                    pathNode = nodeEnd.fromNode;
                    while(pathNode != null) {
                        ///Debug.Log(pathNode.voxelReference.voxelPosition);
                        listFinalPath.Add(pathNode.voxelReference);
                        pathNode = pathNode.fromNode;
                    }
                    listFinalPath.Reverse();

                    Debug.Log("<b>CharacterPathfinder</b> has found a <color=teal><i>Path</i></color>. Returning <b>listFinalPath</b> with a total of: <i>" + listFinalPath.Count + " Voxels</i>. " +
                              "<i>StartPosition: " + startPosition + "</i>; <i>EndPosition: " + endPosition + "</i>.");
                    return listFinalPath;
                }

                // Adding 'nodeNeighbour' to listSearching
                listVoxelNeighboursCount = nodeCurrent.voxelReference.listVoxelNeighbours.Count;
                for(loopK = 0; loopK < listVoxelNeighboursCount; loopK++) {
                    voxelNeighbour = nodeCurrent.voxelReference.listVoxelNeighbours[loopK];

                    // Check if nodeNeighbour already 'Exist', if not Create a 'New nodeNeighbour'
                    if(!dictionaryPathfinderNodes.ContainsKey(voxelNeighbour.voxelPosition)) {
                        nodeNeighbour = new PathfinderNode() {
                            voxelReference = voxelNeighbour,
                            nodePosition = voxelNeighbour.voxelPosition,
                            isSearched = false,
                            voxelMovementCost = voxelNeighbour._chunk.dictionaryChunkVoxels[voxelNeighbour.voxelPosition + baseVoxelPosition].baseMovementCost,
                            gCost = float.MaxValue,
                        };
                        dictionaryPathfinderNodes.Add(nodeNeighbour.nodePosition, nodeNeighbour);
                        listSearching.Add(nodeNeighbour);
                    }
                    nodeNeighbour = dictionaryPathfinderNodes[voxelNeighbour.voxelPosition];

                    // Check gCost of 'currentPath' (nodeCurrent) is lower than nodeNeighbour gCost
                    currentPathGCost = CalculateGCost(nodeCurrent, nodeNeighbour) + nodeCurrent.gCost;
                    if(nodeNeighbour.gCost > currentPathGCost) {
                        nodeNeighbour.fromNode = nodeCurrent;
                        nodeNeighbour.gCost = currentPathGCost;
                        nodeNeighbour.hCost = CalculateHCost(nodeNeighbour.nodePosition, nodeEnd.nodePosition);
                        nodeNeighbour.CalculateFCost();
                        ///Debug.Log(nodeNeighbour.nodePosition + " " + nodeNeighbour.gCost + " " + nodeNeighbour.hCost + " " + nodeNeighbour.fCost);
                    }
                }
            }

            Debug.LogWarning("<b>CharacterPathfinder</b> could not find a <color=yellow><i>Path</i></color>. " +
                             "<i>StartPosition: " + startPosition + "</i>; <i>EndPosition: " + endPosition + "</i>.");
            return null;
        }

        private float CalculateHCost(Vector3 positionA, Vector3 positionB) {
            xDistance = Mathf.Abs(positionA.x - positionB.x);
            yDistance = Mathf.Abs(positionA.y - positionB.y);
            zDistance = Mathf.Abs(positionA.z - positionB.z);
            straitDistance = Mathf.Abs(xDistance - zDistance);
            return ((moveStrait * straitDistance) + (moveDiagonal * Mathf.Min(xDistance, zDistance)) + (moveVertical * yDistance)) * baseVoxelMovement;
        }

        private float CalculateGCost(PathfinderNode nodeA, PathfinderNode nodeB) {
            xDistance = Mathf.Abs(nodeA.nodePosition.x - nodeB.nodePosition.x);
            yDistance = Mathf.Abs(nodeA.nodePosition.y - nodeB.nodePosition.y);
            zDistance = Mathf.Abs(nodeA.nodePosition.z - nodeB.nodePosition.z);
            straitDistance = Mathf.Abs(xDistance - zDistance);
            return ((moveStrait * straitDistance) + (moveDiagonal * Mathf.Min(xDistance, zDistance)) + (moveVertical * yDistance)) * ((nodeA.voxelMovementCost / 2) + (nodeB.voxelMovementCost / 2));
        }
    }
}