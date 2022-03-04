using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.Mouse;

[RequireComponent(typeof(Rigidbody))]
public class CameraController : MonoBehaviour {
    private Vector2 touchPos;

    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _panningSpeed = 0.5f;
    [SerializeField] private float _movementSpeed = 1f;
    
    private Controls controls;
    
    private bool _isRotating;
    private bool _isPanning;

    private Vector3 _touchStart;
    private Vector3 _direction;

    [SerializeField] private GameObject _cameraPivot;
    [SerializeField] private GameObject _camera;

    private Vector2 turn;

    private float _rot;

    private bool _isZooming;
    private float _zoomModifer;

    private float y;

    private Vector2 moveInput;

    private void Awake() {
        controls = new Controls();
        
        controls.Camera.GetMouseStartPos.performed += ctx => _touchStart = cursorWorldPosOnNCP;
        
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
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isZooming)
        {
            _zoomModifer = Mathf.Clamp(_zoomModifer, -5f, 5f);
            _camera.transform.position += _camera.transform.forward * _zoomModifer;
        }
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, y, moveInput.y);
        _cameraPivot.transform.Translate(move * _movementSpeed, Space.Self);
        
        if (_isPanning && !GameManager.Instance.IsInPlaceItemMode)
        {
            _direction = _touchStart - cursorWorldPosOnNCP;
            _cameraPivot.transform.Translate(_direction * _panningSpeed, Space.Self);
        }
        else if (_isRotating) {
            _direction = _touchStart - cursorWorldPosOnNCP;

            // Standardises rotation speed
            _direction = StandardiseRotationSpeed(_direction);

            transform.Rotate(Vector3.up, _direction.x * _rotationSpeed);
        }
    }

    private Vector3 StandardiseRotationSpeed(Vector3 inVector)
    {
        float x = 0f, y = 0f, z = 0f;

        if (inVector.x < 0)
        {
            x = -1f;
        }
        else if (inVector.x > 0)
        {
            x = 1f;
        }

        if (inVector.y < 0)
        {
            y = -1f;
        }
        else if (inVector.y > 0)
        {
            y = 1f;
        }

        if (inVector.z < 0)
        {
            z = -1f;
        }
        else if (inVector.z > 0)
        {
            z = 1f;
        }

        return new Vector3(x, y, z);

    }

    private static Vector3 cursorWorldPosOnNCP {
        get {
            return Camera.main.ScreenToViewportPoint(
                new Vector3(Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    Camera.main.nearClipPlane));
        }
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
