using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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

    private SelectObject so;

    public Transform ItemTransForm;
    
    private void Awake()
    {
        so = GameObject.FindObjectOfType<SelectObject>();
    }

    public void SetInitialValues(Item i)
    {
        Item = i;
        _nameText.text = Item.Name;
        _descriptionText.text = Item.Description;
        _stressText.text = Item._stress;
        _enrichmentText.text = Item._enrichment;
        
        transform.GetChild(0).gameObject.SetActive(Item.isInfoOpen);
        transform.GetChild(4).gameObject.SetActive(Item.isColourOpen);
        transform.GetChild(4).GetComponent<FlexibleColorPicker>().color = GetItemColour();
    }

    public void TogglePanel()
    {
        Item.isInfoOpen = !Item.isInfoOpen;
        foreach (var item in GameManager.Instance.Items)
        {
            if (item.Name == Item.Name)
            {
                item.isInfoOpen = Item.isInfoOpen;
            }
        }
        transform.GetChild(0).gameObject.SetActive(Item.isInfoOpen);
    }

    public void ToggleColour()
    {
        Item.isColourOpen = !Item.isColourOpen;
        foreach (var item in GameManager.Instance.Items)
        {
            if (item.Name == Item.Name)
            {
                item.isColourOpen = Item.isColourOpen;
            }
        }
        transform.GetChild(4).gameObject.SetActive(Item.isColourOpen);
        transform.GetChild(4).GetComponent<FlexibleColorPicker>().color = GetItemColour();
    }
    
    public void DestroyGameObject()
    {
        so.Remove();
        GameManager.Instance.RemoveFromItems(Item);
        GameManager.Instance.CursorExitUI();
        Destroy(Item.gameObject);
    }

    public void MoveObject()
    {
        ItemTransForm = Item.gameObject.transform;
        panelManager.GetComponent<PlaceItems>().ItemPrefab = Item.prefab;
        GameManager.Instance.IsInPlaceItemMode = true;
        GameManager.Instance.CursorExitUI();
        GameManager.Instance.RemoveFromItems(Item);
        panelManager.GetComponent<PlaceItems>().ResetPreview(ItemTransForm, true);
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
