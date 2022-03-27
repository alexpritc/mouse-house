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
    
    private GameObject _enclosure;

    public GameObject Enclosure
    {
        get => _enclosure;
        set => _enclosure = value;
    }

    private bool _isInPlaceItemMode = false;
    public bool IsInPlaceItemMode{
        get => _isInPlaceItemMode;
        set => _isInPlaceItemMode = value;
    }
    
    
    private bool _isCursorOverUI = false;
    public bool IsCursorOverUI{
        get => _isCursorOverUI;
        set => _isCursorOverUI = value;
    }

    public Texture2D cursorNormal;
    public Texture2D cursorInteract;
    public CursorMode cursorMode = CursorMode.Auto;

    public void CursorEnterUI()
    {
        _isCursorOverUI = true;
        Cursor.SetCursor(cursorInteract, Vector2.zero, cursorMode);
    }

    public void CursorExitUI()
    {
        _isCursorOverUI = false;
        Cursor.SetCursor(cursorNormal, Vector2.zero, cursorMode);
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

    private GameObject _bedding;

    private Controls _controls;

    private float _beddingMultiplier;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If Instance is ever not its first 'this',
            //  destroy it.
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _controls = new Controls();
        }
    }

    public void SpawnEnclosure(GameObject prefab)
    {
        SceneManager.LoadScene("DecorateEnclosure");
        Enclosure = Instantiate(prefab, transform);
        
        _bedding = Enclosure.GetComponent<Enclosure>().Bedding;
        _beddingMultiplier = Enclosure.GetComponent<Enclosure>().Bedding.transform.localScale.y;
        
        GetComponent<LookIntoEnclosure>().targets = Enclosure.GetComponent<Enclosure>().Targets;
        GetComponent<LookIntoEnclosure>().radius = Enclosure.GetComponent<Enclosure>().Radius;
    }

    public void IncrementStage()
    {
        switch (_gameState)
        {
            case GameState.DecorFloor:
                _gameState = GameState.FillBedding;
                break;
            case GameState.FillBedding:
                _gameState = GameState.DecorBedding;
                break;
            case GameState.DecorBedding:
                _gameState = GameState.DecorRoof;
                break;
            case GameState.DecorRoof:
                _gameState = GameState.Rating;
                break;
            case GameState.Rating:
                _gameState = GameState.Feedback;
                break;
            case GameState.Feedback:
                _gameState = GameState.DecorFloor;
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
                _bedding.transform.localScale = new Vector3(_bedding.transform.localScale.x, _beddingInches * _beddingMultiplier,
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