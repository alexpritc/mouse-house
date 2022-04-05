using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[RequireComponent(typeof(Slider))]
public class AudioManager : MonoBehaviour
{
    Slider slider
    {
        get { return GetComponent<Slider>(); }
    }

    public AudioMixer mixer;
    public string volName;

    public void UpdateValueOnChange(float value)
    {
        mixer.SetFloat(volName, Mathf.Log(value) * 20f);
    }
}