using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpciones : MonoBehaviour
{
    public UIBlock Root = null;

    private void Start()
    {
        //Botones
        Root.AddGestureHandler<Gesture.OnHover, CambioVisual>(CambioVisual.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, CambioVisual>(CambioVisual.HandleUnHover);
        Root.AddGestureHandler<Gesture.OnPress, CambioVisual>(CambioVisual.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, CambioVisual>(CambioVisual.HandleRelease);

        Root.AddGestureHandler<Gesture.OnClick, CambioVisual>(HandleToggleClicked);

    }

    private void HandleToggleClicked(Gesture.OnClick evt, CambioVisual target)
    {
        target.SceneLoader();
    }

}
