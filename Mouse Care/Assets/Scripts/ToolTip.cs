using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private string _message;
    [SerializeField] private GameObject _toolTipPrefab;
    [SerializeField] private Canvas infoCanvas;

    private GameObject currentToolTip;

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
            currentToolTip = Instantiate(_toolTipPrefab, infoCanvas.transform);
            currentToolTip.transform.position = transform.position + new Vector3(150f, -60f, 0f);
            currentToolTip.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = _message;
        }
        else
        {
            currentToolTip.SetActive(true);
        }
    }
}
