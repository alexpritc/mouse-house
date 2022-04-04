using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    private static GameManager s_instance;

    public static GameManager Instance {
        get => s_instance;
        set => s_instance = value;
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
    
    private bool _isInPlaceItemMode = false;
    public bool IsInPlaceItemMode{
        get => _isInPlaceItemMode;
        set => _isInPlaceItemMode = value;
    }

    private Controls _controls;

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
            _items = new List<Item>();
        }
    }
    
    // Manage chosen enclosure and bedding 
    
    private GameObject _prefab;
    
    public void SpawnEnclosure(GameObject prefab)
    {
        SceneManager.LoadScene("DecorateEnclosure");

        _prefab = prefab;
        Invoke("SetEnclosure", 0.25f);
    }

    private GameObject _enclosure;

    public GameObject Enclosure
    {
        get => _enclosure;
        set => _enclosure = value;
    }
    
    private GameObject _bedding;
    
    private float _beddingMultiplier;

    private void SetEnclosure()
    {
        Enclosure = Instantiate(_prefab, transform);
        
        _bedding = Enclosure.GetComponent<Enclosure>().Bedding;
        _beddingMultiplier = Enclosure.GetComponent<Enclosure>().Bedding.transform.localScale.y;
        
        GetComponent<LookIntoEnclosure>().targets = Enclosure.GetComponent<Enclosure>().Targets;
        GetComponent<LookIntoEnclosure>().radius = Enclosure.GetComponent<Enclosure>().Radius;
    }

    private float _beddingInches = 0;
    public float BeddingInches{
        get => _beddingInches;
        set => _beddingInches = value;
    }
    
    public void FillBedding()
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
    
    // Manage keeping track of all items placed in the enclosure

    private List<Item> _items;
    public List<Item> Items
    {
        get => _items;
    }

    public void AddToItems(Item item)
    {
        TypeOfItem(item.ItemType);
        IsWaterOrFood(item.Name);
        
        Stress(item._stress);
        Enrichment(item._enrichment);
        
        IsWheel(item.Name);
        
        Items.Add(item);
    }
    
    public void RemoveFromItems(Item item)
    {
        TypeOfItem(item.ItemType, -1);
        IsWaterOrFood(item.Name, -1);
        
        Stress(item._stress, -1);
        Enrichment(item._enrichment, -1);
        
        IsWheel(item.Name, -1);
        
        Items.Remove(item);
    }

    // How many items from each category
    private int _numberOfDecorationItems;
    private int _numberOfExerciseItems;
    private int _numberOfHouseItems;
    private int _numberOfForageItems;
    
    private int _numberOfWaterSources;
    private int _numberOfFoodSources;
    
    private void TypeOfItem(ItemType type, int modifier = 1)
    {
        switch (type)
        {
            case ItemType.Decoration:
                _numberOfDecorationItems += modifier;
                break;
            case ItemType.Exercise:
                _numberOfExerciseItems += modifier;
                break;
            case ItemType.Forage:
                _numberOfForageItems += modifier;
                break;
            case ItemType.Housing:
                _numberOfHouseItems += modifier;
                break;
        }
    }

    private void IsWaterOrFood(string name, int modifier = 1)
    {
        switch (name)
        {
            case "Food Bowl":
                _numberOfFoodSources += modifier;
                break;
            case "Water Bowl":
                _numberOfWaterSources += modifier;
                break;
        }
    }
    
    // How many wheels
    private int _numberOfWheels;
    private void IsWheel(string name, int modifier = 1)
    {
        if (name.Contains("Wheel"))
        {
            _numberOfWheels += modifier;
        }
    }

    // Overall stressfulness
    private int _stress;
    
    /// <summary>
    /// A lower value is best.
    /// </summary>
    /// <param name="stress"></param>
    /// <param name="modifier"></param>
    private void Stress(string stress, int modifier = 1)
    {
        switch (stress)
        {
            case "Low":
                _stress += modifier * 0;
                break;
            case "Medium":
                _stress += modifier * 10;
                break;
            case "High":
                _stress += modifier * 100;
                break;
        }
    }
    // Overall how enriching it is
    private int _enrichment;
    
    /// <summary>
    /// A higher value is best.
    /// </summary>
    /// <param name="enrichment"></param>
    /// <param name="modifier"></param>
    private void Enrichment(string enrichment, int modifier = 1)
    {
        switch (enrichment)
        {
            case "Low":
                _enrichment += modifier * 0;
                break;
            case "Medium":
                _enrichment += modifier * 10;
                break;
            case "High":
                _enrichment += modifier * 100;
                break;
        }
    }

    // How many overall items (can use _items.Count)
    // How many unique items
    private int _numberOfUniqueItems;
    private void UniqueItems()
    {
        _numberOfUniqueItems = 0;
        
        List<Item> temp = new List<Item>();

        bool isUnique;
        
        foreach (var item in _items)
        {
            isUnique = true;
            
            foreach (var tempItem in temp)
            {
                if (item.Name == tempItem.name)
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                _numberOfUniqueItems++;
            }
            
            temp.Add(item);
        }
    }
    
    private int _score;
    public int Score
    {
        get => _score;
        set => _score = value;
    }
    
    // Which enclosure they picked
    private int _enclosureRating;

    private int _fp;
    private int _height;
    private void EnclosureRating()
    {
        _enclosureRating = 0;
        
        // Floorspace is more important than height
        _fp = GetFloorSpaceFromString(Enclosure.GetComponent<Enclosure>().Floorspace);
        _enclosureRating += _fp * 10;

        _height = GetIntFromString(Enclosure.GetComponent<Enclosure>().Dimensions, 3);
        _enclosureRating += _height * 10;
    }

    int GetFloorSpaceFromString(string input)
    {
        // Split on one or more non-digit characters.
        int i = 0;
        string[] numbers = Regex.Split(input, @"\D+");
        foreach (string value in numbers)
        {
            if (!string.IsNullOrEmpty(value))
            {
                i = int.Parse(value);
            }
        }

        return i;
    }

    int GetIntFromString(string input, int index)
    {
        // Split on one or more non-digit characters.
        List<int> values = new List<int>();
        string[] numbers = Regex.Split(input, @"\D+");
        foreach (string value in numbers)
        {
            if (!string.IsNullOrEmpty(value))
            {
                values.Add(int.Parse(value));
            }
        }

        return values[index];
    }
    
    // TODO: Balance this out once more items have been added.
    public void CalculateFinalScore()
    {
        Score = 0;
        UniqueItems();
        EnclosureRating();
        
        Score += _items.Count * 100;
        Score += _numberOfUniqueItems * 500;
        
        Score += (int)BeddingInches * 5000;
        Score += _enclosureRating;

        Score += _numberOfDecorationItems * 100;
        Score += _numberOfExerciseItems * 150;
        Score += _numberOfHouseItems * 200;
        
        Score += _numberOfForageItems * 500;

        Score += TooMuchOfAGoodThing(_numberOfFoodSources, 3) * 500;
        Score += TooMuchOfAGoodThing(_numberOfWaterSources, 3) * 500;

        Score += TooMuchOfAGoodThing(_numberOfWheels, 2) * 1000;

        // Limit score
        if (Score > 99999)
        {
            Score = 99999;
        }
        
        FillRecommendations();
        SceneManager.LoadScene("RateEnclosure");
    }

    /// <summary>
    /// Limits how many points can be awarded for putting in
    /// too many of the same item.
    ///
    /// The user gets rewarded for multiple items anyway.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="peak"></param>
    /// <returns></returns>
    int TooMuchOfAGoodThing(int value, int peak)
    {
        if (value > peak)
        {
            return peak;
        }

        return value;
    }
    
    // Recommendations and feedback
    private List<string> _recommendations;

    public List<string> Recommendations
    {
        get => _recommendations;
    }

    private void FillRecommendations()
    {
        _recommendations = new List<string>();
        
        if (_items.Count <= 10)
        {
            _recommendations.Add("Your enclosure is too empty. You need to add items for your mice to gnaw on, hide in, and play with.");
        }
        else if (_numberOfUniqueItems <= 10)
        {
            _recommendations.Add("You should add more variety to your enclosure. Mice can get bored of having the same things to play with.");
        }

        // Doesnt apply until smaller enclosures have been picked.
        if (_fp < 500)
        {
            _recommendations.Add("You need to upgrade the size of your enclosure. Mice need at least 600 sq inches of unbroken floorspace.");
        }

        if ((int)_beddingInches * 12 < 8)
        {
            _recommendations.Add("You need to add more substrate to your enclosure so that your mice can make burrows. Ideally, they need at least 8 inches of bedding.");
        }

        if (_numberOfWheels < 2)
        {
            _recommendations.Add("You need at least 2 wheels for a group of mice, so that they do not fight over which mouse gets to use it.");
        }

        if (_numberOfWaterSources == 0)
        {
            _recommendations.Add("Your mice have no water! You can add water from the forage tab. It is recommended to have at least 2 water sources to reduce fighting.");
        }
        else if (_numberOfWaterSources < 2)
        {
            _recommendations.Add("You should add more water sources to your enclosure, otherwise your pets might fight. You can add water from the forage tab.");
        }
        
        if (_numberOfFoodSources == 0)
        {
            _recommendations.Add("Your mice have no food! You can add food from the forage tab. It is recommended to have at least 2 food sources to reduce fighting.");
        }
        else if (_numberOfFoodSources < 2)
        {
            _recommendations.Add("You should add more food sources to your enclosure, otherwise your pets might fight. You can add food from the forage tab.");
        }

        if (_recommendations.Count == 0)
        {
            _recommendations.Add("Well done! This is a great example of a mouse enclosure!");
        }
    }

    public void BackToDecorating()
    {
        SceneManager.LoadScene("DecorateEnclosure");
    }
    
    public void PickEnclosure()
    {
        SceneManager.LoadScene("PickEnclosure");
    }

    // Controls
    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
    }
}