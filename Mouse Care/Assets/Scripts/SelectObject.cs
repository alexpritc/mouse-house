using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    private Controls _controls;
    private GameObject _selected;

    private GameObject CurrentInfoPanel;
    public GameObject InfoPanel;
    public GameObject InfoCanvas;
    
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
            RemovePrompt();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~_layerMask))
        {
            _selected = hit.collider.gameObject;
            
            if (_selected.tag == "Item")
            {
                cc._target = _selected;
                cc._isFollowing = true;
                RemovePrompt();
                CreatePrompt(_selected.tag);
            }
            else
            {
                cc._target = null;
                cc._isFollowing = false;
                RemovePrompt();
            }
        }
    }

    public void CreatePrompt(string message)
    {
        CurrentInfoPanel = Instantiate(InfoPanel, InfoCanvas.transform);
        //CurrentInfoPanel.GetComponent<DisplayInfoPanelUI>().SetInitialValues(_selected.GetComponentInParent<Mouse>());
    }

    private Vector2 WorldToUI(Vector3 position)
    {
        RectTransform CanvasRect = InfoCanvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToScreenPoint(position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        return WorldObject_ScreenPosition;
    }
    
    public void RemovePrompt()
    {
        if (CurrentInfoPanel != null)
        {
            Destroy(CurrentInfoPanel);   
        }
    }
    
    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
