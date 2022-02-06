using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int _id;
    private string _name;
    private string _description;
    
    /// <summary>
    /// How much enrichment this item gives
    /// </summary>
    private float _enrichment;
    
    /// <summary>
    /// How much stress this item gives
    /// </summary>
    private float _stress;
}
