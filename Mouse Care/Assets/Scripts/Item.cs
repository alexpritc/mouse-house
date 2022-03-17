using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private bool _isUnlocked;
    [SerializeField] private float _yPos;

    public float GetYPos()
    {
        if (GameManager.Instance.GameState == GameState.DecorFloor)
        {
            return 0f;
        }
        else
        {
            return _yPos * GameManager.Instance.BeddingInches;
        }
    }

    public bool IsUnlocked
    {
        get => _isUnlocked;
    }
    
    public string Name
    {
        get => _name;
    }

    /// <summary>
    /// Used for checking if all of the item is on the mesh
    /// </summary>
    public Transform[] corners;

    /// <summary>
    /// How much enrichment this item gives
    /// </summary>
    public float _enrichment;
    
    /// <summary>
    /// How much stress this item gives
    /// </summary>
    public float _stress;

    private bool _canSpawn = true;

    public bool CanSpawn
    {
        get => _canSpawn;
        set => _canSpawn = value;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            _canSpawn = false;
        }
    }
    
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            _canSpawn = true;
        }
    }
}
