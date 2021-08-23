using UnityEngine;
using BlueBird.World.Visual;

namespace BlueBird.World {
    public sealed class WorldGenerator {
        /* Instances */ /// Constants_str
        Region2D _region2D;

        public void CreateWorldGameObject(Vector3 @worldSize) {
            // Create a GameObject for the World
            GameObject @worldObject = new GameObject("WorldMap", typeof(World), typeof(MeshFilter), typeof(MeshRenderer));
            @worldObject.transform.localPosition = Vector3.zero;
            @worldObject.transform.localScale = Vector3.one;
            @worldObject.transform.localEulerAngles = Vector3.zero;
            @worldObject.tag = "World";
            @worldObject.layer = 9;
            Debug.Log("<b>" + @worldObject.name + " GameObject</b> was <color=green><i>created</i></color>.");

            // Create a Region for the World
            Region2D @region = new Region2D(Constants_str.worldBaseVector3, @worldSize);
            World @world = @worldObject.GetComponent<World>();
            @world._region2D = @region;
            Debug.Log("<b>The World</b> was successful <color=green><i>generated</i></color>.");
        }
    }
}