using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enclosure : MonoBehaviour
{
    [SerializeField] private string _name;
    [@TextAreaAttribute(5,10)] [SerializeField] private string _description;
    
    /// <summary>
    /// Floorspace of enclosure in squared inches
    /// </summary>
    [SerializeField] private string _floorspace;
    
    /// <summary>
    /// How much bedding is in the enclosure
    /// </summary>
    [SerializeField] private int _beddingInInches;
    
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

    public int BeddingInInches
    {
        get => _beddingInInches;
    }
}
