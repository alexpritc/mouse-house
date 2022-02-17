using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.Mouse;

public class CameraRotation : MonoBehaviour {
    private Vector2 touchPos;

    public float rotationSpeed = 5f;

    private Controls controls;
    
    private bool _rot;
    private bool _pan;

    private Vector3 _touchStart;
    private Vector3 _direction;
    private float _groundZ = 0;
    
    private Camera _camera;

    private Vector2 turn;

    private bool f;

    private Vector3 worldMouse;
    
    private void Awake() {
        _camera = GetComponentInChildren<Camera>();
        controls = new Controls();
        
        controls.Camera.GetMouseStartPos.performed += ctx => _touchStart = cursorWorldPosOnNCP;
        
        controls.Camera.PanCamera.performed += ctx => _pan = true;
        controls.Camera.PanCamera.canceled += ctx => _pan = false;
        
        controls.Camera.RotateCamera.performed += ctx => _rot = true;
        controls.Camera.RotateCamera.canceled += ctx => _rot = false;
    }

    // Update is called once per frame
    void FixedUpdate() {

        worldMouse = cursorWorldPosOnNCP;

        if (_pan) {
            _direction = _touchStart - cursorWorldPosOnNCP;
            transform.position += _direction * 0.5f;
        }
        else if (_rot) {
            _direction = _touchStart - cursorWorldPosOnNCP;

            transform.Rotate(Vector3.up, _direction.x, Space.World);

           // float rotY = _direction.y;

           // rotY = Mathf.Clamp(rotY, 0f, 90f);

           // transform.Rotate(Vector3.right, rotY, Space.World);
            
            //turn.x += _direction.x;
            //turn.y += _direction.y;

            //-Mathf.Clamp(turn.y, 0f, 90f)

            //transform.localRotation = Quaternion.Euler(0f, turn.x * 0.003f, 0f);
        }
    }

    float ClampAngle (float angle, float min, float max) {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp (angle, min, max);
    }

    private static Vector3 cursorWorldPosOnNCP {
        get {
            return Camera.main.ScreenToViewportPoint(
                new Vector3(Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    Camera.main.nearClipPlane));
        }
    }
    private Vector3 GetWorldPosition(float z){
        Ray mousePos = _camera.ScreenPointToRay(Input.mousePosition);
        
        Plane ground = new Plane(Vector3.forward, new Vector3(0,0,z));
        
        float distance;
        
        ground.Raycast(mousePos, out distance);
        
        return mousePos.GetPoint(distance);
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
