using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _message;
    [SerializeField] private GameObject _toolTipPrefab;
    [SerializeField] private Canvas infoCanvas;

    private GameObject currentToolTip;

    [SerializeField] private float _xOffset = 160f;
    [SerializeField] private float _yOffset = -60f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayToolTip();
        GameManager.Instance.CursorEnterUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RemoveToolTip();
        GameManager.Instance.CursorExitUI();
    }
    
    public void DisplayToolTip()
    {
        Invoke("Display", 1f);
    }

    public void RemoveToolTip()
    {
        CancelInvoke("Display");
        
        if (currentToolTip != null)
        {
            currentToolTip.SetActive(false);
        }
    }

    public void Display()
    {
        if (currentToolTip == null)
        {
            if (infoCanvas == null)
            {
                currentToolTip = Instantiate(_toolTipPrefab, transform.parent);
            }
            else
            {
                currentToolTip = Instantiate(_toolTipPrefab, infoCanvas.transform);
            }
            currentToolTip.transform.position = transform.position + new Vector3(_xOffset, _yOffset, 0f);
            currentToolTip.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = _message;
        }
        else
        {
            currentToolTip.SetActive(true);
        }
    }
}
