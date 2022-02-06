using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mouse : MonoBehaviour {
    private string _status;

    private int _id;
    private string _name;
    private char _sex;
    private string _breed;
    private string _temprement;

    /// <summary>
    /// Age of mouse in months.
    /// </summary>
    private int _age;

    private int _weight;
    private bool _isFertile;

    private float _hunger;
    private float _thirst;

    /// <summary>
    /// How loneley is the mouse
    /// </summary>
    private float _socialisation;

    private float _reproductiveUrge;
    private float _stress;

    /// <summary>
    /// How far the mouse can see around them
    /// </summary>
    [SerializeField] private float _sensoryRadius;
    [SerializeField] private float _speed;

    public GameObject target;

    private NavMeshAgent _navMeshAgent;
    
    // Sensory radius gizmo
    [Range(0, 100)] public int segments = 100;
    private LineRenderer line;

    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;

        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
    }

    // Update is called once per frame
    void Update() {

        SetDestination(target.transform.position);
    }

    public void SetDestination(Vector3 targetPos) {
        _navMeshAgent.destination = targetPos;
    }

    public void SetDestination(Transform target) {
        _navMeshAgent.destination = target.position;
    }
    
    void CreatePoints() {
        float x;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++) {
            x = Mathf.Cos(Mathf.Deg2Rad * angle) * _sensoryRadius;
            z = Mathf.Sin(Mathf.Deg2Rad * angle) * _sensoryRadius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }
}
