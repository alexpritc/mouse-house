using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusPanelController : MonoBehaviour
{
    private Animator _animator;

    private bool lastFrame;

    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            if (!lastFrame)
            {
                _animator.Play("Opening");
            }
        }
        else
        {
            if (lastFrame)
            {
                _animator.Play("Closing");
            }
        }
        
        lastFrame = GameManager.Instance.IsInPlaceItemMode;
    }

    public void ClosePanel()
    {
        _animator.Play("Closing");
        GameManager.Instance.IsInPlaceItemMode = false;
        GameManager.Instance.IsShopOpen = false;
    }
    
    public void ChangeMessage(string message)
    {
        tmp.text = message;
    }
}
