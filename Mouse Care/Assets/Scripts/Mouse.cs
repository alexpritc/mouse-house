using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class Mouse : MonoBehaviour {
    private string _status;
    
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

    /// <summary>
    /// Higher _hunger means the mouse needs to eat.
    /// </summary>
    [Range(0,100)] private float _hunger = 0f;
    /// <summary>
    /// Higher _thirst means the mouse needs to drink.
    /// </summary>
    [Range(0,100)] private float _thirst = 0f;

    /// <summary>
    /// How loneley is the mouse
    /// </summary>
    private float _socialisation;
    private float _stress;
    
    /// <summary>
    /// How far the mouse can see around them
    /// </summary>
    [SerializeField] private float _sensoryRadius = 5f;
    [SerializeField] private float _speed = 5f;

    public GameObject target;

    private NavMeshAgent _navMeshAgent;

    [HideInInspector] public Enclosure _enclosure;
    private Vector3 _currentPosition;
    
    // Sensory radius gizmo
    [Range(0, 100)] public int segments = 100;
    
    [Header("Jump Info")]
    private Rigidbody _rigidbody;
    
    public float JumpTime = 0.6f;
    Transform _dummyAgent;
    Vector3 JumpMidPoint;
    Vector3 JumpEndPoint;
    bool checkForStartPointReached;
    Transform _transform;
    List<Vector3> Path = new List<Vector3>();
    float JumpDistance;
    Vector3[] _jumpPath;
    bool previousRigidBodyState;

    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;
    }

    // Update is called once per frame
    void Update() {

       SetDestination(target.transform.position);
    }

    void OnMovementUpdate() {

        _hunger += 10f;
        _thirst += 5f;
        
        GetAllInSensoryRadius();
        CheckMouseAttributes();
    }
    
    void DoJump()
    {
       // previousRigidBodyState = Rigidbody.isKinematic;
        _navMeshAgent.enabled = false;
        _rigidbody.isKinematic = true;

        _jumpPath = Path.ToArray();

        // if you don't want to use a RigidBody change this to
        //transform.DoLocalPath per the DoTween doc's
    }

    void JumpFinished()
    {
        _navMeshAgent.enabled = true;
        _rigidbody.isKinematic = previousRigidBodyState;

        // If using Pooling DeSpawn here instead
        Destroy(_dummyAgent.gameObject);
    }

    void CheckNeeds() {

        if (_hunger >= 10f) {
            // needs to eat
        }

        if (_thirst >= 10f) {
            // needs to drink
        }

    }
    
    void GetAllInSensoryRadius() {

        foreach (var item in _enclosure._itemsInEnclosure) {
            if (Vector3.Distance(item.transform.position, transform.position) < _sensoryRadius) {
                _stress += item._stress;
            }
        }
        
        foreach (var mouse in _enclosure._miceInEnclosure) {
            if (Vector3.Distance(mouse.transform.position, transform.position) < _sensoryRadius) {
                // do something with _socialisation
                // and _reproductiveUrge
            }
        }
        
    }

    void CheckMouseAttributes() {
     
        // if thirsty search for water, if hungry look for food, etc

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
