using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpawner : MonoBehaviour {
    
    [SerializeField] private int _howManyMiceToSpawn = 10;

    [SerializeField] private GameObject _maleMousePrefab;
    [SerializeField] private GameObject _femaleMousePrefab;
    
    [SerializeField] private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _howManyMiceToSpawn; i++) {

            if (Random.value > 0.5f) {
                // Spawn female
                InstantiateMouse(true);
            }
            else {
                // spawn male
                InstantiateMouse(false);
            }
        }
    }

    void InstantiateMouse(bool isFemale) {

        GameObject go;
        if (isFemale) {
            go = Instantiate(_femaleMousePrefab, Vector3.zero, Quaternion.identity);
        }
        else {
            go = Instantiate(_maleMousePrefab, Vector3.zero, Quaternion.identity);
        }

        go.GetComponent<Mouse>().target = _target;

    }
}
