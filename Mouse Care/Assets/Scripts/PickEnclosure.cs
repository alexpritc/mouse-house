using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickEnclosure : MonoBehaviour
{
    [SerializeField] private GameObject[] _enclosures;
    private int _currentEnclosure;

    [SerializeField] private GameObject _leftButton;
    [SerializeField] private GameObject _rightButton;

    private void Awake()
    {
        _currentEnclosure = 0;
        _leftButton.SetActive(false);
        _rightButton.SetActive(true);
    }

    public void NextEnclosure()
    {
        _currentEnclosure++;
        
        _leftButton.SetActive(true);
        if (_currentEnclosure == _enclosures.Length - 1)
        {
            _rightButton.SetActive(false);
        }
    }

    public void PreviousEnclosure()
    {
        _currentEnclosure--;

        _rightButton.SetActive(true);
        if (_currentEnclosure == 0)
        {
            _leftButton.SetActive(false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
