using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum GameState {
    DecorFloor,
    FillBedding,
    DecorBedding,
    DecorRoof,
    Rating,
    Feedback
}

public class GameManager : MonoBehaviour {
    private static GameManager s_instance;

    public static GameManager Instance {
        get => s_instance;
        set => s_instance = value;
    }

    private GameState _gameState = GameState.DecorFloor;

    public GameState GameState
    {
        get => _gameState;
        set => _gameState = value;
    }
    
    private GameObject _enclosurePrefab;

    public GameObject EnclosurePrefab
    {
        get => _enclosurePrefab;
        set => _enclosurePrefab = value;
    }

    private bool _isInPlaceItemMode = false;
    public bool IsInPlaceItemMode{
        get => _isInPlaceItemMode;
        set => _isInPlaceItemMode = value;
    }
    
    private bool _isInFollowingMode = false;
    public bool IsInFollowingMode{
        get => _isInFollowingMode;
        set => _isInFollowingMode = value;
    }
    
    private bool _isShopOpen = false;
    public bool IsShopOpen{
        get => _isShopOpen;
        set => _isShopOpen = value;
    }

    private float _beddingInches = 0;
    public float BeddingInches{
        get => _beddingInches;
        set => _beddingInches = value;
    }

    [SerializeField] private GameObject _bedding;

    [SerializeField] private TextMeshProUGUI _status;

    private Controls _controls;

    [SerializeField] private TextMeshProUGUI _TextBoxMP;
    [SerializeField] private TextMeshProUGUI _TextBoxMPMin;

    [SerializeField] private Color textColorNormal;
    [SerializeField] private Color textColorIncrease;
    [SerializeField] private Color textColorDecrease;

    void Awake() {

        if (s_instance != null) {
            Destroy(s_instance.gameObject);
        }

        s_instance = this;
        _controls = new Controls();
        _controls.GameManager.ToggleItemPlaceMode.performed += ctx => IsInPlaceItemMode = !_isInPlaceItemMode;
    }
    
    public void IncrementStage()
    {
        switch (_gameState)
        {
            case GameState.DecorFloor:
                _gameState = GameState.FillBedding;
                _status.text = "Fill with bedding";
                break;
            case GameState.FillBedding:
                _gameState = GameState.DecorBedding;
                _status.text = "Decorate your cage";
                break;
            case GameState.DecorBedding:
                _gameState = GameState.DecorRoof;
                _status.text = "Decorate your lid";;
                break;
            case GameState.DecorRoof:
                _gameState = GameState.Rating;
                _status.text = "Rating";
                break;
            case GameState.Rating:
                _gameState = GameState.Feedback;
                _status.text = "Feedback";
                break;
            case GameState.Feedback:
                _gameState = GameState.DecorFloor;
                _status.text = "Decorate your cage";
                break;
        }
    }
    
    public void FillBedding()
    {
        if (_gameState == GameState.FillBedding)
        {
            _beddingInches += 0.2f;
            if (_beddingInches > 1)
            {
                _beddingInches = 0;
                _bedding.SetActive(false);
            }
            else
            {
                _bedding.SetActive(true);
                _bedding.transform.localScale = new Vector3(_bedding.transform.localScale.x, _beddingInches,
                    _bedding.transform.localScale.z);
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