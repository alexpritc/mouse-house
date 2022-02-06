using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enclosure : MonoBehaviour
{
    private int _id;
    private string _name;

    /// <summary>
    /// Floorspace of enclosure in squared inches
    /// </summary>
    private int _size;
    
    private float _water;
    private float _food;
    private float _enrichment;
    /// <summary>
    /// Type of bedding
    /// </summary>
    private string _bedding;
    /// <summary>
    /// How much bedding is in the enclosure
    /// </summary>
    private float _beddingInInches;
    private int _maxMiceCapacity;
}
