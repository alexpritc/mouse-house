using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickEnclosure : MonoBehaviour
{
    [Header("Enclosures in the scene")]
    [SerializeField] private GameObject[] _enclosuresInScene;
    [SerializeField] private GameObject[] _enclosurePrefabs;
    private int _currentEnclosure;

    [Header("Buttons")]
    [SerializeField] private GameObject _leftButton;
    [SerializeField] private GameObject _rightButton;
    
    [Header("Move the camera")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _movementSpeed = 100f;
    
    private void Awake()
    {
        _currentEnclosure = 0;
        _leftButton.SetActive(false);
        _rightButton.SetActive(true);
    }

    private void Update()
    {
        Vector3 targetMove = new Vector3(_enclosuresInScene[_currentEnclosure].transform.position.x,
            _camera.transform.position.y, _camera.transform.position.z);
        _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, targetMove, _movementSpeed * Time.deltaTime);
    }

    public void NextEnclosure()
    {
        _currentEnclosure++;
        NewEnclosure();

        // Hide/show the correct buttons
        _leftButton.SetActive(true);
        if (_currentEnclosure == _enclosuresInScene.Length - 1)
        {
            _rightButton.SetActive(false);
        }
    }

    public void PreviousEnclosure()
    {
        _currentEnclosure--;
        NewEnclosure();
        
        // Hide/show the correct buttons
        _rightButton.SetActive(true);
        if (_currentEnclosure == 0)
        {
            _leftButton.SetActive(false);
        }
    }


    void NewEnclosure()
    {
        // Reset the rotation of the last enclosure
        _enclosuresInScene[_currentEnclosure].transform.rotation =
            new Quaternion(0f, 0f, 0f, _enclosuresInScene[_currentEnclosure].transform.rotation.w);
        
        // Be able to modify the rotation of the new current enclosure
        _camera.GetComponent<CameraController>().CurrentEnclosure = _enclosuresInScene[_currentEnclosure];
    }
    
    public void ConfirmSelection()
    {
        GameManager.Instance.SpawnEnclosure(_enclosurePrefabs[_currentEnclosure]);
    }
}
