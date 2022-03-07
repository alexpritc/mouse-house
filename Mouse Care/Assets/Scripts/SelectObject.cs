using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    private Controls _controls;
    private GameObject _selected;

    public GameObject CurrentPrompt;
    public GameObject Prompt;
    public GameObject PromptCanvas;
    
    [SerializeField] private CameraController cc;
    
    private void Awake() {
        _controls = new Controls();
        _controls.GameManager.Select.performed += ctx => Select();
    }

    void Select()
    {
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _selected = hit.collider.gameObject;
            
            if (_selected.tag == "Mouse")
            {
                cc._target = _selected;
                cc._isFollowing = true;   
            }
            else
            {
                cc._target = null;
                cc._isFollowing = false;
            }
        }
    }
    
    public void CreatePrompt(Vector3 position, string message)
    {
        if (CurrentPrompt == null)
        {
            CurrentPrompt = Instantiate(Prompt, PromptCanvas.transform);
        }
        else
        {
            RemovePrompt();
        }
        
        CurrentPrompt.GetComponent<RectTransform>().anchoredPosition = WorldToPromtUI(position);
        TextMeshProUGUI promptText = CurrentPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        promptText.text = message;
    }
    
    private Vector2 WorldToPromtUI(Vector3 position)
    {
        RectTransform CanvasRect = PromptCanvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        return WorldObject_ScreenPosition;
    }
    
    public void RemovePrompt()
    {
        Destroy(CurrentPrompt);
    }
    
    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
