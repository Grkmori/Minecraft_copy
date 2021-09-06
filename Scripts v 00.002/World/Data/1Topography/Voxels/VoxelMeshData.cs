using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelMeshData {
		private static float voxelWidthOffSet = Constants_str.voxelSize.x;
		private static float voxelHeightOffSet = Constants_str.voxelSize.y;
		private static float halfvoxelWidthOffSet = Constants_str.voxelSize.x / 2;
		private static float halfvoxelHeightOffSet = Constants_str.voxelSize.y / 2;

		public readonly Vector3[] voxelVertices = new Vector3[8] {
			new Vector3(-halfvoxelWidthOffSet, -halfvoxelHeightOffSet, -halfvoxelWidthOffSet),
			new Vector3(+halfvoxelWidthOffSet, -halfvoxelHeightOffSet, -halfvoxelWidthOffSet),
			new Vector3(+halfvoxelWidthOffSet, +halfvoxelHeightOffSet, -halfvoxelWidthOffSet),
			new Vector3(-halfvoxelWidthOffSet, +halfvoxelHeightOffSet, -halfvoxelWidthOffSet),
			new Vector3(-halfvoxelWidthOffSet, -halfvoxelHeightOffSet, +halfvoxelWidthOffSet),
			new Vector3(+halfvoxelWidthOffSet, -halfvoxelHeightOffSet, +halfvoxelWidthOffSet),
			new Vector3(+halfvoxelWidthOffSet, +halfvoxelHeightOffSet, +halfvoxelWidthOffSet),
			new Vector3(-halfvoxelWidthOffSet, +halfvoxelHeightOffSet, +halfvoxelWidthOffSet)
		};

		public readonly int[,] voxelTrianglesIndices = new int[6, 4] {
			// 0 1 2 2 1 3
			{0, 3, 1, 2}, // South Face
			{5, 6, 4, 7}, // North Face
			{3, 7, 2, 6}, // Top Face
			{1, 5, 0, 4}, // Bottom Face
			{4, 7, 0, 3}, // West Face
			{1, 2, 5, 6} // East Face
		};

		public readonly Vector2[] voxelUVs = new Vector2[4] {
			new Vector2 (0.0f, 0.0f),
			new Vector2 (0.0f, 1.0f),
			new Vector2 (1.0f, 0.0f),
			new Vector2 (1.0f, 1.0f)
		};

		public readonly Vector3[] faceChecks = new Vector3[6] {
			new Vector3(0.0f, 0.0f, -voxelWidthOffSet), // South Face
			new Vector3(0.0f, 0.0f, +voxelWidthOffSet), // North Face
			new Vector3(0.0f, +voxelHeightOffSet, 0.0f), // Top Face
			new Vector3(0.0f, -voxelHeightOffSet, 0.0f), // Bottom Face
			new Vector3(-voxelWidthOffSet, 0.0f, 0.0f), // West Face
			new Vector3(+voxelWidthOffSet, 0.0f, 0.0f) // East Face
		};
	}
}