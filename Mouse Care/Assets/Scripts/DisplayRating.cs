using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DisplayRating : MonoBehaviour
{
    private TextMeshProUGUI _scoreTextbox;
    [SerializeField] private TextMeshProUGUI _letterTextbox;
    private int _total;
    private int _current = 00000;
    
    public Camera Camera;

    private float _viewportWidthNormal = 0f;
    private float _viewportWidthInfoMode = 0.3f;
    
    private float x = 0f;
    private float speed = 0.0000001f;

    [SerializeField] private Animator RecommendationsPanel;
    public bool toggle;
    void Awake()
    {
        _scoreTextbox = GetComponent<TextMeshProUGUI>();
        _total = GameManager.Instance.Score;
    }

    // Update is called once per frame
    void Update()
    {
        if (_current < _total)
        {
            StartCoroutine(AddToScoreDisplay());
        }
        
        if (toggle)
        {
            x = Mathf.Lerp(_viewportWidthInfoMode, _viewportWidthNormal, speed * Time.deltaTime);
        }
        else
        {
            x = Mathf.Lerp(_viewportWidthNormal, _viewportWidthInfoMode, speed * Time.deltaTime);
        }
        
        Camera.rect = new Rect(x, 0.0f, 1.0f, 1.0f);
    }

    public void ToggleRecommendations()
    {
        toggle = !toggle;

        if (toggle)
        {
            RecommendationsPanel.CrossFade("Hidden_To_Visible", 0.2f);
        }
        else
        {
            RecommendationsPanel.CrossFade("Visible_To_Hidden", 0.2f);
        }
    }
    
    private IEnumerator AddToScoreDisplay()
    {
        string temp = "";
        _current += 20;

        if (_current < 10)
        {
            temp = "0000";
        }
        else if (_current < 100)
        {
            temp = "000";
        }
        else if (_current < 1000)
        {
            temp = "00";
        }
        else if (_current < 10000)
        {
            temp = "0";
        }

        if (_current < 10000)
        {
            _letterTextbox.text = "F";
        }
        else if (_current < 20000)
        {
            _letterTextbox.text = "E";
        }
        else if (_current < 35000)
        {
            _letterTextbox.text = "D";
        }
        else if (_current < 50000)
        {
            _letterTextbox.text = "C";
        }
        else if (_current < 65000)
        {
            _letterTextbox.text = "B";
        }
        else if (_current < 75000)
        {
            _letterTextbox.text = "A";
        }
        else if (_current < 90000)
        {
            _letterTextbox.text = "S";
        }

        if (_current >= GameManager.Instance.Score)
        {
            _current = GameManager.Instance.Score;
            
            string clipInfo = RecommendationsPanel.GetCurrentAnimatorClipInfo(0)[0].clip.name;

            if (clipInfo != "Visible" && clipInfo != "Hidden_To_Visible")
            {
                RecommendationsPanel.CrossFade("Hidden_To_Visible", 0.2f); 
                toggle = true;
            }
        }

        _scoreTextbox.text = temp + _current.ToString();
        yield return new WaitForEndOfFrame();
    }
}
