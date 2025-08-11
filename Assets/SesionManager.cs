using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SesionManager
{
    //public static SesionManager Instance;

    public static string CurrentSesion = null;

    public static float MasterVolume = 1f;
    public static bool MusicAllowed = true;
    public static bool SFXAllowed = true;
    public static bool FullscreenEnabled = true;

    //private void Awake()
    //{
    //    //MasterVolume = 1.0f;
    //    //MusicMute = true;
    //    //SFXMute = true;

    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(Instance);
    //        Destroy(Instance.gameObject);
    //    }
    //    else
    //    {
    //        Instance = this;
            
    //    }

    //    DontDestroyOnLoad(gameObject);

    //}
}
