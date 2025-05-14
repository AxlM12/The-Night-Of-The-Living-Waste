using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccionar_Audio : MonoBehaviour
{
    [SerializeField] GameObject Desactivar = null;
    [SerializeField] GameObject Activar = null;
    [SerializeField] GameObject IndicadorActivo = null;
    [SerializeField] GameObject IndicadorInactivo = null;
    // Start is called before the first frame update
    public void Escondido()
    {
        Desactivar.SetActive(false);
        Activar.SetActive(true);
        IndicadorActivo.SetActive(true);
        IndicadorInactivo.SetActive(false);

    }
}
