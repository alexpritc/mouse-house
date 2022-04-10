using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    private GameObject _highlightedObject;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private SelectObject so;

    private GameObject lastSelected;
    
    private void Update()
    {
        ClearAllHighlights();
        
        if (GameManager.Instance.IsInPlaceItemMode || GameManager.Instance.IsShopOpen || GameManager.Instance.IsCursorOverUI)
        {
            if (_highlightedObject != null)
            {
                _highlightedObject.GetComponent<Outline>().enabled = false;
            }
            _highlightedObject = null;
            return;
        }
        
        Highlight();
    }

    void ClearAllHighlights()
    {
        foreach (var item in GameManager.Instance.Items)
        {
            if (item.gameObject != _highlightedObject && item.gameObject != so._selected)
            {
                item.GetComponent<Outline>().enabled = false;
            }
        }
    }
    
    void Highlight()
    {
        if (!GameManager.Instance.IsCursorOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                if (_highlightedObject != null && _highlightedObject != so._selected)
                {
                    _highlightedObject.GetComponent<Outline>().enabled = false;
                }
                
                _highlightedObject = hit.collider.transform.parent.gameObject;
                _highlightedObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if (_highlightedObject != null && _highlightedObject != so._selected)
                {
                    _highlightedObject.GetComponent<Outline>().enabled = false;
                }

                _highlightedObject = null;
            }
        }
    }
}
