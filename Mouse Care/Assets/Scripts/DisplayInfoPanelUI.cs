using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfoPanelUI : MonoBehaviour
{
    [SerializeField] private Slider _hungerSlider;
    [SerializeField] private Slider _thirstSlider;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _statusText;
    [HideInInspector] public Mouse mouse;

    public void SetInitialValues(Mouse m)
    {
        mouse = m;
        _nameText.text = mouse.Name;
        _hungerSlider.value = mouse.Hunger;
        _thirstSlider.value = mouse.Thirst;
    }

    // Update is called once per frame
    void Update()
    {
        _hungerSlider.value = mouse.Hunger;
        _thirstSlider.value = mouse.Thirst;
        _statusText.text = mouse.Status.ToString();
    }
}
