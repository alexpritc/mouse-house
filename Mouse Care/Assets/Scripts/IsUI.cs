using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class IsUI : MonoBehaviour
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.CursorEnterUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.CursorExitUI();
    }
    
    public void Enter()
    {
        GameManager.Instance.CursorEnterUI();
    }

    public void Exit()
    {
        GameManager.Instance.CursorExitUI();
    }
}
