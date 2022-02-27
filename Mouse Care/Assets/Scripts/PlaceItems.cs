using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class PlaceItems : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    
    private Controls _controls;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private bool isItemSelected;


    private GameObject preview;
    [SerializeField] private Material previewMat;
    [SerializeField] private Material previewMatRed;

    [SerializeField] private LayerMask meshLayer;

    private float distanceBetweenItems = 2f;
    private bool canSpawn;

    private float rotationSpeed = 3f;

    private void Start()
    {
        preview = Instantiate(cubePrefab);
        preview.gameObject.name = "Preview";
        preview.GetComponent<Collider>().enabled = false;
        preview.GetComponent<MeshRenderer>().material = previewMat;
        preview.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        preview.GetComponent<MeshRenderer>().receiveShadows = false;
        Destroy(preview.GetComponent<NavMeshObstacle>());
        
        if (!isItemSelected)
        {
            preview.SetActive(false);
        }
    }

    private void Awake() {
        _controls = new Controls();

        _controls.Item.PlaceItem.performed += ctx => PlaceItem();
        preview = null;
    }

    private void Update()
    {
        if (isItemSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            { 
                if (!preview.activeSelf)
                {
                    preview.SetActive(true);

                    preview.transform.rotation = new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, 
                        cubePrefab.transform.rotation.w);
                }
                
                if (hit.collider.gameObject.tag == "Mesh")
                {
                    if (IsOverlapping(preview.transform.position, preview.transform.localScale.x,
                            preview.transform.localScale.y, preview.transform.localScale.z))
                    {
                        canSpawn = false;
                        preview.GetComponent<MeshRenderer>().material = previewMatRed;

                        Ray ray2 = new Ray(hit.point, hit.point - Camera.main.transform.position);
                    
                        if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity, meshLayer.value))
                        {
                            Quaternion rot = new Quaternion(hit2.normal.x, hit2.normal.y, hit2.normal.z,
                                cubePrefab.transform.rotation.w);
                            
                            StartCoroutine(UpdateRotation(preview, preview.transform.rotation, rot));
                        }
                    }
                    else
                    {
                        canSpawn = true;
                        
                        preview.GetComponent<MeshRenderer>().material = previewMat;
                    
                        Quaternion rot = new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z,
                            cubePrefab.transform.rotation.w);
                        
                        StartCoroutine(UpdateRotation(preview, preview.transform.rotation, rot));
                    }
                }
                else
                {
                    canSpawn = false;
                    preview.GetComponent<MeshRenderer>().material = previewMatRed;

                    Ray ray2 = new Ray(hit.point, hit.point - Camera.main.transform.position);
                    
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity, meshLayer.value))
                    {
                        Quaternion rot = new Quaternion(hit2.normal.x, hit2.normal.y, hit2.normal.z,
                            cubePrefab.transform.rotation.w);
                        
                        StartCoroutine(UpdateRotation(preview, preview.transform.rotation, rot));
                    }
                }
                
                preview.transform.position =
                    hit.point + new Vector3(0f, preview.transform.localScale.y / 2, 0f);

            }
            else
            {
                canSpawn = false;
                StopAllCoroutines();
                preview.SetActive(false);
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
            if (hit.collider.gameObject.tag == "Mesh" && canSpawn)
            {
                GameObject go = Instantiate(cubePrefab, hit.point + new Vector3(0f,cubePrefab.transform.localScale.y / 2,0f),
                    new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, cubePrefab.transform.rotation.w));
                
                preview.transform.rotation = new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, 
                    cubePrefab.transform.rotation.w);
                canSpawn = false;
            }
        }
    }

    IEnumerator UpdateRotation(GameObject go, Quaternion from, Quaternion to)
    {
        go.transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime * rotationSpeed);
        yield return null;
    }
    
    private bool IsOverlapping(Vector3 point, float width, float height, float depth)
    {

        // foreach "point" in an item
        Collider[] hitColliders = Physics.OverlapSphere(point, 0.6f);
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
