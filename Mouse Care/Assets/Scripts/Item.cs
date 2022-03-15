using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    [SerializeField] private bool _isUnlocked;

    [SerializeField] private float _yPos;
    
    public float YPos
    {
        get => _yPos;
    }
    
    public bool IsUnlocked
    {
        get => _isUnlocked;
    }
    
    public string Name
    {
        get => _name;
    }

    [SerializeField] private int _price;
    public int Price
    {
        get => _price;
    }
    
    public string PriceToString
    {
        get => _price.ToString();
    }
    
    /// <summary>
    /// Used for checking if all of the item is on the mesh
    /// </summary>
    public Transform[] corners;
    
    /// <summary>
    /// Used for checking if the item fits into the space
    /// </summary>
    public Transform[] widths;
    
    /// <summary>
    /// How much enrichment this item gives
    /// </summary>
    public float _enrichment;
    
    /// <summary>
    /// How much stress this item gives
    /// </summary>
    public float _stress;
}
