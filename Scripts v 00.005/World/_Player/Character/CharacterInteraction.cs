using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap;
using BlueBird.World.WorldMap.Topography.Chunks;
using BlueBird.World.WorldMap.Topography.Voxels;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterInteraction : MonoBehaviour {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();
        VoxelRuntime _voxelRuntime = new VoxelRuntime();

        /* Instances - For Character Interaction */
        Transform characterTransform;
        Transform characterCameraTransform;
        Transform characterCrosshairTransform;
        Transform highlightVoxelTransform;
        Transform placeVoxelTransform;
        Image characterCrosshairVerticalLine;
        Image characterCrosshairHorizontalLine;

        WaitForSeconds blockInteractionDelay = new WaitForSeconds(delayTime);

        Chunk currentChunk;
        Chunk cursorChunk;

        /* Variables - For Character Interaction */
        private readonly float checkIncrement = Constants_str.cursorCheckIncrement;
        private readonly float minReach = Constants_str.minCursorReach;
        private readonly float maxReach = Constants_str.maxCursorReach;

        private Vector3 cursorPosition;
        private Vector3 cursorVoxelPosition;
        private Vector3 cursorLastPosition;
        private Vector3 cursorDistanceFromCharacter;
        private float cursorDistanceOffset = 0.5f;

        private bool destroyVoxelOnCursorPosition = false;
        private bool placeVoxelOnCursorPosition = false;
        private static readonly float delayTime = Constants_str.defaultDelayTimeInSeconds;
        private bool voxelActionDelay = false;

        /* Variables - For Voxels */
        private readonly string defaultVoxelName = Constants_str.defaultVoxelTypeName;
        public static string selectedVoxelTypeName;

        private void OnEnable() {
            PlayerInputHandler.eventOnInputMouse0Down1st += DestroyVoxelOnCursorPosition;
            PlayerInputHandler.eventOnInputMouse1Down1st += PlaceVoxelOnCursorPosition;
        }

        private void Start() {
            // Setting up
            characterTransform = PlayerData.dictionaryPlayerObject["Character"].transform;
            characterCameraTransform = PlayerData.dictionaryPlayerObject["CharacterCamera"].transform;
            characterCrosshairTransform = PlayerData.dictionaryPlayerUIObject["CharacterCrosshair"].transform;
            highlightVoxelTransform = PlayerData.dictionaryPlayerObject["HighlightVoxel"].transform;
            placeVoxelTransform = PlayerData.dictionaryPlayerObject["PlaceVoxel"].transform;
            characterCrosshairVerticalLine = characterCrosshairTransform.Find("VerticalLine").GetComponent<Image>();
            characterCrosshairHorizontalLine = characterCrosshairTransform.Find("HorizontalLine").GetComponent<Image>();

            // Setting Initial Values
            Vector3 characterChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(transform.position);
            currentChunk = WorldMapData.dictionaryChunkData[characterChunkPosition];
            cursorChunk = currentChunk;
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                CursorVoxelPosition();
            }
        }

        private void OnDisable() {
            PlayerInputHandler.eventOnInputMouse0Down1st -= DestroyVoxelOnCursorPosition;
            PlayerInputHandler.eventOnInputMouse1Down1st -= PlaceVoxelOnCursorPosition;
            StopCoroutine(CoroutineCursorVoxelInteractionDelay());
        }

        private void CursorVoxelPosition() {
            // Setting up
            currentChunk = _chunkUtilities.CheckForCurrentChunk(transform.position, currentChunk);
            float stepIncrement = minReach;

            while(!PlayerInputHandler.view3rdPerson && stepIncrement < maxReach) {
                cursorPosition = characterCameraTransform.position + (characterCameraTransform.forward * stepIncrement);
                cursorVoxelPosition = _voxelUtilities.GetVoxelPositionFromPosition(cursorPosition);
                cursorDistanceFromCharacter = characterTransform.position - cursorLastPosition;

                if(currentChunk.dictionaryChunkVoxels.ContainsKey(cursorVoxelPosition)) {
                    if(currentChunk.dictionaryChunkVoxels[cursorVoxelPosition].isSolid) {
                        highlightVoxelTransform.position = cursorVoxelPosition;
                        highlightVoxelTransform.gameObject.SetActive(true);
                        if((cursorDistanceFromCharacter.x < -(Character.characterRadius.x + cursorDistanceOffset) || cursorDistanceFromCharacter.x > Character.characterRadius.x + cursorDistanceOffset) ||
                           (cursorDistanceFromCharacter.y < -(Character.characterRadius.y + cursorDistanceOffset) || cursorDistanceFromCharacter.y > Character.characterRadius.y + cursorDistanceOffset) ||
                           (cursorDistanceFromCharacter.z < -(Character.characterRadius.z + cursorDistanceOffset) || cursorDistanceFromCharacter.z > Character.characterRadius.z + cursorDistanceOffset)) {
                            placeVoxelTransform.position = cursorLastPosition;
                            placeVoxelTransform.gameObject.SetActive(true);
                        } else {
                            placeVoxelTransform.gameObject.SetActive(false);
                        }

                        return;
                    } else {
                        cursorLastPosition = cursorVoxelPosition;
                        stepIncrement += checkIncrement;
                    }
                } else {
                    cursorChunk = _chunkUtilities.CheckForCurrentChunk(cursorVoxelPosition, currentChunk);
                    if(cursorChunk.dictionaryChunkVoxels.ContainsKey(cursorVoxelPosition)) {
                        if(cursorChunk.dictionaryChunkVoxels[cursorVoxelPosition].isSolid) {
                            highlightVoxelTransform.position = cursorVoxelPosition;
                            highlightVoxelTransform.gameObject.SetActive(true);
                            if((cursorDistanceFromCharacter.x < -(Character.characterRadius.x + cursorDistanceOffset) || cursorDistanceFromCharacter.x > Character.characterRadius.x + cursorDistanceOffset) ||
                               (cursorDistanceFromCharacter.y < -(Character.characterRadius.y + cursorDistanceOffset) || cursorDistanceFromCharacter.y > Character.characterRadius.y + cursorDistanceOffset) ||
                               (cursorDistanceFromCharacter.z < -(Character.characterRadius.z + cursorDistanceOffset) || cursorDistanceFromCharacter.z > Character.characterRadius.z + cursorDistanceOffset)) {
                                placeVoxelTransform.position = cursorLastPosition;
                                placeVoxelTransform.gameObject.SetActive(true);
                            } else {
                                placeVoxelTransform.gameObject.SetActive(false);
                            }

                            return;
                        } else {
                            cursorLastPosition = cursorVoxelPosition;
                            stepIncrement += checkIncrement;
                        }
                    } else {
                        cursorLastPosition = cursorVoxelPosition;
                        stepIncrement += checkIncrement;
                    }
                }
            }

            highlightVoxelTransform.gameObject.SetActive(false);
            placeVoxelTransform.gameObject.SetActive(false);
        }

        private void DestroyVoxelOnCursorPosition() {
            if(highlightVoxelTransform.gameObject.activeSelf && !destroyVoxelOnCursorPosition) {
                if(currentChunk.dictionaryChunkVoxels.ContainsKey(highlightVoxelTransform.position)) {
                    _voxelRuntime.DestroyVoxel(currentChunk.dictionaryChunkVoxels[highlightVoxelTransform.position], defaultVoxelName);
                    characterCrosshairVerticalLine.color = Color.red;
                    characterCrosshairHorizontalLine.color = Color.red;
                    destroyVoxelOnCursorPosition = true;
                    placeVoxelOnCursorPosition = true;
                    voxelActionDelay = true;
                    StartCoroutine(CoroutineCursorVoxelInteractionDelay());
                } else {
                    if(cursorChunk.dictionaryChunkVoxels.ContainsKey(highlightVoxelTransform.position)) {
                        _voxelRuntime.DestroyVoxel(cursorChunk.dictionaryChunkVoxels[highlightVoxelTransform.position], defaultVoxelName);
                        characterCrosshairVerticalLine.color = Color.red;
                        characterCrosshairHorizontalLine.color = Color.red;
                        destroyVoxelOnCursorPosition = true;
                        placeVoxelOnCursorPosition = true;
                        voxelActionDelay = true;
                        StartCoroutine(CoroutineCursorVoxelInteractionDelay());
                    }
                }
            }
        }

        private void PlaceVoxelOnCursorPosition() {
            if(placeVoxelTransform.gameObject.activeSelf && !placeVoxelOnCursorPosition) {
                if(currentChunk.dictionaryChunkVoxels.ContainsKey(placeVoxelTransform.position)) {
                    if(selectedVoxelTypeName != "Empty") {
                        _voxelRuntime.PlaceVoxel(currentChunk.dictionaryChunkVoxels[placeVoxelTransform.position], selectedVoxelTypeName);
                        characterCrosshairVerticalLine.color = Color.red;
                        characterCrosshairHorizontalLine.color = Color.red;
                        destroyVoxelOnCursorPosition = true;
                        placeVoxelOnCursorPosition = true;
                        voxelActionDelay = true;
                        StartCoroutine(CoroutineCursorVoxelInteractionDelay());
                    }
                } else {
                    if(cursorChunk.dictionaryChunkVoxels.ContainsKey(placeVoxelTransform.position)) {
                        if(selectedVoxelTypeName != "Empty") {
                            _voxelRuntime.PlaceVoxel(cursorChunk.dictionaryChunkVoxels[placeVoxelTransform.position], selectedVoxelTypeName);
                            characterCrosshairVerticalLine.color = Color.red;
                            characterCrosshairHorizontalLine.color = Color.red;
                            destroyVoxelOnCursorPosition = true;
                            placeVoxelOnCursorPosition = true;
                            voxelActionDelay = true;
                            StartCoroutine(CoroutineCursorVoxelInteractionDelay());
                        }
                    }
                }
            }
        }

        private IEnumerator CoroutineCursorVoxelInteractionDelay() {
            while(placeVoxelOnCursorPosition && destroyVoxelOnCursorPosition) {
                if(voxelActionDelay) {
                    voxelActionDelay = false;
                    yield return blockInteractionDelay;
                } else {
                    characterCrosshairVerticalLine.color = Color.white;
                    characterCrosshairHorizontalLine.color = Color.white;
                    destroyVoxelOnCursorPosition = false;
                    placeVoxelOnCursorPosition = false;
                }
            }
        }
    }
}