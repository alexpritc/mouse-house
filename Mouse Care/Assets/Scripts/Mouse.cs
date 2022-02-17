using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using TMPro;

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
    private char _sex;
    private string _breed;
    private string _temprement;

    /// <summary>
    /// Age of mouse in months. A mouse is initially 8 weeks old.
    /// </summary>
    private int _ageMonths = 0;
    private int _ageWeeks = 8;

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
    /// <summary>
    /// The area outside of the sensory radius that the
    /// mouse will pick a new random destination from
    /// </summary>
    [SerializeField] private float _boundaryRadius = 2f;
    
    [SerializeField] private float _speed = 10f;

    private NavMeshAgent _navMeshAgent;

    [HideInInspector] public Enclosure _enclosure;
   
    private Vector3 _currentPosition;

    private Vector3[] _allVertices;
    
    // Sensory radius gizmo
    [Range(0, 100)] public int segments = 100;

    private Animator _animator;

    private TextMeshProUGUI _statusUI;
    private Canvas _statusCanvas;
    
    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;

        _animator = GetComponent<Animator>();

        _statusUI = GetComponentInChildren<TextMeshProUGUI>();
        _statusCanvas = GetComponentInChildren<Canvas>();
        
        _allVertices = _enclosure.MeshGen.GetMesh().vertices;
    }

    // Update is called once per frame
    void Update() {

        if (!IsMouseWithinTarget()) {

            if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Walking") {
                _animator.Play("Walking");
                _status = MouseStates.Moving;
                _statusUI.text = _status.ToString();
            }
            
            OnMovementUpdate();
        }
        else {
            // Find new destination
            _status = MouseStates.Idle;
            _statusUI.text = _status.ToString();

            if (Random.Range(0f, 100f) <= 1f) {
                SetDestination(FindNewDestinationOutsideSensoryRadius());   
            }
        }
        
        
        
        _statusCanvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
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
        
        int index = Random.Range(0,list.Count);
        
        // Only go to a vertex that isn't occupied
        if (_enclosure.MeshGen.isPositionOccupied[index]) {
            return GetRandomVertex(list);
        }
        
        return list[Random.Range(0, list.Count)];
    }

    bool IsInsideRadius(Vector3 pos, float outerBounds = 1f) {

        if (Vector3.Distance(pos, transform.position) <= _sensoryRadius * outerBounds) {
            return true;
        }
        
        return false;
    }

    void OnMovementUpdate() {

        _hunger += 0.05f;
        _thirst += 0.1f;
        
        CheckNeeds();
    }
    
    void CheckNeeds() {

        if (_hunger >= 50f) {
            // needs to eat
           // Debug.Log("look for food");
        }

        if (_thirst >= 50f) {
            // needs to drink
           // Debug.Log("look for water");
        }
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
    
    void GetAllInSensoryRadius() {

        foreach (var item in _enclosure.ItemsInEnclosure) {
            if (Vector3.Distance(item.transform.position, transform.position) < _sensoryRadius) {
                _stress += item._stress;
            }
        }

        foreach (var mouse in _enclosure.MiceInEnclosure) {
            if (Vector3.Distance(mouse.transform.position, transform.position) < _sensoryRadius) {
                // do something with _socialisation
                // and _reproductiveUrge
            }
        }
    }
    
    public void SetDestination(Vector3 targetPos) {
        _navMeshAgent.destination = targetPos;
    }

    public void SetDestination(Transform target) {
        _navMeshAgent.destination = target.position;
    }

    private void OnDrawGizmosSelected() {
        //Gizmos.DrawWireSphere(transform.position, _sensoryRadius);
        //Gizmos.DrawWireSphere(transform.position, _boundaryRadius);
        
        if (_navMeshAgent.hasPath) {
            Gizmos.DrawLine(transform.position, _navMeshAgent.destination);
        }
    }
}
