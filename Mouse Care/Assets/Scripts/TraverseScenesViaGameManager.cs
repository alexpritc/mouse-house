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
}
