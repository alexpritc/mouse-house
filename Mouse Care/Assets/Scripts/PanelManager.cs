using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    private Animator panelAnimator;
    string clipName;
    AnimatorClipInfo[] currentClipInfo;

    private GameObject currentOpenShop;

    [SerializeField] private Color buttonSelected;
    [SerializeField] private Color buttonNormal;

    [SerializeField] private Shop[] shops;
    [SerializeField] private GameObject[] buttons;

    public Transform[] slots;
    private int _currentPage = -1;

    private void Start()
    {
        panelAnimator = GetComponent<Animator>();
    }

    public void CloseCurrentPanel(bool isItemSelected)
    {
        panelAnimator.CrossFade("Closing", 0.2f);
        GameManager.Instance.IsInPlaceItemMode = true;
    }
    
    public void OpenCurrentPanel(bool isItemSelected)
    {
        panelAnimator.CrossFade("Opening", 0.2f);
        GameManager.Instance.IsInPlaceItemMode = true;
    }

    private int FindShopIndex(GameObject button)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (button.name == buttons[i].name)
            {
                return i;
            }
        }

        return -1;
    }

    private void SetAllShopsInactive()
    {
        foreach (var shop in shops)
        {
            shop.gameObject.SetActive(false);
            shop.DestroySlots();
        }
    }
    
    public void OpenShopPanel(GameObject shopButton)
    {
        //Access the Animation clip name
        clipName = panelAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        GameManager.Instance.IsInPlaceItemMode = false;

        _currentPage = 0;
        
        SetAllShopsInactive();
        shops[FindShopIndex(shopButton)].gameObject.SetActive(true);
        shops[FindShopIndex(shopButton)].FillSlots();

        // If its closed or closing
        if (clipName == "PanelIdleClosed" || clipName == "PanelClosed")
        {
            if (currentOpenShop != null)
            {
                currentOpenShop.GetComponent<Image>().color = buttonNormal;
            }
            shopButton.GetComponent<Image>().color = buttonSelected;
            panelAnimator.CrossFade("Opening", 0.2f);
            currentOpenShop = shopButton;
        }

        // If its open or opening
        else if (clipName == "PanelOpen" || clipName == "PanelIdleOpen")
        {
            // Only close if pressing the same button twice
            if (currentOpenShop == shopButton)
            {
                // TODO: Change this later on once buying items has been added
                panelAnimator.CrossFade("Closing", 0.2f);
                shopButton.GetComponent<Image>().color = buttonNormal;
                _currentPage = -1;
            }
            else
            {
                // No animations, just change contents
                if (currentOpenShop != null)
                {
                    currentOpenShop.GetComponent<Image>().color = buttonNormal;
                }
                shopButton.GetComponent<Image>().color = buttonSelected;
                currentOpenShop = shopButton;
            }
        }
    }
}
