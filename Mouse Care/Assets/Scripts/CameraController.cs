using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.Mouse;

[RequireComponent(typeof(Rigidbody))]
public class CameraController : MonoBehaviour {
    [SerializeField] private float _horizontalRotationSpeed = 2f;
    [SerializeField] private float _verticalRotationSpeed = 1f;
    [SerializeField] private float _panningSpeed = 0.75f;
    [SerializeField] private float _movementSpeed = 1f;
    
    private Controls controls;
    
    private bool _isRotating;
    private bool _isPanning;
    
    private Vector3 _direction;

    [SerializeField] private GameObject _cameraPivot;
    [SerializeField] private GameObject _camera;

    private bool _isZooming;
    private float _zoomModifer;

    private float y;

    private Vector2 moveInput;

    private Vector2 mousePosThisFrame;
    private Vector2 mousePosLastFrame;

    private float pitch;
    private float yaw;

    [HideInInspector] public bool _isFollowing;
    [HideInInspector] public GameObject _target;

    private Vector3 _startPos;

    private void Awake() {
        controls = new Controls();

        controls.Camera.PanCamera.performed += ctx => _isPanning = true;
        controls.Camera.PanCamera.canceled += ctx => _isPanning = false;
        
        controls.Camera.RotateCamera.performed += ctx => _isRotating = true;
        controls.Camera.RotateCamera.canceled += ctx => _isRotating = false;

        controls.Camera.CameraZoom.performed += ctx => _isZooming = true;
        controls.Camera.CameraZoom.performed += ctx => _zoomModifer = ctx.ReadValue<Vector2>().y;
        controls.Camera.CameraZoom.canceled += ctx => _isZooming = false;

        controls.Camera.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Camera.Movement.canceled += ctx => moveInput = ctx.ReadValue<Vector2>();

        controls.Camera.Up.performed += ctx => y = 1f;
        controls.Camera.Down.performed += ctx => y = -1f;
        controls.Camera.Up.canceled += ctx => y = 0f;
        controls.Camera.Down.canceled += ctx => y = 0f;
        
        controls.Camera.MousePosition.performed += ctx => mousePosThisFrame = ctx.ReadValue<Vector2>();

        _startPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        _direction = mousePosLastFrame - mousePosThisFrame;
        
        Vector3 move = new Vector3(moveInput.x, y, moveInput.y);
        _cameraPivot.transform.Translate(move * _movementSpeed, Space.Self);

        if (_isFollowing && (move != Vector3.zero) || _isPanning )
        {
            _isFollowing = false;
        }
        
        if (_isFollowing)
        {
            Vector3 targetMove = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetMove, _movementSpeed);
        }

        if (transform.position != _startPos && !_isFollowing)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPos, _movementSpeed);
        }

        if (_isZooming)
        {
            _zoomModifer = Mathf.Clamp(_zoomModifer, -5f, 5f);
            _camera.transform.position += _camera.transform.forward * _zoomModifer;
        }
        else if (_isPanning && !GameManager.Instance.IsInPlaceItemMode)
        {
            // _direction = _touchStart - cursorWorldPosOnNCP;
            _cameraPivot.transform.Translate(_direction * _panningSpeed, Space.Self);
        }
        else if (_isRotating)
        {
            pitch += _direction.y * _verticalRotationSpeed;
            pitch = Mathf.Clamp(pitch, -20f, 70.0f);
            yaw += _direction.x * _horizontalRotationSpeed;

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        mousePosLastFrame = mousePosThisFrame;
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
