using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;


public enum MouseStates {
    Idle,
    Moving,
    LookingForFood,
    Eating,
    LookingForWater,
    Drinking
}

public class Mouse : MonoBehaviour {
    [SerializeField] private MouseStates _status;

    private string _name;
    
    public string Name
    {
        get => _name;
    }
    
    public MouseStates Status
    {
        get => _status;
    }

    /// <summary>
    /// Higher _hunger means the mouse needs to eat.
    /// </summary>
    [Range(0,100)] private float _hunger = 0f;
    /// <summary>
    /// Higher _thirst means the mouse needs to drink.
    /// </summary>
    [Range(0,100)] private float _thirst = 0f;
    
    public float Hunger
    {
        get => _hunger;
    }
    public float Thirst
    {
        get => _thirst;
    }
    
    /// <summary>
    /// How far the mouse can see around them
    /// </summary>
    [SerializeField] private float _sensoryRadius = 5f;
    /// <summary>
    /// The area outside of the sensory radius that the
    /// mouse will pick a new random destination from
    /// </summary>
    [SerializeField] private float _boundaryRadius = 2f;

    private CapsuleCollider _collider;
    
    [SerializeField] private float _speed = 10f;
    private NavMeshAgent _navMeshAgent;

    [HideInInspector] public Enclosure _enclosure;
    private Vector3 _currentPosition;
    private Vector3[] _allVertices;
    
    // Sensory radius gizmo
    [Range(0, 100)] private int segments = 100;

    private Animator _animator;

    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;

        _animator = GetComponent<Animator>();
        //_collider = GetComponent<CapsuleCollider>();
        //_collider.radius = _sensoryRadius;
        
        _name = PickRandomName();
    }

    string PickRandomName()
    {
        if (Random.Range(1, 3) % 2 == 0)
        {
            return "Dendy";
        }
        else
        {
            return "Mishmash";
        }
    }

    // Update is called once per frame
    void Update() {

        //if (NeedsMet())
       // {
            if (!IsMouseWithinTarget()) {
                Move();
            }
            else {
                // Find new destination
                Idle(500f);
            }   
       // }
        //else
       // {
            //if (NeedsWater())
            //{
             //   // Look for water
            //    _status = MouseStates.LookingForWater;
            //    _statusUI.text = _status.ToString();
            //}
       // }
       
    }
    
    ///////// Needs /////////
    bool NeedsMet()
    {
        return !(_hunger > 50f) && !(_thirst > 50f);
    }

    bool NeedsWater()
    {
        return _thirst > 50f;
    }
    
    bool NeedsFood()
    {
        return _hunger > 50f;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (NeedsWater() && other.CompareTag("Water"))
        {
            SetDestination(other.transform);
        }
    }

    Transform [] array;
    void GetInactiveInRadius(){
        foreach (Transform tr in array){
            float distanceSqr = (transform.position - tr.position).sqrMagnitude;
            if (distanceSqr < _sensoryRadius)
                tr.gameObject.SetActive(true);
        }
    }

    void AdjustNeeds(ref float need, float value, float chance)
    {
        if (Random.Range(0f, chance) <= 1f)
        {
            need += value;
        }
    }
    
    ///////// Movement /////////
    void Move()
    {
        if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Walking") {
            _animator.Play("Walking");
            _status = MouseStates.Moving;
        }

        AdjustNeeds(ref _hunger, 1f, 250f);
        AdjustNeeds(ref _thirst, 2f, 250f);
    }

    void Idle(float chance)
    {
        // Find new destination
        _status = MouseStates.Idle;

        if (Random.Range(0f, chance) <= 1f) {
            SetDestination(FindNewDestinationOutsideSensoryRadius());   
        }
        
        AdjustNeeds(ref _hunger, 1f, 750f);
        AdjustNeeds(ref _thirst, 1f, 750f);
    }
    
    Vector3 FindNewDestinationOutsideSensoryRadius() {
        List<Vector3> verticesOutsideSensoryRadius = new List<Vector3>();
        
        foreach (var vertex in _allVertices) {

            if (!IsInsideRadius(vertex) && IsInsideRadius(vertex, _boundaryRadius)) {
                verticesOutsideSensoryRadius.Add(vertex);
            }
        }

        return GetRandomVertex(verticesOutsideSensoryRadius);
    }

    Vector3 GetRandomVertex(List<Vector3> list) {
        return list[Random.Range(0, list.Count)];
    }

    bool IsInsideRadius(Vector3 pos, float outerBounds = 1f) {

        if (Vector3.Distance(pos, transform.position) <= _sensoryRadius * outerBounds) {
            return true;
        }
        
        return false;
    }

    bool IsMouseWithinTarget() {
        
        // Check if we've reached the destination
        if (!_navMeshAgent.pathPending) {
            
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
                
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f) {
                    
                    return true;  // Arrived
                }
            }
        }
        return false;
    }

    public void SetDestination(Vector3 targetPos) {
        _navMeshAgent.destination = targetPos;
    }

    public void SetDestination(Transform target) {
        _navMeshAgent.destination = target.position;
    }

    ///////// Debugging /////////
    private void OnDrawGizmosSelected() {

        if (_navMeshAgent.hasPath) {
            Gizmos.DrawLine(transform.position, _navMeshAgent.destination);
        }
    }
}
