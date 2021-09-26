using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.Utilities.Pathfinders;
using BlueBird.World.WorldMap;
using BlueBird.World.WorldMap.Topography.Voxels;
using BlueBird.World.WorldMap.Topography.Chunks;
using BlueBird.World.Entity;

namespace BlueBird.World.Player {
    public sealed class PlayerEntitySpawner : MonoBehaviour {
        /* Instances */
        PlayerUtilities _playerUtilities = new PlayerUtilities();

        CharacterPathfinder _characterPathfinder = new CharacterPathfinder();

        ChunkUtilities _chunkUtilities = new ChunkUtilities();

        /* Instances */
        LayerMask chunkMask;

        Transform NPC;

        /* Storage */
        private List<Voxel> listPath = new List<Voxel>();

        /* Variables */
        private Vector3 mouseVoxelPosition;
        private Vector3 mouseChunkPosition;

        private int currentPathIndex = 0;

        private void OnEnable() {
            PlayerInputHandler.eventOnInputMouse0Down3rd += InputMouse0Down;
            PlayerInputHandler.eventOnInputMouse1Down3rd += InputMouse1Down;
        }

        private void Start() {
            chunkMask = LayerMask.GetMask("Chunk");
        }

        private void Update() {
            //HandleMovement();
        }

        private void OnDisable() {
            PlayerInputHandler.eventOnInputMouse0Down3rd -= InputMouse0Down;
            PlayerInputHandler.eventOnInputMouse1Down3rd -= InputMouse1Down;
        }

        private void InputKeyGDown() {
            if(mouseVoxelPosition != null) {
                NPC = Instantiate(EntityData.dictionaryDynamicEntityPrefabs["NPC"].transform, PlayerInputHandler.mouseVoxelPosition, Quaternion.identity, transform);
                PlayerInputHandler.eventOnInputKeyGDown3rd -= InputKeyGDown;
            }
        }

        private void InputMouse0Down() {
            mouseVoxelPosition = _playerUtilities.GetMousePosition(chunkMask);
            PlayerInputHandler.eventOnInputKeyGDown3rd += InputKeyGDown;
        }

        private void InputMouse1Down() {
            mouseVoxelPosition = _playerUtilities.GetMousePosition(chunkMask);
            mouseChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(mouseVoxelPosition);
            if(WorldMapData.dictionaryChunkData[mouseChunkPosition].dictionaryChunkVoxels[mouseVoxelPosition].isWalkable) {
                listPath = _characterPathfinder.Pathfinder(NPC.position, mouseVoxelPosition);
            } else {
                Debug.LogWarning("Position not Walkable.");
            }

            for(int i = 0; i < listPath.Count - 1; i++) {
                Debug.DrawLine(listPath[i].voxelPosition, listPath[i + 1].voxelPosition, Color.blue, 60f);
            }
        }

        //private void HandleMovement() {
        //    Vector3 targetPosition = listPath[currentPathIndex].voxelPosition;
        //    if(Vector3.Distance(unitSpawn.position, targetPosition) > 1f) {
        //        Vector3 moveDir = (targetPosition - transform.position).normalized;

        //        float distanceBefore = Vector3.Distance(unitSpawn.position, targetPosition);
        //        unitSpawn.position = unitSpawn.position + moveDir * Time.deltaTime;
        //    } else {
        //        currentPathIndex++;
        //        if(currentPathIndex >= listPath.Count) {
        //        }
        //    }
        //}
    }
}