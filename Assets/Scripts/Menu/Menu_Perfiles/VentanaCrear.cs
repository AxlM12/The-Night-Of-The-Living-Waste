using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentanaCrear : MonoBehaviour
{
    [SerializeField] GameObject Ventana;
    [SerializeField] GameObject Crear;
    [SerializeField] GameObject Seleccionar;
    [SerializeField] GameObject Salir;
    public void AbrirVentana()
    {
        Ventana.SetActive(true);
        Crear.SetActive(false);
        Seleccionar.SetActive(false);
        Salir.SetActive(false);

    }
}
