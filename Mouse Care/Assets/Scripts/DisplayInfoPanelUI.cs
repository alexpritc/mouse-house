using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfoPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _stressText;
    [SerializeField] private TextMeshProUGUI _enrichmentText;
    [HideInInspector] public Item Item;

    public void SetInitialValues(Item i)
    {
        Item = i;
        _nameText.text = Item.Name;
        _descriptionText.text = Item.Description;
    }

    public void DestroyGameObject()
    {
        GameManager.Instance.gameObject.GetComponent<SelectObject>().Remove();
        GameManager.Instance.CursorExitUI();
        Destroy(Item.gameObject);
    }
}
