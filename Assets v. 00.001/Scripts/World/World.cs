using UnityEngine;
using BlueBird.World.Visual;
using BlueBird.World.Visual.Meshes;

namespace BlueBird.World {
    public sealed class World : MonoBehaviour {
        /* Instances */ /// Constants_str, VisualData
        MeshFilter _meshFilter;
        MeshRenderer _meshRenderer;

        MeshUtilities _meshUtilities;

        public float baseNoise = 0.02f;
        public float baseHeight = -5f;
        public float baseNoiseHeight = 4;
        public float elevation = 15;
        public float frequency = 0.005f;

        /* Variables - For constructor */
        public Region2D _region2D { get; internal set; }

        public World() {
            this._region2D = _region2D;
        }

        private void Start() {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            MeshData @meshData = CreateWorld();
            LoadMeshData(@meshData);
        }

        private MeshData CreateWorld() {
            _meshUtilities = new MeshUtilities();
            MeshData @meshData = new MeshData();
            foreach(Vector3 @position in _region2D) {
                _meshUtilities.CreateFaceUP(@meshData, @position);
            }

            return @meshData;
        }

        private void LoadMeshData(MeshData @meshData) {
            Mesh @mesh = new Mesh() {
                vertices = @meshData.vertices.ToArray(),
                triangles = @meshData.trianglesIndices.ToArray(),
                uv = @meshData.uvs.ToArray()
            };

            @mesh.name = "WorldMesh";
            @mesh.RecalculateNormals();
            @mesh.RecalculateBounds();
            @mesh.Optimize();

            _meshFilter.mesh = @mesh;
            _meshRenderer.material = VisualData.materialsDictionary["default"];
        }
    }
}