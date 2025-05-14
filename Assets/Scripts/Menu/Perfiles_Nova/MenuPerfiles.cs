using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPerfiles : MonoBehaviour
{

    public UIBlock Root = null;

    // Start is called before the first frame update
    void Start()
    {
        Root.AddGestureHandler<Gesture.OnHover, PerfilVisual>(PerfilVisual.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, PerfilVisual>(PerfilVisual.HandleUnHover);
        Root.AddGestureHandler<Gesture.OnPress, PerfilVisual>(PerfilVisual.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, PerfilVisual>(PerfilVisual.HandleRelease);

        Root.AddGestureHandler<Gesture.OnClick, PerfilVisual>(HandleToggleClicked);
    }

    private void HandleToggleClicked(Gesture.OnClick evt, PerfilVisual target)
    {
        target.SceneLoader();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
