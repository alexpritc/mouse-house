using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _name;
    //TextAreaAttribute(int minLines, int maxLines);
    [@TextAreaAttribute(5,10)]  [SerializeField] private string _description;
    [SerializeField] private bool _isUnlocked;
    [SerializeField] private float _yPos;
    public GameObject prefab;
    [SerializeField] private int _changeableMaterialIndex;

    public bool canPlaceOnTopOf;
    
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

    /// <summary>
    /// How much enrichment this item gives
    /// </summary>
    public string _enrichment;
    
    /// <summary>
    /// How much stress this item gives
    /// </summary>
    public string _stress;

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
