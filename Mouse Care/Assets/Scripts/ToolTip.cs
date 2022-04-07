using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _message;
    [SerializeField] private GameObject _toolTipPrefab;

    private GameObject currentToolTip;

    [SerializeField] private bool _hasToolTip = true;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Click);   
    }

    public void Click()
    {
        GameManager.Instance.PlayAudio(0);
        RemoveToolTip();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_hasToolTip)
        {
            DisplayToolTip();   
        }
        GameManager.Instance.CursorEnterUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_hasToolTip)
        {
            RemoveToolTip();   
        }
        GameManager.Instance.CursorExitUI();
    }

    public void DisplayToolTip()
    {
        if (_hasToolTip)
        {
            Invoke("Display", 0.1f);   
        }
    }

    public void RemoveToolTip()
    {
        if (_hasToolTip)
        {
            GameManager.Instance.CursorExitUI();
            CancelInvoke("Display");
        
            if (currentToolTip != null)
            {
                currentToolTip.SetActive(false);
            }   
        }
    }

    private void Update()
    {
        if (currentToolTip != null && currentToolTip.activeInHierarchy)
        {
            currentToolTip.transform.position = Input.mousePosition;
        }
    }

    public void Display()
    {
        if (currentToolTip == null)
        {
            currentToolTip = Instantiate(_toolTipPrefab, transform.parent);
            currentToolTip.transform.position = Input.mousePosition;
            currentToolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _message;
        }
        else
        {
            currentToolTip.SetActive(true);
        }
    }
}
