using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;

    [SerializeField] private int _xSize = 20;
    [SerializeField] private int _zSize = 20;

    private NavMeshSurface _navMeshSurface;
    
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _navMeshSurface = GetComponent<NavMeshSurface>();

        CreateShape();
        UpdateMesh();
    }

    void CreateShape() {
        
        // Calculate vertices
        _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];
        
        for (int i = 0, z = 0; z <= _zSize; z++) {
            
            for (int x = 0; x <= _xSize; x++, i++) {
                
                //float y = Mathf.PerlinNoise(x * 0.7f, z * 0.7f) * 2f;
                _vertices[i] = new Vector3(x, 0, z);
            }
        }
        
        // Calculate triangles
        _triangles = new int[_xSize * _zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < _zSize; z++) {
            
            for (int x = 0; x < _xSize; x++) {
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

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        
        _mesh.RecalculateNormals();
        
        _navMeshSurface.BuildNavMesh();
    }
}
