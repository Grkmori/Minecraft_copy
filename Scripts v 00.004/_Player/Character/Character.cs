using UnityEngine;
using System.Collections.Generic;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Player.Character {
    public sealed class Character : MonoBehaviour {
        /* Storage - For PathFind */
        public List<Voxel> listVoxelsPathFind = new List<Voxel>();

        /* Variables - For Character */
        //public static readonly int characterID = 1;
        //public static readonly string characterName = "Blue";
        //public static readonly string characterLastName = "Bird";

        public static Vector3 characterRadius = Constants_str.characterBaseRadius;
        public static float walkSpeed = Constants_str.characterBaseWalkSpeed;
        public static float jumpForce = Constants_str.characterBaseJumpForce;
    }
}