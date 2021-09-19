using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.Utilities.Pathfinders;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Player {
    public sealed class PlayerEntitySpawner : MonoBehaviour {
        /* Instances */
        LayerMask chunkMask;

        Transform unitSpawn;

        PlayerUtilities _playerUtilities = new PlayerUtilities();

        CharacterPathfinder _characterPathfinder = new CharacterPathfinder();

        /* Storage */
        private List<Voxel> listPath = new List<Voxel>();

        /* Variables */
        private bool isUnitPlaced;
        private Vector3 mouseVoxelPosition;

        private int currentPathIndex = 0;

        private void OnEnable() {
            PlayerInputHandler.eventOnInputMouse0Down3rd += InputMouse0Down;
            PlayerInputHandler.eventOnInputMouse1Down3rd += InputMouse1Down;
        }

        private void Start() {
            chunkMask = LayerMask.GetMask("Chunk");
            unitSpawn = GameObject.Find("Unit").transform;
        }

        private void Update() {
            //HandleMovement();
        }

        private void OnDisable() {
            PlayerInputHandler.eventOnInputMouse1Down3rd -= InputMouse1Down;
        }

        private void InputMouse0Down() {
            unitSpawn.position = PlayerInputHandler.mouseVoxelPosition + new Vector3(0, 0.5f, 0);
            PlayerInputHandler.eventOnInputMouse0Down3rd -= InputMouse0Down;
        }

        private void InputMouse1Down() {
            mouseVoxelPosition = _playerUtilities.GetMousePosition(chunkMask);
            listPath = _characterPathfinder.Pathfinder(unitSpawn.position + new Vector3(0, -0.5f, 0), mouseVoxelPosition);
            for(int i = 0; i < listPath.Count - 1; i++) {
                Debug.DrawLine(listPath[i].voxelPosition, listPath[i + 1].voxelPosition, Color.blue);
            }
        }

        private void HandleMovement() {
            Vector3 targetPosition = listPath[currentPathIndex].voxelPosition;
            if(Vector3.Distance(unitSpawn.position, targetPosition) > 1f) {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(unitSpawn.position, targetPosition);
                unitSpawn.position = unitSpawn.position + moveDir * Time.deltaTime;
            } else {
                currentPathIndex++;
                if(currentPathIndex >= listPath.Count) {
                }
            }
        }
    }
}