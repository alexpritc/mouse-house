using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRecommendations : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject _leftButtonIcon;
    [SerializeField] private GameObject _leftButton;
    
    [SerializeField] private GameObject _rightButtonIcon;
    [SerializeField] private GameObject _rightButton;

    private int _currentRecommendation = 0;

    [SerializeField] private TextMeshProUGUI _textbox;
    
    // Start is called before the first frame update
    void Awake()
    {
        SetButtonComponents(_leftButton, _leftButtonIcon, false);
        
        if (GameManager.Instance.Recommendations.Count -1 == 0)
        {
            SetButtonComponents(_rightButton, _rightButtonIcon, false);
        }
        else
        {
            SetButtonComponents(_rightButton, _rightButtonIcon, true);   
        }
        UpdateTextbox();
    }

    public void Next()
    {
        _currentRecommendation++;
        SetButtonComponents(_leftButton, _leftButtonIcon, true);
        
        if (_currentRecommendation >= GameManager.Instance.Recommendations.Count -1)
        {
            SetButtonComponents(_rightButton, _rightButtonIcon, false);
        }
        
        UpdateTextbox();
    }

    public void Previous()
    {
        _currentRecommendation--;
        SetButtonComponents(_rightButton, _rightButtonIcon, true);
        
        if (_currentRecommendation == 0)
        {
            SetButtonComponents(_leftButton, _leftButtonIcon, false);
        }
        
        UpdateTextbox();
    }
    
    private void SetButtonComponents(GameObject button, GameObject buttonIcon, bool enabled)
    {
        button.GetComponent<Button>().enabled = enabled;
        button.GetComponent<Image>().enabled = enabled;
        buttonIcon.GetComponent<Image>().enabled = enabled;
    }

    private void UpdateTextbox()
    {
        _textbox.text = GameManager.Instance.Recommendations[_currentRecommendation];
    }
}
