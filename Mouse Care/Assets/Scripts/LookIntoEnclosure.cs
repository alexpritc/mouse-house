using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIntoEnclosure : MonoBehaviour
{
    public Transform[] targets;
    private List<Transform> obstructions;

    void Start()
    {
        obstructions = new List<Transform>();
    }
 
    private void LateUpdate()
    {
        foreach (var target in targets)
        {
            ViewObstructed(target);   
        }
    }

    void ViewObstructed(Transform target)
    {
        float characterDistance = Vector3.Distance(transform.position, target.transform.position);
        
        int layerNumber = LayerMask.NameToLayer("Walls");
        int layerMask = 1 << layerNumber;
        
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 10f, target.transform.position - transform.position, characterDistance, layerMask);
        
        // Walls are blocking the camera view
        if (hits.Length > 0)
        {
            if (obstructions != null && obstructions.Count > 0)
            {
                // Repaint all the previous obstructions. Because some of the stuff might be not blocking anymore
                for (int i = 0; i < obstructions.Count; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }

                obstructions.Clear();
            }
            
            // Hide the current obstructions
            for (int i = 0; i < hits.Length; i++)
            {
                Transform obstruction = hits[i].transform;
                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                obstructions.Add(obstruction);
            }
        }
        else
        {
            // Mean that no more stuff is blocking the view and sometimes all the stuff is not blocking as the same time
            if (obstructions != null && obstructions.Count > 0)
            {
                for (int i = 0; i < obstructions.Count; i++)
                {
                    obstructions[i].gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                obstructions.Clear();
            }
        }
    }
}
