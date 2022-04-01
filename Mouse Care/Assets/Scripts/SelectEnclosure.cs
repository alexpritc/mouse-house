using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SelectEnclosure : MonoBehaviour
{
    private GameObject CurrentInfoPanel;
    public GameObject InfoPanel;
    public GameObject InfoCanvas;
    
    [SerializeField] private LayerMask _layerMask;

    private GameObject _currentlySelected;

    private void Update()
    {
        if (!GameManager.Instance.IsCursorOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                if (_currentlySelected == null)
                {
                    _currentlySelected = hit.collider.gameObject;
                    CreatePrompt();   
                }
            }
            else
            {
                RemovePrompt();
                _currentlySelected = null;
            }
        }
    }

    public void CreatePrompt()
    {
        CurrentInfoPanel = Instantiate(InfoPanel, InfoCanvas.transform);
        CurrentInfoPanel.transform.position += new Vector3(100f, 2f, 0f);
        CurrentInfoPanel.GetComponent<DisplayEnclosurePanelUI>().SetInitialValues( _currentlySelected.GetComponentInParent<Enclosure>());
    }

    public void RemovePrompt()
    {
        if (CurrentInfoPanel != null)
        {
            Destroy(CurrentInfoPanel);   
        }
    }
}
