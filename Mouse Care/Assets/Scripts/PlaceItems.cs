using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItems : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    
    private Controls _controls;

    [SerializeField] private Camera mainCamera;
    private void Awake() {
        _controls = new Controls();
        
        _controls.Item.PlaceItem.performed += ctx => PlaceItem();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        { 
            if (hit.collider.gameObject.tag == "Mesh")
            {
                Debug.Log(true);
            }
            else
            {
                Debug.Log(false);
            }
        }
    }

    private void OnDrawGizmos() {

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
                GameObject go = Instantiate(cubePrefab, hit.point + new Vector3(0f,0.5f,0f),
                    new Quaternion(hit.normal.x, hit.normal.y, hit.normal.z, cubePrefab.transform.rotation.w));
            }
        }
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}
