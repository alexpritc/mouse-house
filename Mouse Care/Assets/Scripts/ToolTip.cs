using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _message;
    [SerializeField] private GameObject _toolTipPrefab;
    [SerializeField] private Canvas infoCanvas;

    private GameObject currentToolTip;

    [SerializeField] private float _xOffset = 160f;
    [SerializeField] private float _yOffset = -60f;

    [SerializeField] private bool _hasToolTip = true;

    [SerializeField] private AudioClip _clickAudioClip;
    
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
            Invoke("Display", 1f);   
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
