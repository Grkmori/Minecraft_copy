using UnityEngine;
using System.Collections.Concurrent;
using BlueBird.World.WorldMap.Topography.Voxels;

namespace BlueBird.World.Entity.NPCs {
    public sealed class NPC : MonoBehaviour {
        /* Storage - For PathFind */
        public ConcurrentQueue<Voxel> queueCharacterPath;

        /* Variables - For NPC */
        //public readonly int NPCID;
        //public readonly string NPCName;
        //public readonly string NPCLastName;

        //public Vector3 NPCRadius;
        public float walkSpeed { get; set; }
        //public float jumpForce;

        public NPC() {
            this.walkSpeed = walkSpeed;

            this.queueCharacterPath = new ConcurrentQueue<Voxel>();
        }
    }
}