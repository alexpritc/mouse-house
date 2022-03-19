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
    public GameObject panelManager;

    public void SetInitialValues(Item i)
    {
        Item = i;
        _nameText.text = Item.Name;
        _descriptionText.text = Item.Description;
        _stressText.text = Item._stress;
        _enrichmentText.text = Item._enrichment;
    }

    public void DestroyGameObject()
    {
        GameManager.Instance.gameObject.GetComponent<SelectObject>().Remove();
        GameManager.Instance.CursorExitUI();
        Destroy(Item.gameObject);
    }

    public void MoveObject()
    {
        panelManager.GetComponent<PlaceItems>().ItemPrefab = Item.prefab;
        GameManager.Instance.IsInPlaceItemMode = true;
        GameManager.Instance.CursorExitUI();
        panelManager.GetComponent<PlaceItems>().ResetPreview(true);
        Item.gameObject.SetActive(false);
    }

    public void UpdateColour(Color newColour)
    {
        Item.ChangeColour(newColour);
    }

    public Color GetItemColour()
    {
        return Item.GetColour();
    }
}
