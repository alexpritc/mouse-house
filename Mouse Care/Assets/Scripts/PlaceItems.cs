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

    [SerializeField] private bool _isItemSelected;


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
        
        if (!_isItemSelected)
        {
            _preview.SetActive(false);
        }
    }

    private void Awake() {
        _controls = new Controls();

        _controls.Item.PlaceItem.performed += ctx => PlaceItem();
        _preview = null;
    }

    private void Update()
    {
        if (_isItemSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            { 
                if (!_preview.activeSelf)
                {
                    _preview.SetActive(true);

                    _preview.transform.rotation = new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, 
                        _cubePrefab.transform.rotation.w);
                }
                
                if (hit.collider.gameObject.tag == "Mesh")
                {
                    if (IsOverlapping(_preview.transform.position, _preview.transform.localScale.x,
                            _preview.transform.localScale.y, _preview.transform.localScale.z))
                    {
                        _canSpawn = false;
                        _preview.GetComponent<MeshRenderer>().material = _previewMatRed;

                        Ray ray2 = new Ray(hit.point, hit.point - Camera.main.transform.position);
                    
                        if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity, _meshLayer.value))
                        {
                            Quaternion rot = new Quaternion(hit2.normal.x, hit2.normal.y, hit2.normal.z,
                                _cubePrefab.transform.rotation.w);
                            
                            StartCoroutine(UpdateRotation(_preview, _preview.transform.rotation, rot));
                        }
                    }
                    else
                    {
                        _canSpawn = true;
                        
                        _preview.GetComponent<MeshRenderer>().material = _previewMat;
                    
                        Quaternion rot = new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z,
                            _cubePrefab.transform.rotation.w);
                        
                        StartCoroutine(UpdateRotation(_preview, _preview.transform.rotation, rot));
                    }
                }
                else
                {
                    _canSpawn = false;
                    _preview.GetComponent<MeshRenderer>().material = _previewMatRed;

                    Ray ray2 = new Ray(hit.point, hit.point - Camera.main.transform.position);
                    
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity, _meshLayer.value))
                    {
                        Quaternion rot = new Quaternion(hit2.normal.x, hit2.normal.y, hit2.normal.z,
                            _cubePrefab.transform.rotation.w);
                        
                        StartCoroutine(UpdateRotation(_preview, _preview.transform.rotation, rot));
                    }
                }
                
                _preview.transform.position =
                    hit.point + new Vector3(0f, _preview.transform.localScale.y / 2, 0f);

            }
            else
            {
                _canSpawn = false;
                StopAllCoroutines();
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "Mesh" && _canSpawn)
            {
                GameObject go = Instantiate(_cubePrefab, hit.point + new Vector3(0f,_cubePrefab.transform.localScale.y / 2,0f),
                    new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, _cubePrefab.transform.rotation.w));
                
                _preview.transform.rotation = new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, 
                    _cubePrefab.transform.rotation.w);
                _canSpawn = false;
            }
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
