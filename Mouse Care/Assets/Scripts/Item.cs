using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum ItemType {
    Decoration,
    Exercise,
    Housing,
    Forage,
}

public class Item : MonoBehaviour
{

    [SerializeField] private string _name;
    //TextAreaAttribute(int minLines, int maxLines);
    [@TextAreaAttribute(5,10)]  [SerializeField] private string _description;
    [SerializeField] private bool _isUnlocked;
    [SerializeField] private float _yPos;
    public GameObject prefab;
    [SerializeField] private int _changeableMaterialIndex;
    [SerializeField] private Sprite _preview;
    public bool canPlaceOnTopOf;

    [SerializeField] private float _xOffset = 0f;
    [SerializeField] private float _zOffset = 0f;

    [Header("Ratings")] [SerializeField] private ItemType _itemType;
    
    /// <summary>
    /// How much enrichment this item gives
    /// </summary>
    public string _enrichment;
    
    /// <summary>
    /// How much stress this item gives
    /// </summary>
    public string _stress;
    
    public float XOffset
    {
        get => _xOffset;
    }
    
    public float ZOffset
    {
        get => _zOffset;
    }
    
    public float GetYPos()
    {
        if (GameManager.Instance.BeddingInches == 0f)
        {
            return _yPos;
        }
        else
        {
            return _yPos * GameManager.Instance.BeddingInches;
        }
    }

    public void ChangeColour(Color newColour)
    {
        GetComponent<MeshRenderer>().materials[_changeableMaterialIndex].color = newColour;
    }
    public Color GetColour()
    {
        return GetComponent<MeshRenderer>().materials[_changeableMaterialIndex].color;
    }

    public bool IsUnlocked
    {
        get => _isUnlocked;
    }
    
    public string Name
    {
        get => _name;
    }
    public string Description
    {
        get => _description;
    }
    
    /// <summary>
    /// Used for checking if all of the item is on the mesh
    /// </summary>
    public Transform[] corners;

    private bool _canSpawn = true;

    public bool CanSpawn
    {
        get => _canSpawn;
        set => _canSpawn = value;
    }

    public ItemType ItemType
    {
        get => _itemType;
        set => _itemType = value;
    }
    
    public Sprite Preview
    {
        get => _preview;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item") || other.CompareTag("Walls"))
        {
            _canSpawn = false;
        }
    }
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item") || other.CompareTag("Walls"))
        {
            _canSpawn = true;
        }
    }
}
