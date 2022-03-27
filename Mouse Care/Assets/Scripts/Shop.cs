 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

 public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject[] itemsInShop;
    [SerializeField] private GameObject itemUI;
    private int _totalPages = 1;

    [SerializeField] private GameObject _panel;
    private PlaceItems _placeItems;
    private PanelManager _panelManager;

    private List<GameObject> currentSlots;
    
    [SerializeField] private StatusPanelController _statusPanelController;

    private void Awake()
    {
        _placeItems = _panel.GetComponent<PlaceItems>();
        _panelManager = _panel.GetComponent<PanelManager>();
        currentSlots = new List<GameObject>();
        _totalPages = Mathf.RoundToInt(itemsInShop.Length / _panelManager.slots.Length);
    }

    public void DestroySlots()
    {
        if (currentSlots != null)
        {
            foreach (var go in currentSlots)
            {
                Destroy(go);
            }
        
            currentSlots.Clear();   
        }
        else
        {
            currentSlots = new List<GameObject>();
        }
    }
    
    public void FillSlots()
    {
        for (int i = 0; i < itemsInShop.Length; i++)
        {
            if (_panelManager.slots.Length > i)
            {
                GameObject go = Instantiate(itemUI, _panelManager.slots[i].position, Quaternion.identity, transform);
                go.GetComponent<Slot>().SetParams(this, itemsInShop[i]);
                currentSlots.Add(go);
            }
            else
            {
                return;
            }
        }
    }

    public void NewItemSelected(GameObject itemPrefab)
    {
        _placeItems.ItemPrefab = itemPrefab;
        _placeItems.ResetPreview(false);
        _panelManager.CloseCurrentPanel(true);
        _statusPanelController.ChangeMessage("Placing " + itemPrefab.GetComponent<Item>().Name);
    }
}
