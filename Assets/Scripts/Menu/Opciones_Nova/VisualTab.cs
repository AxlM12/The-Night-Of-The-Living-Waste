using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTab : ItemVisuals
{
    public TextBlock Label = null;
    public UIBlock2D Background = null;
    public UIBlock2D SelectorIndicador = null;

    public Color DefaultColor;
    public Color SelectedColor;

    public Color DefaultGradientColor;
    public Color HoveredGradientColor;
    public Color PressedGradientColor;

    public bool IsSelected
    {
        get => SelectorIndicador.gameObject.activeSelf;
        set
        {
            SelectorIndicador.gameObject.SetActive(value);
            Background.Color = value ? SelectedColor : DefaultColor;
        }
    }

    internal static void HandleHover(Gesture.OnHover evt, VisualTab target, int index)
    {
        target.Background.Gradient.Color = target.HoveredGradientColor;
    }

    internal static void HandlePress(Gesture.OnPress evt, VisualTab target, int index)
    {
        target.Background.Gradient.Color = target.PressedGradientColor;
    }

    internal static void HandleRelease(Gesture.OnRelease evt, VisualTab target, int index)
    {
        target.Background.Gradient.Color = target.HoveredGradientColor;
    }

    internal static void HandleUnhover(Gesture.OnUnhover evt, VisualTab target, int index)
    {
        target.Background.Gradient.Color = target.DefaultGradientColor;
    }
}
