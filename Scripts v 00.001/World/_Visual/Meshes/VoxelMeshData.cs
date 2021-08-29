using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Visual {
    public sealed class VoxelMeshData {
		private static readonly float voxelWidthOffSet = Constants_str.voxelWidth;
		private static readonly float voxelHeightOffSet = Constants_str.voxelHeight;
		private static readonly float halfVoxelWidthOffSet = Constants_str.voxelWidth / 2;
		private static readonly float halfVoxelHeightOffSet = Constants_str.voxelHeight / 2;

		public static readonly Vector3[] voxelVertices = new Vector3[8] {
			new Vector3(-halfVoxelWidthOffSet, -halfVoxelHeightOffSet, -halfVoxelWidthOffSet),
			new Vector3(+halfVoxelWidthOffSet, -halfVoxelHeightOffSet, -halfVoxelWidthOffSet),
			new Vector3(+halfVoxelWidthOffSet, +halfVoxelHeightOffSet, -halfVoxelWidthOffSet),
			new Vector3(-halfVoxelWidthOffSet, +halfVoxelHeightOffSet, -halfVoxelWidthOffSet),
			new Vector3(-halfVoxelWidthOffSet, -halfVoxelHeightOffSet, +halfVoxelWidthOffSet),
			new Vector3(+halfVoxelWidthOffSet, -halfVoxelHeightOffSet, +halfVoxelWidthOffSet),
			new Vector3(+halfVoxelWidthOffSet, +halfVoxelHeightOffSet, +halfVoxelWidthOffSet),
			new Vector3(-halfVoxelWidthOffSet, +halfVoxelHeightOffSet, +halfVoxelWidthOffSet)
		};

		public static readonly int[,] voxelTrianglesIndices = new int[6, 4] {
			// 0 1 2 2 1 3
			{0, 3, 1, 2}, // Back Face
			{5, 6, 4, 7}, // Front Face
			{3, 7, 2, 6}, // Top Face
			{1, 5, 0, 4}, // Bottom Face
			{4, 7, 0, 3}, // Left Face
			{1, 2, 5, 6} // Right Face
		};

		public static readonly Vector2[] voxelUVs = new Vector2[4] {
			new Vector2 (0.0f, 0.0f),
			new Vector2 (0.0f, 1.0f),
			new Vector2 (1.0f, 0.0f),
			new Vector2 (1.0f, 1.0f)
		};

		public static readonly Vector3[] faceChecks = new Vector3[6] {
			new Vector3(0.0f, 0.0f, -voxelWidthOffSet), // Back Face
			new Vector3(0.0f, 0.0f, +voxelWidthOffSet), // Front Face
			new Vector3(0.0f, +voxelHeightOffSet, 0.0f), // Top Face
			new Vector3(0.0f, -voxelHeightOffSet, 0.0f), // Bottom Face
			new Vector3(-voxelWidthOffSet, 0.0f, 0.0f), // Left Face
			new Vector3(+voxelWidthOffSet, 0.0f, 0.0f) // Right Face
		};
	}
}