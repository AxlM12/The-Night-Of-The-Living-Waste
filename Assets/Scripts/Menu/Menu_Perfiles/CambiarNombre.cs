using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CambiarNombre : MonoBehaviour
{
    public InputField Input;
    public Text texto;
    public void ModificarNombre()
    {
        texto.text = Input.text;
    }
}
