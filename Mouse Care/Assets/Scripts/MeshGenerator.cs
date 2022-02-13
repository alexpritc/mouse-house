using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uvs;
    private Color32[] _colors;

    [SerializeField] private int _xSize = 20;
    [SerializeField] private int _zSize = 20;

    [SerializeField] private int _xSpacing = 1;
    [SerializeField] private int _zSpacing = 1;
    
    [SerializeField] private float _yModifier = 2f;

    private NavMeshSurface _navMeshSurface;

    [SerializeField] private Gradient _gradient;
    private Color32 currentColor;

    [HideInInspector] public Vector3[] _gridPositions;

    void Start()
    {
        if (_mesh != null) {
            return;
        }
        
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _navMeshSurface = GetComponent<NavMeshSurface>();

        currentColor = new Color32(0,0,0,0);
        
        CreateShape();
        //GenerateGridPositions();
        UpdateMesh();
        SaveMesh("Assets/Meshes/mesh.asset");
    }

    void CreateShape() {
        
        // Calculate vertices
        int numberOfVertices = (_xSize + 1) * (_zSize + 1);
        _vertices = new Vector3[numberOfVertices];
        _uvs = new Vector2[numberOfVertices];
        
        for (int i = 0, cols = 0; cols <= _zSize; cols++) {
            
            for (int rows = 0; rows <= _xSize; rows++, i++) {

                float z = cols * _zSpacing;
                float x = rows * _xSpacing;
                float y = Mathf.PerlinNoise(x * 0.2f, z * 0.2f) * _yModifier;

                _vertices[i] = new Vector3(x, y, z);
                _uvs[i] = new Vector2(z / _zSize, x / _xSize);
            }
        }
        
        // Calculate triangles
        _triangles = new int[_xSize * _zSize * 6];
        for (int vert = 0, tris = 0, cols = 0; cols < _zSize; cols++) {
            
            for (int rows = 0; rows < _xSize; rows++) {
                
                // 6 triangles per quad
                _triangles[tris + 0] = vert + 0;
                _triangles[tris + 1] = vert + _xSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + _xSize + 1;
                _triangles[tris + 5] = vert + _xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
    }

    void UpdateMesh() {
        _mesh.Clear();
        
        UpdateMeshAttributes();

        _mesh.RecalculateNormals();
        
        _navMeshSurface.BuildNavMesh();
    }

    private void GenerateGridPositions() {
        
        _gridPositions = new Vector3[(_xSize + 1) * (_zSize + 1)];
        
        for (int i = 0, cols = 0; cols < _zSize; cols++){

            for (int rows = 0; rows < _xSize; rows++) {
                
                float xOffset = _xSpacing / 2f;
                float zOffset = _zSpacing / 2f;

                float y1 = FindVertexHeightAtXZ(_vertices[i].x + _xSpacing, _vertices[i].z);
                float y2 = FindVertexHeightAtXZ(_vertices[i].x, _vertices[i].z + _zSpacing);
                
                float yOffset = (y1 + y2) / 2f;

                Vector3 pos = transform.position; 
                pos += new Vector3( _vertices[i].x + xOffset, yOffset, _vertices[i].z + zOffset);

                _gridPositions[i] = pos;
                
                i++;
            }

            i++;
        }
    }

    private float FindVertexHeightAtXZ(float x, float z) {

        foreach (var vertex in _vertices) {
            if (vertex.x == x && vertex.z == z) {
                return vertex.y;
            }
        }

        return Mathf.Epsilon;
    }
    
    private void OnDrawGizmosSelected() {

        foreach (var position in _mesh.vertices) {
            Gizmos.DrawSphere(position, 0.1f);
        }
    }

    private void UpdateMeshAttributes()
    {
        Vector3[] verticesModified = new Vector3[_triangles.Length];
        int[] trianglesModified = new int[_triangles.Length];
        
        Color32[] colors = new Color32[_triangles.Length];
        
        for (int i = 0; i < trianglesModified.Length; i++) {
            
            // Makes every vertex unique
            verticesModified[i] = _vertices[_triangles[i]];
            trianglesModified[i] = i;
            
            // Every third vertex randomly chooses new color
            if(i % 3 == 0){
                currentColor = _gradient.Evaluate (Random.Range (0f, 1f));
            }

            colors[i] = currentColor;
        }
        
        // Apply changes to mesh
        _mesh.vertices = verticesModified;
        _mesh.triangles = trianglesModified;
        _mesh.colors32 = colors;
    }

    private void SaveMesh(string path) {
        AssetDatabase.CreateAsset(_mesh, path);
        AssetDatabase.SaveAssets();
    }
}
