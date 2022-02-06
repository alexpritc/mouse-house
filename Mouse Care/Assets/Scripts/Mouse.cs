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
    
    /// <summary>
    /// Age of mouse in months.
    /// </summary>
    private string _breed;
    private string _temprement;
    
    /// <summary>
    /// Measures how attractive the mouse is to potential mates
    /// </summary>
    private float _desirabilityByOtherSex;
    private float _territorial;
    
    private int _age;
    private int _weight;
    private bool _isFertile;
    private bool _isPregnant;

    private float _hunger;
    private float _thirst;
    /// <summary>
    /// How loneley is the mouse
    /// </summary>
    private float _socialisation;
    private float _reproductiveUrge;
    private float _stress;
    
    public GameObject target;

    private NavMeshAgent _navMeshAgent;

    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {
        
        SetDestination(target.transform.position);
    }

    private void SetDestination(Vector3 targetPos) {
        _navMeshAgent.destination = targetPos;
    }
    
    private void SetDestination(Transform target) {
        _navMeshAgent.destination = target.position;
    }
}
