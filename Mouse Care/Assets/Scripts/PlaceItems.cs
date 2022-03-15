using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private LayerMask _allLayersExceptWalls;
    
    private float _distanceBetweenItems = 1f;
    private bool _canSpawn;
    private bool _canAfford;

    private RaycastHit hitLastTimeWasOnMesh;
    
    public void ResetPreview()
    {
        if (_preview != null)
        {
            Destroy(_preview);
        }
        _preview = Instantiate(_itemPrefab);  
        _preview.gameObject.name = "Preview";
        _preview.GetComponent<Collider>().enabled = false;
        _preview.GetComponent<MeshRenderer>().material = _previewMat;
        _preview.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        _preview.GetComponent<MeshRenderer>().receiveShadows = false;
        Destroy(_preview.GetComponent<NavMeshObstacle>());
        Destroy(_preview.GetComponentInChildren<NavMeshObstacle>());
        
        if (!GameManager.Instance.IsInPlaceItemMode)
        {
            _preview.SetActive(false);
        }
    }

    private void Awake() {
        _controls = new Controls();

        _controls.Item.PlaceItem.performed += ctx => PlaceItem();
        _controls.Item.RotateItem.performed += ctx => RotateItem();
        _preview = null;
    }

    private void Update()
    {
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            if (_itemPrefab.GetComponent<Item>().Price <= GameManager.Instance.MeritPoints)
            {
                _canAfford = true;
            }
            else
            {
                _canAfford = false;
            }
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _meshLayer))
            {
                if (!_preview.activeSelf)
                {
                    _preview.SetActive(true);
                }

                if (hit.collider.gameObject.tag == "Mesh")
                {
                    if (IsOverlapping(_preview.transform.position) || !IsOnMesh())
                    {
                        _canSpawn = false;
                    }
                    else
                    {
                        _canSpawn = true;
                    }
                    
                }
                else
                {
                    _canSpawn = false;
                }
                
                _preview.transform.position =
                    hit.point;

                if (_canSpawn && _canAfford)
                {
                    ChangeMaterial(_preview, _previewMat);
                }
                else
                {
                    ChangeMaterial(_preview, _previewMatRed);
                }
            }
            else
            {
                _canSpawn = false;
                _preview.SetActive(false);
            }
        }
        else
        {
            _canSpawn = false;
            if (_preview != null)
            {
                _preview.SetActive(false);   
            }
        }
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
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            if (_canSpawn && _canAfford)
            {
                GameObject go = Instantiate(_itemPrefab, _preview.transform.position,
                    new Quaternion(_preview.transform.rotation.x, _preview.transform.rotation.y,
                        _preview.transform.rotation.z, _preview.transform.rotation.w));
                float target = GameManager.Instance.MeritPoints -= _itemPrefab.GetComponent<Item>().Price;
                StartCoroutine(GameManager.Instance.ModifyPoints(GameManager.Instance.MeritPoints, target, Time.time, true, 0.1f));
            }
        }
    }
    
    private void SelectedItem(GameObject button)
    {
        GameManager.Instance.IsInPlaceItemMode = true;
    }
    
    private void RotateItem()
    {
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            _preview.transform.Rotate(_preview.transform.up, 90f);
        }
    }

    private bool IsOverlapping(Vector3 point)
    {

        // foreach "point" in an item
        if (_preview.GetComponent<Item>().widths.Length > 1)
        {
            foreach (var width in _preview.GetComponent<Item>().widths)
            {
                Collider[] hitColliders = Physics.OverlapSphere(width.position, _distanceBetweenItems);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.tag != "Mesh")
                    {
                        return true;   
                    }
                }   
            }
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(point, _distanceBetweenItems);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag != "Mesh")
                {
                    return true;   
                }
            }   
        }

        return false;
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

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
