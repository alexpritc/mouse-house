using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PlaceItems : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    
    private Controls _controls;
    
    private GameObject _preview;
    [SerializeField] private Material _previewMat;
    [SerializeField] private Material _previewMatRed;

    [SerializeField] private LayerMask _meshLayer;

    private float _distanceBetweenItems = 1f;
    private bool _canSpawn;
    private float _rotationSpeed = 3f;

    private void Start()
    {
        _preview = Instantiate(_cubePrefab);
        _preview.gameObject.name = "Preview";
        _preview.GetComponent<Collider>().enabled = false;
        _preview.GetComponent<MeshRenderer>().material = _previewMat;
        _preview.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        _preview.GetComponent<MeshRenderer>().receiveShadows = false;
        Destroy(_preview.GetComponent<NavMeshObstacle>());
        
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            { 
                if (!_preview.activeSelf)
                {
                    _preview.SetActive(true);
                }
                
                if (hit.collider.gameObject.tag == "Mesh")
                {
                    if (IsOverlapping(_preview.transform.position, _preview.transform.localScale.x,
                            _preview.transform.localScale.y, _preview.transform.localScale.z))
                    {
                        _canSpawn = false;
                        _preview.GetComponent<MeshRenderer>().material = _previewMatRed;
                    }
                    else
                    {
                        _canSpawn = true;
                        _preview.GetComponent<MeshRenderer>().material = _previewMat;
                    }
                }
                else
                {
                    _canSpawn = false;
                    _preview.GetComponent<MeshRenderer>().material = _previewMatRed;
                }
                
                _preview.transform.position =
                    hit.point + new Vector3(0f, _preview.transform.localScale.y / 2, 0f);
            }
            else
            {
                _canSpawn = false;
                _preview.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
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
    }
    
    private void PlaceItem()
    {
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.tag == "Mesh" && _canSpawn)
                {
                    GameObject go = Instantiate(_cubePrefab, hit.point,
                        new Quaternion(_preview.transform.rotation.x, _preview.transform.rotation.y, _preview.transform.rotation.z, _preview.transform.rotation.w));
                    
                    _canSpawn = false;
                }
            }   
        }
    }

    private void RotateItem()
    {
        if (GameManager.Instance.IsInPlaceItemMode)
        {
            _preview.transform.Rotate(_preview.transform.up, 90f);
        }
    }

    IEnumerator UpdateRotation(GameObject go, Quaternion from, Quaternion to)
    {
        go.transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime * _rotationSpeed);
        yield return null;
    }
    
    private bool IsOverlapping(Vector3 point, float width, float height, float depth)
    {

        // foreach "point" in an item
        Collider[] hitColliders = Physics.OverlapSphere(point, _distanceBetweenItems);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag != "Mesh")
            {
                return true;   
            }
        }

        return false;
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
