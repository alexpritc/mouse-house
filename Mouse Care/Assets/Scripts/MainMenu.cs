using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPrefab;
    [SerializeField] private GameObject _settingsMenuPrefab;

    public bool isMenuOpen;
    public bool isSettingsOpen;
    
    // Start is called before the first frame update
    void Awake()
    {
        CloseMainMenu();
        CloseSettings();
    }
    
    public void OpenSettings()
    {
        isSettingsOpen = true;
        CloseMainMenu();
        _settingsMenuPrefab.SetActive(true);
    }

    public void OpenMainMenu()
    {
        isMenuOpen = true;
        CloseSettings();
        _mainMenuPrefab.SetActive(true);
    }

    public void CloseSettings()
    {
        isSettingsOpen = false;
        _settingsMenuPrefab.SetActive(false);
    }

    public void CloseMainMenu()
    {
        isMenuOpen = false;
        _mainMenuPrefab.SetActive(false);
    }

    public void StartFromScratch()
    {
        CloseMainMenu();
        CloseSettings();
    }
}
