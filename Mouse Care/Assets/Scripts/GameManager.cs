using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    private static GameManager s_instance;

    public static GameManager Instance {
        get => s_instance;
        set => s_instance = value;
    }

    private static int s_meritPoints = 10;
    
    public static int MeritPoints {
        get => s_meritPoints;
        set => s_meritPoints = value;
    }
    
    private static int s_mpPerMin = 100;
    
    public static int MpPerMin {
        get => s_mpPerMin;
        set => s_mpPerMin = value;
    }

    private bool _isInPlaceItemMode = false;
    public bool IsInPlaceItemMode{
        get => _isInPlaceItemMode;
        set => _isInPlaceItemMode = value;
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
        StartCoroutine(IncreasePoints(_TextBoxMP, s_meritPoints, target, Time.time));
    }
    
    void Tock()
    {
        float target = s_mpPerMin + Random.Range(1, 16);
        StartCoroutine(IncreasePoints(_TextBoxMPMin, s_mpPerMin, target, Time.time, false));
    }
    
    public IEnumerator IncreasePoints(TextMeshProUGUI textBox, float startValue, float endValue, float startTime, bool isMP = true, float timeToLerp = 1f)
    {
        float percentage = 0f;
        int result = 0;
        
        while (percentage < 1f) {
            percentage = (Time.time - startTime) / timeToLerp;
            result = (int)Mathf.Lerp(startValue, endValue, percentage);

            s_meritPoints = isMP ? result : s_meritPoints;
            s_mpPerMin = isMP ? s_mpPerMin : result;

            if (textBox.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "MPJuice")
            {
                textBox.GetComponent<Animator>().Play("MPJuice");   
            }
            
            textBox.color = textColorIncrease;
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