using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    private GameObject _highlightedObject;
    [SerializeField] private LayerMask _layerMask;

    private void Update()
    {
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

    void Highlight()
    {
        if (!GameManager.Instance.IsCursorOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                if (_highlightedObject != null)
                {
                    _highlightedObject.GetComponent<Outline>().enabled = false;
                }
                
                _highlightedObject = hit.collider.transform.parent.gameObject;
                _highlightedObject.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if (_highlightedObject != null)
                {
                    _highlightedObject.GetComponent<Outline>().enabled = false;
                }

                _highlightedObject = null;
            }
        }
    }
}
