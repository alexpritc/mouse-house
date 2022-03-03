using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator;
    string clipName;
    AnimatorClipInfo[] currentClipInfo;

    private GameObject currentOpenShop;

    [SerializeField] private Color buttonSelected;
    [SerializeField] private Color buttonNormal;

    public void PlayPanelAnim(GameObject shopButton)
    {
        //Access the Animation clip name
        clipName = panelAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        
        // If its closed or closing
        if (clipName == "PanelIdleClosed" || clipName == "PanelClosed")
        {
            if (currentOpenShop != null)
            {
                currentOpenShop.GetComponent<Image>().color = buttonNormal;
            }
            
            GameManager.Instance.IsInPlaceItemMode = true;
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
                GameManager.Instance.IsInPlaceItemMode = false;
                panelAnimator.CrossFade("Closing", 0.2f);
                shopButton.GetComponent<Image>().color = buttonNormal;
            }
            else
            {
                // No animations, just change contents
                if (currentOpenShop != null)
                {
                    currentOpenShop.GetComponent<Image>().color = buttonNormal;
                }
                GameManager.Instance.IsInPlaceItemMode = true;
                shopButton.GetComponent<Image>().color = buttonSelected;
                currentOpenShop = shopButton;
            }
        }
    }
}
