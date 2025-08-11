using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Fmod Events")]
    [SerializeField] FmodEvent bgmEvent;
    [SerializeField] FmodEvent ambienceEvent;
    [SerializeField] float ambienceIntensity;
    [SerializeField] FmodEvent gamePausedSnapshot;

    //Buses
    private Bus MasterBus;
    private Bus MusicBus;
    private Bus SFXBus;

    //Event Instances
    EventInstance MainBGM;
    EventInstance AmbienceSound;
    EventInstance GamePaused;

    //Singleton Instance
    public static AudioManager Instance;

    #region Unity Functions

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        MasterBus = RuntimeManager.GetBus("bus:/");
        MusicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
    }

    void Start()
    {
        if (bgmEvent != null)
        {
            InitializeBMG(bgmEvent.Event);
        }
        else
        {
            Debug.LogWarning("No BGM event selected on inspector", gameObject);
        }

        if (ambienceEvent != null)
        {
            InitializeAmbience(ambienceEvent.Event);
            ChangeAmbienceIntensity(ambienceIntensity);
        }
        else
        {
            Debug.LogWarning("No ambience event selected on inspector", gameObject);
        }

        if(gamePausedSnapshot != null)
        {
            InitializeGamePausedSnapshot(gamePausedSnapshot.Event);
        }
        else
        {
            Debug.LogWarning("No Game Paused event selected on inspector", gameObject);
        }
    }

    private void OnDestroy()
    {
        StopBGM();
        StopAmbienceSound();
        RelaseGamePausedSnapshot();
    }

    #endregion

    #region Bus Control

    public void SetMasterVolume(float volume)
    {
        MasterBus.setVolume(volume);
    }

    public void SetMuteMusicBus(bool mute)
    {
        MusicBus.setMute(mute);
    }

    public void SetMuteSfxBus(bool mute)
    {
        SFXBus.setMute(mute);
    }

    #endregion

    public EventInstance CreateEventInstance(EventReference fmodEventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEventReference);
        return eventInstance;
    }

    #region Ambience Control

    void InitializeAmbience(EventReference AmbienceRef)
    {
        AmbienceSound = CreateEventInstance(AmbienceRef);
        AmbienceSound.start();
    }

    public void ChangeAmbienceIntensity(float intensity)
    {
        AmbienceSound.setParameterByName("Ambience_Intensity", intensity);
    }

    public void StopAmbienceSound()
    {
        AmbienceSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AmbienceSound.release();
    }

    #endregion

    #region BGM Control

    void InitializeBMG(EventReference BGMEventRef)
    {
        MainBGM = CreateEventInstance(BGMEventRef);
        MainBGM.start();
    }

    public void ChangeBGMIntensity(GameManager.LevelState levelState)
    {
        //int numLevelState = 0;
        //switch (levelState)
        //{
        //    case GameManager.LevelState.Soft:
        //        numLevelState = 0;
        //        break;
        //    case GameManager.LevelState.Medium:
        //        numLevelState = 1;
        //        break;
        //    case GameManager.LevelState.Hard:
        //        numLevelState = 2;
        //        break;
        //    case GameManager.LevelState.Finish:
        //        MainBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //        MainBGM.release();
        //        return;
        //}
        MainBGM.setParameterByName("Gameplay_Music_Intensity", ((int)levelState));
    }

    public void ChangeBGMIntensity(int musicIntensity)
    {
        MainBGM.setParameterByName("Gameplay_Music_Intensity", musicIntensity);
    }

    public void ChangeEndingSound(EndingData.EndingType endingType)
    {
        MainBGM.setParameterByName("Ending_Type", ((int)endingType));
    }

    public void StopBGM()
    {
        MainBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        MainBGM.release();
    }

    #endregion

    #region Game Paused Control

    void InitializeGamePausedSnapshot(EventReference Eventref)
    {
        GamePaused = CreateEventInstance(Eventref);
    }

    public void StartGamePausedSnapshot()
    {
        GamePaused.start();
    }

    public void StopGamePausedSnapshot()
    {
        GamePaused.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void RelaseGamePausedSnapshot()
    {
        GamePaused.release();
    }

    #endregion

    public void PlaySound(FmodEvent eventToPlay)
    {
        RuntimeManager.PlayOneShot(eventToPlay.Event);
    }

    public void PlaySound(EventReference eventReference)
    {
        RuntimeManager.PlayOneShot(eventReference);
    }
}
