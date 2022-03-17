using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfoPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [HideInInspector] public Item Item;

    public void SetInitialValues(Item i)
    {
        Item = i;
        _nameText.text = Item.Name;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
