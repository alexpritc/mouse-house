using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpawner : MonoBehaviour {
    
    [SerializeField] private int _howManyMiceToSpawn = 10;
    
    [SerializeField] private GameObject _mousePrefab;
    
    [SerializeField] private GameObject _target;
    [SerializeField] private Enclosure _enclosure;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _howManyMiceToSpawn; i++) {
            InstantiateMouse();
        }
    }

    void InstantiateMouse() {

        GameObject go = Instantiate(_mousePrefab, Vector3.zero, Quaternion.identity);
        go.GetComponent<Mouse>().target = _target;
        go.GetComponent<Mouse>()._enclosure = _enclosure;
    }
}
