using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;
using BlueBird.World.Player.Character;

namespace BlueBird.World.Player.Character.Collisions {
    public sealed class CharacterCollisions {
        /* Instances - For Collisions */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Variables - For  Voxels */
        private readonly Vector2 voxelSize = Constants_str.voxelSize;
        private readonly float voxelWidthRadius = Constants_str.voxelSize.x / 2;
        private readonly float voxelHeightRadius = Constants_str.voxelSize.y / 2;

        /* Variables - For Collisions */
        private readonly float bounceBack = Constants_str.onCollisionBounceBack;

        private readonly Vector3 characterBoundS = new Vector3(0, 0, -CharacterBehaviour.characterRadius.x);
        private readonly Vector3 characterBoundN = new Vector3(0, 0, +CharacterBehaviour.characterRadius.x);
        private readonly Vector3 characterBoundW = new Vector3(-CharacterBehaviour.characterRadius.x, 0, 0);
        private readonly Vector3 characterBoundE = new Vector3(+CharacterBehaviour.characterRadius.x, 0, 0);
        private readonly Vector3 characterBoundT = new Vector3(0, +CharacterBehaviour.characterRadius.y + CharacterBehaviour.characterHeight, 0);
        private readonly Vector3 characterBoundB = new Vector3(0, -CharacterBehaviour.characterRadius.y, 0);

        public float CheckForCollisionTB(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @characterVoxelPosition, Vector3 @offSetFromVoxelCenter) {
            if(@offSetFromVoxelCenter.y + characterBoundT.y > voxelHeightRadius && @velocity.y > 0) {
                Vector3 @neighbourT = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + characterBoundT);
                if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbourT)) {
                    if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbourT].voxelTypeName].isSolid) { // Checking if Top Voxel isSolid
                        return -bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollisionsT_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundT, characterBoundE, characterBoundN, characterBoundS); // Checking if Top/East & Top/East/North&South Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollisionsT_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundT, characterBoundW, characterBoundN, characterBoundS); // Checking if Top/West & Top/West/North&South Voxel isSolid
                        } else {
                            return @velocity.y;
                        }
                    }
                } else {
                    if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbourT)].dictionaryChunkVoxels[@neighbourT].voxelTypeName].isSolid) { // Checking if Top Voxel isSolid + new Chunk
                        return -bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollisionsT_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundT, characterBoundE, characterBoundN, characterBoundS); // Checking if Top/East & Top/East/North&South Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollisionsT_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundT, characterBoundW, characterBoundN, characterBoundS); // Checking if Top/West & Top/West/North&South Voxel isSolid
                        } else {
                            return @velocity.y;
                        }
                    }
                }
            } else if(@offSetFromVoxelCenter.y + characterBoundB.y < -voxelHeightRadius && @velocity.y < 0) {
                Vector3 @neighbourB = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + characterBoundB);
                if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbourB)) {
                    if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbourB].voxelTypeName].isSolid) { // Checking if Bottom Voxel isSolid
                        CharacterBehaviour.isGrounded = true;
                        return +bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollisionsB_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundB, characterBoundE, characterBoundN, characterBoundS); // Checking if Bottom/East & Bottom/East/North&South Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollisionsB_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundB, characterBoundW, characterBoundN, characterBoundS); // Checking if Bottom/West & Bottom/West/North&South Voxel isSolid
                        } else {
                            return @velocity.y;
                        }
                    }
                } else {
                    if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbourB)].dictionaryChunkVoxels[@neighbourB].voxelTypeName].isSolid) { // Checking if Bottom Voxel isSolid + new Chunk
                        CharacterBehaviour.isGrounded = true;
                        return +bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollisionsB_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundB, characterBoundE, characterBoundN, characterBoundS); // Checking if Bottom/East & Bottom/East/North&South Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollisionsB_Layer2(@velocity, @characterPosition, @currentChunk, @offSetFromVoxelCenter, characterBoundB, characterBoundW, characterBoundN, characterBoundS); // Checking if Bottom/West & Bottom/West/North&South Voxel isSolid
                        } else {
                            return @velocity.y;
                        }
                    }
                }
            }

            return @velocity.y;
        }

        private float CheckForCollisionsT_Layer2(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @offSetFromVoxelCenter, Vector3 @bound1st, Vector3 @bound2nd, Vector3 @bound3rdA, Vector3 @bound3rdB) {
            Vector3 @neighbour12 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour12)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    if(@offSetFromVoxelCenter.z + @bound3rdA.z > voxelWidthRadius && @velocity.z > 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdA);
                    } else if(@offSetFromVoxelCenter.z + @bound3rdB.z < -voxelWidthRadius && @velocity.z < 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdB);
                    } else {
                        return @velocity.y;
                    }
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour12)].dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    if(@offSetFromVoxelCenter.z + @bound3rdA.z > voxelWidthRadius && @velocity.z > 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdA);
                    } else if(@offSetFromVoxelCenter.z + @bound3rdB.z < -voxelWidthRadius && @velocity.z < 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdB);
                    } else {
                        return @velocity.y;
                    }
                }
            }
        }

        private float CheckForCollisionT_Layer3(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @bound1st, Vector3 @bound2nd, Vector3 @bound3rd) {
            Vector3 @neighbour123 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd + @bound3rd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour123)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour123].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    return @velocity.y;
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour123)].dictionaryChunkVoxels[@neighbour123].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    return @velocity.y;
                }
            }
        }

        private float CheckForCollisionsB_Layer2(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @offSetFromVoxelCenter, Vector3 @bound1st, Vector3 @bound2nd, Vector3 @bound3rdA, Vector3 @bound3rdB) {
            Vector3 @neighbour12 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour12)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    CharacterBehaviour.isGrounded = true;
                    return +bounceBack;
                } else {
                    if(@offSetFromVoxelCenter.z + @bound3rdA.z > voxelWidthRadius && @velocity.z > 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdA);
                    } else if(@offSetFromVoxelCenter.z + @bound3rdB.z < -voxelWidthRadius && @velocity.z < 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdB);
                    } else {
                        return @velocity.y;
                    }
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour12)].dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    CharacterBehaviour.isGrounded = true;
                    return +bounceBack;
                } else {
                    if(@offSetFromVoxelCenter.z + @bound3rdA.z > voxelWidthRadius && @velocity.z > 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdA);
                    } else if(@offSetFromVoxelCenter.z + @bound3rdB.z < -voxelWidthRadius && @velocity.z < 0) {
                        return CheckForCollisionB_Layer3(@velocity, characterPosition, @currentChunk, @bound1st, @bound2nd, @bound3rdB);
                    } else {
                        return @velocity.y;
                    }
                }
            }
        }

        private float CheckForCollisionB_Layer3(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @bound1st, Vector3 @bound2nd, Vector3 @bound3rd) {
            Vector3 @neighbour123 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd + @bound3rd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour123)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour123].voxelTypeName].isSolid) {
                    CharacterBehaviour.isGrounded = true;
                    return +bounceBack;
                } else {
                    return @velocity.y;
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour123)].dictionaryChunkVoxels[@neighbour123].voxelTypeName].isSolid) {
                    CharacterBehaviour.isGrounded = true;
                    return +bounceBack;
                } else {
                    return @velocity.y;
                }
            }
        }

        public float CheckForCollisionEW(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @characterVoxelPosition, Vector3 @offSetFromVoxelCenter) {
            if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                Vector3 @neighbourE = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + characterBoundE);
                if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbourE)) {
                    if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbourE].voxelTypeName].isSolid) { // Checking if East Voxel isSolid
                        return -bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.z + characterBoundN.z > voxelWidthRadius && @velocity.z > 0) {
                            CheckForCollision_E_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundE, characterBoundN); // Checking if East/North Voxel isSolid
                        } else if(@offSetFromVoxelCenter.z + characterBoundS.z < -voxelWidthRadius && @velocity.z < 0) {
                            CheckForCollision_E_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundE, characterBoundS); // Checking if East/South Voxel isSolid
                        } else {
                            return @velocity.x;
                        }
                    }
                } else {
                    if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbourE)].dictionaryChunkVoxels[@neighbourE].voxelTypeName].isSolid) { // Checking if East Voxel isSolid + new Chunk
                        return -bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.z + characterBoundN.z > voxelWidthRadius && @velocity.z > 0) {
                            CheckForCollision_E_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundE, characterBoundN); // Checking if East/North Voxel isSolid
                        } else if(@offSetFromVoxelCenter.z + characterBoundS.z < -voxelWidthRadius && @velocity.z < 0) {
                            CheckForCollision_E_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundE, characterBoundS); // Checking if East/South Voxel isSolid
                        } else {
                            return @velocity.x;
                        }
                    }
                }
            } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                Vector3 @neighbourW = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + characterBoundW);
                if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbourW)) {
                    if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbourW].voxelTypeName].isSolid) { // Checking if West Voxel isSolid
                        return +bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.z + characterBoundN.z > voxelWidthRadius && @velocity.z > 0) {
                            CheckForCollision_W_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundW, characterBoundN); // Checking if West/North Voxel isSolid
                        } else if(@offSetFromVoxelCenter.z + characterBoundS.z < -voxelWidthRadius && @velocity.z < 0) {
                            CheckForCollision_W_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundW, characterBoundS); // Checking if West/South Voxel isSolid
                        } else {
                            return @velocity.x;
                        }
                    }
                } else {
                    if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbourW)].dictionaryChunkVoxels[@neighbourW].voxelTypeName].isSolid) { // Checking if West Voxel isSolid + new Chunk
                        return +bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.z + characterBoundN.z > voxelWidthRadius && @velocity.z > 0) {
                            CheckForCollision_W_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundW, characterBoundN); // Checking if West/North Voxel isSolid
                        } else if(@offSetFromVoxelCenter.z + characterBoundS.z < -voxelWidthRadius && @velocity.z < 0) {
                            CheckForCollision_W_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundW, characterBoundS); // Checking if West/South Voxel isSolid
                        } else {
                            return @velocity.x;
                        }
                    }
                }
            }

            return @velocity.x;
        }

        public float CheckForCollisionNS(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @characterVoxelPosition, Vector3 @offSetFromVoxelCenter) {
            if(@offSetFromVoxelCenter.z + characterBoundN.z > voxelWidthRadius && @velocity.z > 0) {
                Vector3 @neighbourN = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + characterBoundN);
                if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbourN)) {
                    if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbourN].voxelTypeName].isSolid) { // Checking if North Voxel isSolid
                        return -bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollision_N_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundN, characterBoundE); // Checking if North/East Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollision_N_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundN, characterBoundW); // Checking if North/West Voxel isSolid
                        } else {
                            return @velocity.z;
                        }
                    }
                } else {
                    if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbourN)].dictionaryChunkVoxels[@neighbourN].voxelTypeName].isSolid) { // Checking if North Voxel isSolid + new Chunk
                        return -bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollision_N_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundN, characterBoundE); // Checking if North/East Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollision_N_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundN, characterBoundW); // Checking if North/West Voxel isSolid
                        } else {
                            return @velocity.z;
                        }
                    }
                }
            } else if(@offSetFromVoxelCenter.z + characterBoundS.z < -voxelWidthRadius && @velocity.z < 0) {
                Vector3 @neighbourS = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + characterBoundS);
                if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbourS)) {
                    if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbourS].voxelTypeName].isSolid) { // Checking if South Voxel isSolid
                        return +bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollision_S_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundS, characterBoundE); // Checking if South/East Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollision_S_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundS, characterBoundW); // Checking if South/West Voxel isSolid
                        } else {
                            return @velocity.z;
                        }
                    }
                } else {
                    if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbourS)].dictionaryChunkVoxels[@neighbourS].voxelTypeName].isSolid) { // Checking if South Voxel isSolid + new Chunk
                        return +bounceBack;
                    } else {
                        if(@offSetFromVoxelCenter.x + characterBoundE.x > voxelWidthRadius && @velocity.x > 0) {
                            CheckForCollision_S_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundS, characterBoundE); // Checking if South/East Voxel isSolid
                        } else if(@offSetFromVoxelCenter.x + characterBoundW.x < -voxelWidthRadius && @velocity.x < 0) {
                            CheckForCollision_S_Layer2(@velocity, @characterPosition, @currentChunk, characterBoundS, characterBoundW); // Checking if South/West Voxel isSolid
                        } else {
                            return @velocity.z;
                        }
                    }
                }
            }

            return @velocity.z;
        }

        private float CheckForCollision_E_Layer2(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @bound1st, Vector3 @bound2nd) {
            Vector3 @neighbour12 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour12)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    return @velocity.x;
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour12)].dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    return @velocity.x;
                }
            }
        }

        private float CheckForCollision_W_Layer2(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @bound1st, Vector3 @bound2nd) {
            Vector3 @neighbour12 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour12)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return +bounceBack;
                } else {
                    return @velocity.x;
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour12)].dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return +bounceBack;
                } else {
                    return @velocity.x;
                }
            }
        }

        private float CheckForCollision_N_Layer2(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @bound1st, Vector3 @bound2nd) {
            Vector3 @neighbour12 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour12)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    return @velocity.z;
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour12)].dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return -bounceBack;
                } else {
                    return @velocity.z;
                }
            }
        }

        private float CheckForCollision_S_Layer2(Vector3 @velocity, Vector3 @characterPosition, Chunk @currentChunk, Vector3 @bound1st, Vector3 @bound2nd) {
            Vector3 @neighbour12 = _voxelUtilities.GetVoxelPositionFromPosition(@characterPosition + @bound1st + @bound2nd);
            if(@currentChunk.dictionaryChunkVoxels.ContainsKey(@neighbour12)) {
                if(WorldData.dictionaryVoxelDefinition[@currentChunk.dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return +bounceBack;
                } else {
                    return @velocity.z;
                }
            } else {
                if(WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[_chunkUtilities.GetChunkPositionFromPosition(@neighbour12)].dictionaryChunkVoxels[@neighbour12].voxelTypeName].isSolid) {
                    return +bounceBack;
                } else {
                    return @velocity.z;
                }
            }
        }
    }
}