using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEnclosurePanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _internalDimensionsText;
    [SerializeField] private TextMeshProUGUI _floorspaceText;
    [SerializeField] private TextMeshProUGUI _miceText;
    [HideInInspector] public Enclosure Enclosure;

    public void SetInitialValues(Enclosure e)
    {
        Enclosure = e;
        _nameText.text = Enclosure.Name;
        _descriptionText.text = Enclosure.Description;
        _floorspaceText.text = Enclosure.Floorspace;
        _internalDimensionsText.text = Enclosure.Dimensions;
        _miceText.text = Enclosure.Mice;
    }
}
