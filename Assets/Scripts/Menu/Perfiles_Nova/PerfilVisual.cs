using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerfilVisual : ItemVisuals
{
    public UIBlock2D Designe = null;

    public Color Default;
    public Color Hover;
    public Color Presionar;



    public string NombreEscena;
    public void SceneLoader()
    {
        SceneManager.LoadScene(NombreEscena);
    }
    internal static void HandleHover(Gesture.OnHover evt, PerfilVisual target)
    {
        target.Designe.Color = target.Hover;
    }

    internal static void HandlePress(Gesture.OnPress evt, PerfilVisual target)
    {
        target.Designe.Color = target.Presionar;
    }

    internal static void HandleRelease(Gesture.OnRelease evt, PerfilVisual target)
    {
        target.Designe.Color = target.Hover;
    }

    internal static void HandleUnHover(Gesture.OnUnhover evt, PerfilVisual target)
    {
        target.Designe.Color = target.Default;
    }
}
