using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (preview == null)
                {
                    preview = Instantiate(cubePrefab);
                    preview.GetComponent<Collider>().enabled = false;
                    preview.GetComponent<MeshRenderer>().material = previewMat;
                }
                
                if (hit.collider.gameObject.tag == "Mesh")
                {
                    preview.GetComponent<MeshRenderer>().material = previewMat;
                    
                    preview.transform.rotation = new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z,
                        cubePrefab.transform.rotation.w);
                }
                else
                {
                    preview.GetComponent<MeshRenderer>().material = previewMatRed;

                    Ray ray2 = new Ray(hit.point, hit.point - Camera.main.transform.position);
                    
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity, meshLayer.value))
                    {
                        preview.transform.rotation = new Quaternion(hit2.normal.x, hit2.normal.y, hit2.normal.z,
                            cubePrefab.transform.rotation.w);
                    }
                }
                
                preview.transform.position =
                    hit.point + new Vector3(0f, preview.transform.localScale.y / 2, 0f);

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
            if (hit.collider.gameObject.tag == "Mesh")
            {
                GameObject go = Instantiate(cubePrefab, hit.point + new Vector3(0f,cubePrefab.transform.localScale.y / 2,0f),
                    new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, cubePrefab.transform.rotation.w));
            }
            
            Destroy(preview);
            preview = null;
        }
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
