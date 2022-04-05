using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraverseScenesViaGameManager : MonoBehaviour
{
    public void RateEnclosure()
    {
        GameManager.Instance.CalculateFinalScore();
    }

    public void DecorateEnclosure()
    {
        GameManager.Instance.BackToDecorating();
    }

    public void PickEnclosure()
    {
        GameManager.Instance.PickEnclosure();
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void Settings()
    {
        GameManager.Instance.Settings();
    }
    
    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }
}
