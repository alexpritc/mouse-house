using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleXrayButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    public void Click()
    {
        GameManager.Instance.GetComponent<LookIntoEnclosure>().ToggleXray();
    }

}
