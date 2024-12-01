using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts
{
    public class MeshGenerator : MonoBehaviour
    {
        private Mesh _mesh;

        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uvs;
        private Color[] _colors;

        [FoldoutGroup("Terrain Settings")] [VerticalGroup("Terrain Settings/Vertical")]
        public int _xSize = 20;
        [VerticalGroup("Terrain Settings/Vertical")]
        public int _zSize = 20;
        

        private float _minTerrainHeight;
        private float _maxTerrainHeight;

        [FoldoutGroup("Perlin Noise Amplitudes and Frequencies")]
        [HorizontalGroup("Perlin Noise Amplitudes and Frequencies/Horizontal")]
        [SerializeField, Range(0, 5), LabelWidth(80),
         BoxGroup("Perlin Noise Amplitudes and Frequencies/Horizontal/Amplitudes")]
        private float _amplitude1 = 1f;
        [SerializeField, Range(0, 5), LabelWidth(80),
         BoxGroup("Perlin Noise Amplitudes and Frequencies/Horizontal/Amplitudes")]
        private float _amplitude2 = 1f;
        [SerializeField, Range(0, 5), LabelWidth(80),
         BoxGroup("Perlin Noise Amplitudes and Frequencies/Horizontal/Amplitudes")]
        private float _amplitude3 = 1f;
        [SerializeField, Range(0, 1), LabelWidth(80),
         BoxGroup("Perlin Noise Amplitudes and Frequencies/Horizontal/Frequencies")]
        private float _frequency1 = 1f;
        [SerializeField, Range(0, 1), LabelWidth(80),
         BoxGroup("Perlin Noise Amplitudes and Frequencies/Horizontal/Frequencies")]
        private float _frequency2 = 1f;
        [SerializeField, Range(0, 1), LabelWidth(80),
         BoxGroup("Perlin Noise Amplitudes and Frequencies/Horizontal/Frequencies")]
        private float _frequency3 = 1f;

        [Header("Color Settings")] public Gradient _gradient;

        private void Start() {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;

            CreateShape();
            UpdateMesh();
        }

        private void Update() {
            CreateShape();
            UpdateMesh();
        }

        private void CreateShape() {
            _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];
            _uvs = new Vector2[_vertices.Length];
            _colors = new Color[_vertices.Length];

            for (int i = 0, z = 0; z <= _zSize; z++) {
                for (int x = 0; x <= _xSize; x++) {
                    float y = Mathf.PerlinNoise(x * _frequency1, z * _frequency1) * _amplitude1 +
                              Mathf.PerlinNoise(x * _frequency2, z * _frequency2) * _amplitude2 +
                              Mathf.PerlinNoise(x * _frequency3, z * _frequency3) * _amplitude3;
                    _vertices[i] = new Vector3(x, y, z);
                    _uvs[i] = new Vector2((float)x / _xSize, (float)z / _zSize);

                    float _height = Mathf.InverseLerp(_minTerrainHeight, _maxTerrainHeight, _vertices[i].y);
                    _colors[i] = _gradient.Evaluate(_height);

                    if (y < _minTerrainHeight)
                        _minTerrainHeight = y;
                    if (y > _maxTerrainHeight)
                        _maxTerrainHeight = y;

                    i++;
                }
            }

            _triangles = new int[_xSize * _zSize * 6];

            int _vert = 0;
            int _tris = 0;

            for (int z = 0; z < _zSize; z++) {
                for (int x = 0; x < _xSize; x++) {
                    _triangles[_tris + 0] = _vert + 0;
                    _triangles[_tris + 1] = _vert + _xSize + 1;
                    _triangles[_tris + 2] = _vert + 1;
                    _triangles[_tris + 3] = _vert + 1;
                    _triangles[_tris + 4] = _vert + _xSize + 1;
                    _triangles[_tris + 5] = _vert + _xSize + 2;

                    _vert++;
                    _tris += 6;
                }

                _vert++;
            }


            // for (int i = 0, z = 0; z <= _zSize; z++) {
            //     for (int x = 0; x <= _xSize; x++) {
            //         _uvs[i] = new Vector2((float)x / _xSize, (float)z / _zSize);
            //         i++;
            //     }
            // }
        }

        private void UpdateMesh() {
            _mesh.Clear();

            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.uv = _uvs;
            _mesh.colors = _colors;

            _mesh.RecalculateNormals();
        }
        
        [HorizontalGroup("Terrain Settings/Vertical/Split")]
        [Button("20 x 20"), VerticalGroup("Terrain Settings/Vertical/Split/Left")]
        private void Button20x20() {
            _xSize = 20;
            _zSize = 20;
        }
        [Button("50 x 50"), VerticalGroup("Terrain Settings/Vertical/Split/Middle")]
        private void Button50x50() {
            _xSize = 50;
            _zSize = 50;
        }
        [Button("100 x 100"), VerticalGroup("Terrain Settings/Vertical/Split/Right")]
        private void Button100x100() {
            _xSize = 100;
            _zSize = 100;
        }

        // void OnDrawGizmos() {
        //     for (int i = 0; i < _vertices.Length; i++) {
        //         Gizmos.DrawSphere(_vertices[i], 0.1f);
        //     }
        // }
    }
}