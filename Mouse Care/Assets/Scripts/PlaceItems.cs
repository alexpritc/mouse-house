using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PlaceItems : MonoBehaviour
{
    private GameObject _itemPrefab;

    public GameObject ItemPrefab
    {
        get => _itemPrefab;
        set => _itemPrefab = value;
    }
    
    private Controls _controls;
    
    private GameObject _preview;
    [SerializeField] private Material _previewMat;
    [SerializeField] private Material _previewMatRed;

    [SerializeField] private LayerMask _meshLayer;
    
    private RaycastHit hitLastTimeWasOnMesh;

    private Item _previewItem;

    public bool isMovingExistingItem;
    public Transform ExistingItemTransform;

    private bool _lastFrame;
    

    public void ResetPreview(Transform itemTransform, bool isExistingItem = false)
    {
        if (_preview != null)
        {
            Destroy(_preview);
        }
        
        _preview = Instantiate(_itemPrefab);  
        _preview.gameObject.name = "Preview";

        foreach (var col in _preview.GetComponents<BoxCollider>())
        {
            col.isTrigger = true;
        }
        
        _preview.GetComponent<MeshRenderer>().material = _previewMat;
        Destroy(_preview.GetComponent<NavMeshObstacle>());
        Destroy(_preview.GetComponentInChildren<NavMeshObstacle>()); 
        Destroy(_preview.GetComponentInChildren<AudioSource>()); 
        
        _previewItem = _preview.GetComponent<Item>();

        if (_previewItem.canPlaceOnTopOf)
        {
            Destroy(_preview.transform.GetChild(0).gameObject);
        }

        if (!GameManager.Instance.IsInPlaceItemMode)
        {
            _preview.SetActive(false);
        }

        isMovingExistingItem = isExistingItem;
        ExistingItemTransform = itemTransform;
    }

    private void Awake() {
        _controls = new Controls();

        _controls.Item.PlaceItem.performed += ctx => PlaceItem();
        _controls.Item.RotateItem.performed += ctx => RotateItem();
        _preview = null;
    }

    private void Update()
    {
        if (GameManager.Instance.isGamePaused)
        {
            return;
        }
        
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            if (_preview == null)
            {
                return;
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _meshLayer))
            {
                if (!_preview.activeSelf)
                {
                    _preview.SetActive(true);
                }

                if (_previewItem.CanSpawn && IsOnMesh())
                {
                    ChangeMaterial(_preview, _previewMat);
                }
                else
                {
                    ChangeMaterial(_preview, _previewMatRed);
                }

                _preview.transform.position =
                    hit.point - new Vector3(_previewItem.XOffset, _previewItem.GetYPos(), _previewItem.ZOffset);
            }
            else
            {
                _preview.SetActive(false);
            }
        }
        else
        {
            if (_preview != null)
            {
                _preview.SetActive(false);   
            }
        }

        if (_lastFrame)
        {
            // Was in place item mode but no longer is
            if (!GameManager.Instance.IsInPlaceItemMode)
            {
                // Need to reset the position of this item
                if (isMovingExistingItem)
                {
                    ResetExistingItem();
                }
            }
        }

        _lastFrame = GameManager.Instance.IsInPlaceItemMode;
    }

    private void ChangeMaterial(GameObject go, Material mat)
    {
        int length = go.GetComponent<MeshRenderer>().materials.Length;
        Material[] mats = new Material[length];
        
        for (int i = 0; i < length; i++)
        {
            mats[i] = mat;
        }
        go.GetComponent<MeshRenderer>().materials = mats;
    }

    private void PlaceItem()
    {
        if (GameManager.Instance.IsInPlaceItemMode && !GameManager.Instance.IsCursorOverUI)
        {
            if (_preview == null)
            {
                return;
            }
            
            if (IsOnMesh() & _previewItem.CanSpawn)
            {
                GameObject go = Instantiate(_itemPrefab, _preview.transform.position,
                    new Quaternion(_preview.transform.rotation.x, _preview.transform.rotation.y,
                        _preview.transform.rotation.z, _preview.transform.rotation.w), GameManager.Instance.transform);

                GameManager.Instance.AddToItems(go.GetComponent<Item>());
                
                if (isMovingExistingItem)
                {
                    go.SetActive(true);
                    GameManager.Instance.IsInPlaceItemMode = false;
                    isMovingExistingItem = false;
                    go.GetComponent<Outline>().enabled = true;
                }
            }
        }
    }

    private void ResetExistingItem()
    {
        GameObject go = Instantiate(_itemPrefab, ExistingItemTransform.position,
            ExistingItemTransform.rotation);
        
        go.SetActive(true);
        isMovingExistingItem = false;
    }

    private void SelectedItem(GameObject button)
    {
        GameManager.Instance.IsInPlaceItemMode = true;
    }
    
    private void RotateItem()
    {
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            if (_preview == null)
            {
                return;
            }
            
            _preview.transform.Rotate(_preview.transform.up, 45f);
        }
    }

    /// <summary>
    /// Returns true if the preview object is entirely on top of the mesh
    /// </summary>
    /// <returns></returns>
    private bool IsOnMesh()
    {
        // foreach "point" in an item
        foreach (var corner in _preview.GetComponent<Item>().corners)
        {
            Ray ray = new Ray(corner.position, Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hit, _meshLayer))
            {
                return false;
            }
        }

        return true;
    }
    
    private void OnDrawGizmosSelected() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _meshLayer))
        {
            if (hit.collider.gameObject.tag == "Mesh")
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;   
            }
            Gizmos.DrawRay(hit.point, Camera.main.transform.position);
        }
        
        // foreach "point" in an item
        foreach (var corner in _preview.GetComponent<Item>().corners)
        {
            Gizmos.DrawRay(corner.position, Vector3.down);
        }
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
