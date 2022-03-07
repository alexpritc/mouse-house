 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private Transform[] slots;
    [SerializeField] private GameObject[] itemPrefabs;
    private int pageNumber = 1;
    private int totalPages = 1;

    [SerializeField] private GameObject _panel;
    private PlaceItems _placeItems;
    
    private PanelManager _panelManager;

    private void Start()
    {
        _placeItems = GetComponent<PlaceItems>();
        _panelManager = _panel.GetComponent<PanelManager>();
    }

    public void NewItemSelected(GameObject itemPrefab)
    {
        _placeItems.ItemPrefab = itemPrefab;
        _placeItems.ResetPreview();
        _panelManager.CloseCurrentPanel(true);
    }
}
