using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap.Topography.Voxels;

namespace BlueBird.World.Player.Character {
    public sealed class Character : MonoBehaviour {
        /* Storage - For PathFind */
        public static ConcurrentQueue<Voxel> queueCharacterPath = new ConcurrentQueue<Voxel>();

        /* Variables - For Character */
        //public static readonly int characterID = 1;
        //public static readonly string characterName = "Blue";
        //public static readonly string characterLastName = "Bird";

        public static Vector3 characterRadius = Constants_str.characterBaseRadius;
        public static float walkSpeed = Constants_str.characterBaseWalkSpeed;
        public static float jumpForce = Constants_str.characterBaseJumpForce;

        public static bool isMoving = false;
    }
}