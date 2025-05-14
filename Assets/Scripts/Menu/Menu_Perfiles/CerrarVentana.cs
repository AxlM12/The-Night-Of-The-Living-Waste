using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerrarVentana : MonoBehaviour
{
    [SerializeField] GameObject Ventana;
    [SerializeField] GameObject Crear;
    [SerializeField] GameObject Seleccionar;
    [SerializeField] GameObject Salir;

    public void Cerrar()
    {

        Ventana.SetActive(false);
        Crear.SetActive(true);
        Seleccionar.SetActive(true);
        Salir.SetActive(true);
    }
}
