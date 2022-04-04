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

        if (_current > GameManager.Instance.Score)
        {
            _current = GameManager.Instance.Score;
        }
        
        _scoreTextbox.text = temp + _current.ToString();
        yield return new WaitForEndOfFrame();
    }
}
