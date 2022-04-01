using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMode : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private float _viewportWidthNormal = 0f;
    private float _viewportWidthInfoMode = 0.3f;

    private float x = 0f;
    private float speed = 0.0000001f;
    private float buttonSpeed = 10f;

    private float _leftButtonNormal = -310f;
    private float _leftButtonInfoMode = -120f;
    
    private float _confirmButtonNormal;
    private float _confirmButtonInfoMode;

    [SerializeField] private Animator _leftButton;
    [SerializeField] private Animator _confirmButton;

    [SerializeField] private Animator InfoPanel;

    private bool isInfoMode = false;

    // Update is called once per frame
    void Update()
    {
        if (!isInfoMode)
        {
            x = Mathf.Lerp(_viewportWidthNormal, _viewportWidthInfoMode, speed * Time.deltaTime);
        }
        else
        {
            x = Mathf.Lerp(_viewportWidthInfoMode, _viewportWidthNormal, speed * Time.deltaTime);
        }
        
        _camera.rect = new Rect(x, 0.0f, 1.0f, 1.0f);
    }

    public void Toggle()
    {
        if (isInfoMode)
        {
            // Back to normal mode
            isInfoMode = false;
            _leftButton.CrossFade("LeftButton_InfoMode_To_Normal", 0.2f);
            _confirmButton.CrossFade("ConfirmButton_InfoMode_To_Normal", 0.2f);
            InfoPanel.CrossFade("Visible_To_Hidden", 0.2f);
        }
        else
        {
            // Info mode
            isInfoMode = true;
            _leftButton.CrossFade("LeftButton_Normal_To_InfoMode", 0.2f);
            _confirmButton.CrossFade("ConfirmButton_Normal_To_InfoMode", 0.2f);
            InfoPanel.CrossFade("Hidden_To_Visible", 0.2f);
        }
    }
}
