using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {
    private Vector2 touchPos;
    bool aTouch= false;

    public float rotationSpeed = 5f;

    private Controls controls;
    
    public bool rotHorizontal;

    private Vector2 input;
    
    private void Awake() {
        controls = new Controls();
        
        controls.Camera.RotateHorizontal.performed += ctx => rotHorizontal = true;
        controls.Camera.RotateHorizontal.canceled += ctx => rotHorizontal = false;
    }

    // Update is called once per frame
    void Update() {
        if (rotHorizontal) {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
    
    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
