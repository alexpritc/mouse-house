using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Credit: Code Monkey on YouTube.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler _instance;
    public static ScreenshotHandler Instance
    {
        get => _instance;
    }
    
    [SerializeField] private GameObject _canvas;
    [SerializeField] private DisplayRating dr;
    
    private string _path = "";

    AudioSource _audioSource
    {
        get { return GetComponent<AudioSource>(); }
    }
    
    void Awake()
    {
        _instance = this;
    }

    public void TakeScreenshot()
    {
        dr.toggle = false;
        StartCoroutine(SaveScreenshot());
    }

    IEnumerator SaveScreenshot()
    {
        yield return new WaitUntil(() => dr.Camera.rect.x <= 0.01f);
        _canvas.SetActive(false);
        _audioSource.Play();
        _path = Application.persistentDataPath + "/MouseHouse-" +
                System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(_path);
        Debug.Log("Saved screenshot at " + _path);
        StartCoroutine(TurnOnCanvas(_path));
    }
    
    IEnumerator TurnOnCanvas(string path)
    {
        yield return new WaitUntil(() => File.Exists(path));
        _canvas.SetActive(true);
    }
}
