using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CambioVisual : ItemVisuals 
{
    public UIBlock2D Checkbox = null;

    public Color Default;
    public Color Hover;
    public Color Presionar;



    public string NombreEscena;
    public void SceneLoader()
    {
        SceneManager.LoadScene(NombreEscena);
    }
    internal static void HandleHover(Gesture.OnHover evt, CambioVisual target)
    {
        target.Checkbox.Color = target.Hover;
    }

    internal static void HandlePress(Gesture.OnPress evt, CambioVisual target)
    {
        target.Checkbox.Color = target.Presionar; 
    }

    internal static void HandleRelease(Gesture.OnRelease evt, CambioVisual target)
    {
        target.Checkbox.Color = target.Hover;
    }

    internal static void HandleUnHover(Gesture.OnUnhover evt, CambioVisual target)
    {
        target.Checkbox.Color = target.Default;
    }
}
