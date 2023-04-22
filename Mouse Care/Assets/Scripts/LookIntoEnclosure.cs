using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIntoEnclosure : MonoBehaviour
{
    public List<Transform> targets;
    public float radius;
    private List<Transform> obstructions;

    public bool isEnabled;
    
    void Start()
    {
        obstructions = new List<Transform>();
        targets = new List<Transform>();
    }


    public void ClearObstructions()
    {
        obstructions.Clear();
    }
    
    // Physics-based
    private void LateUpdate()
    {
        if (isEnabled)
        {
            if (targets != null)
            {
                foreach (var target in targets)
                {
                    ViewObstructedSphere(target);   
                }   
            }
        }
    }
    
    void ViewObstructedSphere(Transform target)
    {
        float characterDistance = Vector3.Distance(Camera.main.transform.position, target.transform.position);
        
        int layerNumber = LayerMask.NameToLayer("Walls");
        int layerMask = 1 << layerNumber;
        
        RaycastHit[] hits = Physics.SphereCastAll(Camera.main.transform.position, radius, target.transform.position - Camera.main.transform.position, characterDistance, layerMask);
        
        // Walls are blocking the camera view
        if (hits.Length > 0)
        {
            if (obstructions != null && obstructions.Count > 0)
            {
                // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
                for (int i = 0; i < obstructions.Count; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
                }

                obstructions.Clear();
            }

            // Hide the current obstructions
            for (int i = 0; i < hits.Length; i++)
            {
                Transform obstruction = hits[i].transform;
                obstructions.Add(obstruction);
                obstructions[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
            if (obstructions != null && obstructions.Count > 0)
            {
                for (int i = 0; i < obstructions.Count; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                obstructions.Clear();
            }
        } 
    }

    public void ToggleXray()
    {
        isEnabled = !isEnabled;

        if (isEnabled == false)
        {
            if (obstructions != null && obstructions.Count > 0)
            {
                // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
                for (int i = 0; i < obstructions.Count; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
                }

                obstructions.Clear();
            }   
        }
    }
    
    private void OnDrawGizmosSelected() {


        foreach (var target in targets)
        {
            Gizmos.DrawRay(target.position, Camera.main.transform.position);
        }
    }
}
