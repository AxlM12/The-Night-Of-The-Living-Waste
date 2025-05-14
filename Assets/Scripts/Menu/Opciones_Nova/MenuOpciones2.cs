using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MenuOpciones2 : MonoBehaviour
{

    public UIBlock Root = null;
    public GameObject Body = null;
    public GameObject Otro = null;

    public List<SettingsCollection> SettingsCollections = null;
    public ListView TabBar = null;

    [Header("Temporary")]
    public BoolSettings BoolSettings = new BoolSettings();
    public ItemView ToggleItemView = null;
    public FloatSetting FloatSetting = new FloatSetting();
    public ItemView SliderItemView = null;

    private int selectedIndex = -1;

    private void Start()
    {
        //Genera el metodo de cambio de color de los botones
        Root.AddGestureHandler<Gesture.OnHover, CambioVisualOpciones>(CambioVisualOpciones.HandleHover);
        Root.AddGestureHandler<Gesture.OnUnhover, CambioVisualOpciones>(CambioVisualOpciones.HandleUnHover);
        Root.AddGestureHandler<Gesture.OnPress, CambioVisualOpciones>(CambioVisualOpciones.HandlePress);
        Root.AddGestureHandler<Gesture.OnRelease, CambioVisualOpciones>(CambioVisualOpciones.HandleRelease);

        Root.AddGestureHandler<Gesture.OnClick, CambioVisualOpciones>(HandleToggleClicked);
        Root.AddGestureHandler<Gesture.OnDrag, VisualSlider>(HandleSliderDragged);

        BindToggle(BoolSettings, ToggleItemView.Visuals as CambioVisualOpciones);
        BindSlider(FloatSetting, SliderItemView.Visuals as VisualSlider);

        //Tab
        TabBar.AddDataBinder<SettingsCollection, VisualTab>(BindTab);
        TabBar.AddGestureHandler<Gesture.OnHover, VisualTab>(VisualTab.HandleHover);
        TabBar.AddGestureHandler<Gesture.OnPress, VisualTab>(VisualTab.HandlePress);
        TabBar.AddGestureHandler<Gesture.OnRelease, VisualTab>(VisualTab.HandleRelease);
        TabBar.AddGestureHandler<Gesture.OnUnhover, VisualTab>(VisualTab.HandleUnhover);
        TabBar.AddGestureHandler<Gesture.OnClick, VisualTab>(HandleTabClicked);

        TabBar.SetDataSource(SettingsCollections);

        if(TabBar.TryGetItemView(0, out ItemView firstTab))
        {

            SelectTab(firstTab.Visuals as VisualTab, 0);
        }
    }

    private void HandleTabClicked(Gesture.OnClick evt, VisualTab target, int index)
    {
        SelectTab(target, index);
    }
    private void SelectTab(VisualTab visualTabb, int index)
    {
        if(index == selectedIndex)
        {
            return;
        }
        if(selectedIndex >= 0 && TabBar.TryGetItemView(selectedIndex, out ItemView currenteItemView))
        {
            (currenteItemView.Visuals as VisualTab).IsSelected = false;
            

        }
        if (visualTabb.Label.Text == "Sonido")
        {
            Body.SetActive(true);
            Otro.SetActive(false);
        }
        else
        {
            Body.SetActive(false);
            Otro.SetActive(true);
        }
        selectedIndex = index;
        visualTabb.IsSelected = true;

    }

    private void BindTab(Data.OnBind<SettingsCollection> evt, VisualTab target, int index)
    {
        target.Label.Text = evt.UserData.Categoria;
        target.IsSelected = false;
    }

    private void HandleSliderDragged(Gesture.OnDrag evt, VisualSlider target)
    {
        Vector3 currentPointerPos = evt.PointerPositions.Current;

        float localXpos = target.SliderBackGround.transform.InverseTransformPoint(currentPointerPos).x;
        float sliderWidth = target.SliderBackGround.CalculatedSize.X.Value;

        float distanceFromLeft = localXpos + .5f * sliderWidth;
        float percenteFromLeft = Mathf.Clamp01(distanceFromLeft / sliderWidth);

        FloatSetting.Value = FloatSetting.min + percenteFromLeft * (FloatSetting.max - FloatSetting.min);

        target.FillBar.Size.X.Percent = percenteFromLeft;
        target.ValueLable.Text = FloatSetting.DisplayValue;
    }

    private void HandleToggleClicked(Gesture.OnClick evt, CambioVisualOpciones target)
    {
        BoolSettings.State = !BoolSettings.State;
        target.IsChecked = BoolSettings.State;
    }

    private void BindSlider(FloatSetting floatSetting, VisualSlider slider)
    {
        slider.ValueLable.Text = floatSetting.DisplayValue;
        slider.FillBar.Size.X.Percent = (floatSetting.Value - floatSetting.min) / (floatSetting.max - floatSetting.min);
    }

    private void BindToggle(BoolSettings boolSettings, CambioVisualOpciones Visualess)
    {
        Visualess.IsChecked = boolSettings.State;
    }
}
