using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Player.Character {
    public sealed class CharacterInteraction : MonoBehaviour {
        /* Instances */
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
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();
        VoxelRuntime _voxelRuntime = new VoxelRuntime();

        /* Variables - For Character Interaction */
        private readonly float checkIncrement = Constants_str.cursorCheckIncrement;
        private readonly float minReach = Constants_str.minCursorReach;
        private readonly float maxReach = Constants_str.maxCursorReach;

        private float stepIncrement;
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
            Vector3 @characterChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(transform.position);
            currentChunk = WorldData.dictionaryChunkData[@characterChunkPosition];
            cursorChunk = currentChunk;
        }

        private void FixedUpdate() {
            if(!WorldDirector.isPaused) {
                CursorVoxelPosition();
            }
        }

        private void OnDisable() {
            StopCoroutine(CoroutineCursorVoxelInteractionDelay());
        }

        private void CursorVoxelPosition() {
            // Setting up
            currentChunk = _chunkUtilities.CheckForCurrentChunk(transform.position, currentChunk);
            stepIncrement = minReach;

            while(!PlayerInputHandler.view3rdPerson && stepIncrement < maxReach) {
                cursorPosition = characterCameraTransform.position + (characterCameraTransform.forward * stepIncrement);
                cursorVoxelPosition = _voxelUtilities.GetVoxelPositionFromPosition(cursorPosition);
                cursorDistanceFromCharacter = characterTransform.position - cursorLastPosition;

                if(currentChunk.dictionaryChunkVoxels.ContainsKey(cursorVoxelPosition)) {
                    if(WorldData.dictionaryVoxelDefinition[currentChunk.dictionaryChunkVoxels[cursorVoxelPosition].voxelTypeName].isSolid) {
                        highlightVoxelTransform.position = cursorVoxelPosition;
                        highlightVoxelTransform.gameObject.SetActive(true);
                        if((cursorDistanceFromCharacter.x < -(CharacterBehaviour.characterRadius.x + cursorDistanceOffset) || cursorDistanceFromCharacter.x > CharacterBehaviour.characterRadius.x + cursorDistanceOffset) ||
                           (cursorDistanceFromCharacter.y < -(CharacterBehaviour.characterRadius.y + cursorDistanceOffset) || cursorDistanceFromCharacter.y > CharacterBehaviour.characterRadius.y + cursorDistanceOffset) ||
                           (cursorDistanceFromCharacter.z < -(CharacterBehaviour.characterRadius.z + cursorDistanceOffset) || cursorDistanceFromCharacter.z > CharacterBehaviour.characterRadius.z + cursorDistanceOffset)) {
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
                        if(WorldData.dictionaryVoxelDefinition[cursorChunk.dictionaryChunkVoxels[cursorVoxelPosition].voxelTypeName].isSolid) {
                            highlightVoxelTransform.position = cursorVoxelPosition;
                            highlightVoxelTransform.gameObject.SetActive(true);
                            if((cursorDistanceFromCharacter.x < -(CharacterBehaviour.characterRadius.x + cursorDistanceOffset) || cursorDistanceFromCharacter.x > CharacterBehaviour.characterRadius.x + cursorDistanceOffset) ||
                               (cursorDistanceFromCharacter.y < -(CharacterBehaviour.characterRadius.y + cursorDistanceOffset) || cursorDistanceFromCharacter.y > CharacterBehaviour.characterRadius.y + cursorDistanceOffset) ||
                               (cursorDistanceFromCharacter.z < -(CharacterBehaviour.characterRadius.z + cursorDistanceOffset) || cursorDistanceFromCharacter.z > CharacterBehaviour.characterRadius.z + cursorDistanceOffset)) {
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

        public void DestroyVoxelOnCursorPosition() {
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

        public void PlaceVoxelOnCursorPosition() {
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