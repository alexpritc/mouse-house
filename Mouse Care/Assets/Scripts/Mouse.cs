using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [Range(0,100)] private float _hunger = 50;
    [Range(0,100)] private float _thirst = 50;

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

    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;
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

            Gizmos.DrawWireSphere(transform.position, _sensoryRadius);

            angle += (360f / segments);
        }
    }

    private void OnDrawGizmosSelected() {
        CreatePoints();
    }
}
