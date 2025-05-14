using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public abstract class Settingss
{
    public string Name;
}
public class BoolSettings : Settingss
{
    public bool State;
}

[System.Serializable]
public class FloatSetting : Settingss
{
    [SerializeField]
    private float value;
    public float min;
    public float max;
    public string ValueFormat = "{0:0.0}";

    public float Value
    {
        get => Mathf.Clamp(value, min, max);
        set => this.value = Mathf.Clamp(value, min, max);
    }

    public string DisplayValue => string.Format(ValueFormat, Value);
}
