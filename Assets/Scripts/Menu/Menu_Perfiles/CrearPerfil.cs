using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CrearPerfil : MonoBehaviour
{
    [SerializeField] public TMP_InputField txtUsario;
    public TMP_Text nombre;
    //public string nombreUsuario;
    public void InstantiateCaller(Profile prefab)
    {
        GameObject Content = GameObject.Find("Content");
        //GameObject Input = GameObject.Find("InputField");

        SaveData data = SaveSystem.LoadData(txtUsario.text);
        if(data == null)
        {
            Profile newProfile = Instantiate(prefab, Content.transform);
            newProfile.Init(txtUsario.text);
        }
        txtUsario.text = null;
    }

    //public void Nombre(GameObject prefab)
    //{
    //    nombreUsuario = txtUsario.text;
    //    nombre.text = nombreUsuario;
    //}
}
