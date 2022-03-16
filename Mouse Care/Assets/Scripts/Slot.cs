using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    private bool _isUnlocked;
    private Image _preview;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Button _button;

    private Item _item;
    private GameObject _prefab;
    private Shop _shop;
    private int _id;

    [SerializeField] private Color _buttonColorNormal;
    [SerializeField] private Color _buttonColorIsNotUnlocked;

    private Animator _animator;
    private bool _wasTooExpensive = false;
    private bool _wasUnlocked = true;
    
    private void Awake()
    {
        _button.onClick.AddListener(Click);
    }

    private void Start()
    {
        _preview = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }

    private void Click()
    {
        _shop.NewItemSelected(_prefab);
    }

    private void Update()
    {
        if (_isUnlocked)
        {
            if (!_wasUnlocked)
            {
                _animator.Play("SlotPulse");
                _wasUnlocked = true;
            }
        }
        else
        {
            _wasUnlocked = false;
            _button.GetComponent<Image>().color = _buttonColorIsNotUnlocked;
        }
    }
    
    public void SetParams(Shop shop, GameObject prefab)
    {
        _item = prefab.GetComponent<Item>();
        _name.text = _item.Name;
        _shop = shop;
        _prefab = prefab;
        
        _button.interactable = _item.IsUnlocked;
        _isUnlocked = _item.IsUnlocked;
    }
}
