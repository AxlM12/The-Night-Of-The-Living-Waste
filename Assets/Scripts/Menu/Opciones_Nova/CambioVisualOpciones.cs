using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CambioVisualOpciones : ItemVisuals
{
    public UIBlock2D Checkbox = null;
    public UIBlock2D CheckMark = null;

    public Color Default;
    public Color Hover;
    public Color Presionar;

    public bool IsChecked
    {
        get => CheckMark.gameObject.activeSelf;
        set => CheckMark.gameObject.SetActive(value);
    }
    internal static void HandleHover(Gesture.OnHover evt, CambioVisualOpciones target)
    {
        target.Checkbox.Color = target.Hover;
    }

    internal static void HandlePress(Gesture.OnPress evt, CambioVisualOpciones target)
    {
        target.Checkbox.Color = target.Presionar;
    }

    internal static void HandleRelease(Gesture.OnRelease evt, CambioVisualOpciones target)
    {
        target.Checkbox.Color = target.Hover;
    }

    internal static void HandleUnHover(Gesture.OnUnhover evt, CambioVisualOpciones target)
    {
        target.Checkbox.Color = target.Default;
    }
}
