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
    
    private int s_meritPoints = 0;
    
    public int MeritPoints {
        get => s_meritPoints;
        set => s_meritPoints = value;
    }
    
    private int s_mpPerMin = 100;
    
    public int MpPerMin {
        get => s_mpPerMin;
        set => s_mpPerMin = value;
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
    
    void Start() {
        InvokeRepeating("Tick", 1f, 15);
        
        // TODO: Remove "tock" once way to modify MP/min has been added
        InvokeRepeating("Tock", 0f, 5);
        _TextBoxMPMin.text = s_mpPerMin.ToString();
    }
    void Tick()
    {
        float target = s_meritPoints + s_mpPerMin / 4;
        StartCoroutine(ModifyPoints( s_meritPoints, target, Time.time));
    }
    
    void Tock()
    {
        float target = s_mpPerMin + Random.Range(1, 16);
        StartCoroutine(ModifyPoints(s_mpPerMin, target, Time.time, false));
    }
    
    public IEnumerator ModifyPoints(float startValue, float endValue, float startTime, bool isMP = true, float timeToLerp = 1f)
    {
        float percentage = 0f;
        int result = 0;
        
        TextMeshProUGUI textBox = isMP ? _TextBoxMP : _TextBoxMPMin;

        while (percentage < 1f) {
            percentage = (Time.time - startTime) / timeToLerp;
            result = (int)Mathf.Lerp(startValue, endValue, percentage);

            s_meritPoints = isMP ? result : s_meritPoints;
            s_mpPerMin = isMP ? s_mpPerMin : result;

            if (textBox.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "MPJuice")
            {
                textBox.GetComponent<Animator>().Play("MPJuice");   
            }

            textBox.color = (startValue < endValue) ? textColorIncrease : textColorDecrease;
            textBox.text = result.ToString();

            yield return null;
        }
        
        textBox.color = textColorNormal;
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}