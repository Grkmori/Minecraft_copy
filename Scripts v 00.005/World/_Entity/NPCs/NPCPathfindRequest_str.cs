using UnityEngine;

namespace BlueBird.World.Entity.NPCs {
    public struct NPCPathfindRequest_str {
        /* Instances - For Constructor */
        public Transform NPCTransform { get; set; }

        /* Variables - For Constructor */
        public Vector3 startPosition { get; set; }
        public Vector3 endPosition { get; set; }

        public NPCPathfindRequest_str(Transform _NPCTransform, Vector3 _startPosition, Vector3 _endPosition) {
            this.NPCTransform = _NPCTransform;
            this.startPosition = _startPosition;
            this.endPosition = _endPosition;
        }
    }
}
