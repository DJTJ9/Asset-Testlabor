using System;
using UnityEngine;

namespace _Project.Scripts
{
    public class MeshGenerator : MonoBehaviour
    {
        private Mesh _mesh;
    
        Vector3[] _vertices;
        int[] _triangles;
        Vector2[] _uvs;

        public int _xSize = 20;
        public int _zSize = 20;

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

            for (int i = 0, z = 0; z <= _zSize; z++) {
                for (int x = 0; x <= _xSize; x++) {
                    float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                    _vertices[i] = new Vector3(x, y, z);
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
            
            _uvs = new Vector2[_vertices.Length];
            
            for (int i = 0, z = 0; z <= _zSize; z++) {
                for (int x = 0; x <= _xSize; x++) {
                    _uvs[i] = new Vector2((float)x / _xSize, (float)z / _zSize);
                    i++;
                }
            }
        }
    
        private void UpdateMesh() {
            _mesh.Clear();
        
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.uv = _uvs;
        
            _mesh.RecalculateNormals();
        }

        // void OnDrawGizmos() {
        //     for (int i = 0; i < _vertices.Length; i++) {
        //         Gizmos.DrawSphere(_vertices[i], 0.1f);
        //     }
        // }
    }
}

