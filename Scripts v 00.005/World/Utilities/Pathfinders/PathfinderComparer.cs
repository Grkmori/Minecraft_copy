using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Utilities.Pathfinders {
	public sealed class PathfinderComparer : IComparer<PathfinderNode> {
		public int Compare(PathfinderNode nodeToCompare, PathfinderNode nodeCompared) {
			int compareResult = nodeToCompare.fCost.CompareTo(nodeCompared.fCost);
			Debug.Log(compareResult + " fCost " + nodeToCompare.nodePosition + " " + nodeCompared.nodePosition);
			if(compareResult == 0) {
				compareResult = nodeToCompare.hCost.CompareTo(nodeCompared.hCost);
				Debug.Log(compareResult + " hCost");
				if(compareResult == 0) {
					compareResult = nodeToCompare.nodePosition.y.CompareTo(nodeCompared.nodePosition.y);
					Debug.Log(compareResult + " nodePosition.y");
					if(compareResult == 0) {
						compareResult = nodeToCompare.nodePosition.x.CompareTo(nodeCompared.nodePosition.x);
						Debug.Log(compareResult + " nodePosition.x");
						if(compareResult == 0) {
							compareResult = nodeToCompare.nodePosition.z.CompareTo(nodeCompared.nodePosition.z);
							Debug.Log(compareResult + " nodePosition.z");
						}
					}
				}
			}
			return compareResult;
		}
	}
}