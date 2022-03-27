using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeddingController : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Click);   
    }

    private void Click()
    {
        GameManager.Instance.FillBedding();
    }
}
