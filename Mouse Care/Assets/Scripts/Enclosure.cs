using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enclosure : MonoBehaviour
{
    [SerializeField] private string _name;
    [@TextAreaAttribute(5,10)] [SerializeField] private string _description;
    
    /// <summary>
    /// Floorspace of enclosure
    /// </summary>
    [SerializeField] private string _floorspace;

    /// <summary>
    /// Internal dimensions
    /// </summary>
    [SerializeField] private string _internalDimensions;


    /// <summary>
    /// How much bedding is in the enclosure
    /// </summary>
    [SerializeField] private string _beddingInInches;

    /// <summary>
    /// How many mice can this enclosure comfortably hold
    /// </summary>
    [SerializeField] private string _mice;

    
    public Transform[] Targets;

    public GameObject Bedding;

    public float Radius;

    public string Name
    {
        get => _name;
    }
    
    public string Description
    {
        get => _description;
    }
    
    public string Floorspace
    {
        get => _floorspace;
    }
    
    public string Dimensions
    {
        get => _internalDimensions;
    }
    
    public string BeddingInInches
    {
        get => _beddingInInches;
    }
    
    public string Mice
    {
        get => _mice;
    }
}
