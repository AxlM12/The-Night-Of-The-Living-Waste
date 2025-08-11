using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public PauseState state;

    [SerializeField] GameObject menuVisual;
    [SerializeField] TextMeshProUGUI defeatedCount;
    [SerializeField] GameObject confirmVisual;

    //public static PauseMenu Instance;

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnPauseEnter()
    {
        Time.timeScale = 0f;
        menuVisual.SetActive(true);
        state = PauseState.OnPause;

        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.StartGamePausedSnapshot();
        }

        if(EnemySpawner.Instance != null)
        {
            defeatedCount.text = EnemySpawner.Instance.DefeatedEnemyCount.ToString();
        }
    }

    public void OnPauseExit()
    {
        Time.timeScale = 1f;
        menuVisual.SetActive(false);
        state = PauseState.Unpaused;
        defeatedCount.text = "0";

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopGamePausedSnapshot();
        }
    }

    public void OnEnterConfirmWindow()
    {
        confirmVisual.SetActive(true);
    }

    public void OnExitConfirmWindow()
    {
        confirmVisual.SetActive(false);
    }

    public void OnExitLevel()
    {

    }

    public enum PauseState
    {
        Unpaused,
        OnPause,
    }
}
