using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    private Controls _controls;
    public GameObject _selected;

    private GameObject CurrentInfoPanel;
    public GameObject InfoPanel;
    public GameObject InfoCanvas;

    [SerializeField] private GameObject _panel;
    
    [SerializeField] private CameraController cc;

    [SerializeField] private LayerMask _layerMask;

    private GameObject _mouseInfo;
    
    private void Awake() {
        _controls = new Controls();
        _controls.GameManager.Select.performed += ctx => Select();
    }

    private void Update()
    {
        if (GameManager.Instance.IsInPlaceItemMode || GameManager.Instance.IsShopOpen)
        {
            RemovePrompt();
            return;
        }
    }

    void Select()
    {
        if (GameManager.Instance.IsInPlaceItemMode || GameManager.Instance.IsShopOpen)
        {
            Remove();
            return;
        }
        
        if (!GameManager.Instance.IsCursorOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                if (_selected != null)
                {
                    _selected.GetComponent<Outline>().enabled = false;
                }
                
                _selected = hit.collider.transform.parent.gameObject;
                _selected.GetComponent<Outline>().enabled = true;
                
                if (_selected.tag == "Item")
                {
                    cc._target = _selected;
                    cc._isFollowing = true;
                    RemovePrompt();
                    CreatePrompt();
                }
                else if (_selected.tag != "UI")
                {
                    Remove();
                }
            }
            else
            {
                Remove();
            }
        }
        else
        {
            // check if the UI is related to the object or not
        }
    }

    public void Remove()
    {
        cc._target = null;
        cc._isFollowing = false;
        if (_selected != null)
        {
            _selected.GetComponent<Outline>().enabled = false;
        }
        _selected = null;
        RemovePrompt();
    }
    
    public void CreatePrompt()
    {
        InfoPanel.SetActive(true);
        InfoPanel.GetComponent<DisplayInfoPanelUI>().SetInitialValues( _selected.GetComponentInParent<Item>());
        InfoPanel.GetComponent<DisplayInfoPanelUI>().panelManager = _panel;
    }

    public void RemovePrompt()
    {
        InfoPanel.SetActive(false);
    }
    
    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
