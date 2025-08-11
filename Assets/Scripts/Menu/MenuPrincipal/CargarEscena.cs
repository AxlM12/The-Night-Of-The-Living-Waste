using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
    public string NombreEscena;
    public FmodEvent MenuSfx;

    public void SceneLoader()
    {
        //if (AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.StopBGM();
        //    AudioManager.Instance.StopAmbienceSound();
        //}
        SceneManager.LoadScene(NombreEscena);
    }

    public void SceneLoader(bool CheckProfile)
    {
        if (SesionManager.CurrentSesion == null)
        {
            SceneManager.LoadScene("Menu_Perfiles");
        }
        else
        {
            SceneManager.LoadScene(NombreEscena);
        }
    }

    public void PlayButtonSFX()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(MenuSfx);
        }
    }

    public void QuitGameDelay(float delay)
    {
        Invoke("QuitGame", delay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
