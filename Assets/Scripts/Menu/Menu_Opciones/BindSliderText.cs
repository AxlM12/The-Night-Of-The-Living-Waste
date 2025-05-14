using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BindSliderText : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] private TextMeshProUGUI textComp;
    void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    void UpdateText(float val)
    {
        textComp.text = slider.value.ToString();
    }

}
