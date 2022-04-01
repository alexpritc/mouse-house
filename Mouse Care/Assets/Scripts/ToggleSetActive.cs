using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSetActive : MonoBehaviour
{
    public void ToggleGameObject(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }
}
