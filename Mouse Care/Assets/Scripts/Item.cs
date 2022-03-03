using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int _id;
    private string _name;
    private string _description;
    
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
