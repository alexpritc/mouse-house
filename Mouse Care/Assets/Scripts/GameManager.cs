using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager s_instance;

    public static GameManager Instance {
        get => s_instance;
        set => s_instance = value;
    }

    private static int _gameClock;

    public static int GameClock {
        get => _gameClock;
        set => _gameClock = value;
    }
    
    private bool _isInPlaceItemMode = true;
    public bool IsInPlaceItemMode{
        get => _isInPlaceItemMode;
        set => _isInPlaceItemMode = value;
    }

    private Controls _controls;
    
    void Awake() {

        if (s_instance != null) {
            Destroy(s_instance.gameObject);
        }

        s_instance = this;
        _controls = new Controls();
        _controls.GameManager.ToggleItemPlaceMode.performed += ctx => IsInPlaceItemMode = !_isInPlaceItemMode;
    }

    public event Action onTick;

    public void Tick() {
        if (onTick != null) {
            onTick();
        }
    }
    
    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}