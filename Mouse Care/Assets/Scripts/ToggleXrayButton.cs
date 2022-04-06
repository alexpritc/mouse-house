using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleXrayButton : MonoBehaviour
{
    public Image icon;
    public Sprite on, off;
    
    private void Awake()
    {
        if (GameManager.Instance.GetComponent<LookIntoEnclosure>().isEnabled)
        {
            icon.sprite = on;
        }
        else
        {
            icon.sprite = off;
        }
        
        GetComponent<Button>().onClick.AddListener(Click);
    }

    public void Click()
    {
        GameManager.Instance.GetComponent<LookIntoEnclosure>().ToggleXray();

        if (icon.sprite == on)
        {
            icon.sprite = off;
        }
        else if (icon.sprite == off)
        {
            icon.sprite = on;
        }
    }

}
