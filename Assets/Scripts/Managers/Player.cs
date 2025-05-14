using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ChrState State;

    Animator animator;
    WeaponSpawner wpSpawner;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        wpSpawner = GetComponent<WeaponSpawner>();
        State = ChrState.Idle;

        //wpSpawner.SetLaunchAviability();
    }

    // Update is called once per frame
    void Update()
    {
        wpSpawner.SetLaunchAviability();
    }

    public void SetState(ChrState newState)
    {
        //wpSpawner.SetLaunchAviability();
        State = newState;
        switch (State)
        {
            case ChrState.Idle:
                animator.ResetTrigger("Point");
                animator.ResetTrigger("Launch");
                break;
            case ChrState.Point:
                animator.SetTrigger("Idle");
                animator.SetTrigger("Point");
                break;
            case ChrState.Launch:
                if(wpSpawner.AllowLaunch == true)
                {
                    animator.ResetTrigger("Point");
                    animator.SetTrigger("Launch");
                }
                break;
        }
    }

    public void SetState(int StateFloat)
    {
        //wpSpawner.SetLaunchAviability();
        switch (StateFloat)
        {
            case 0:
                State = ChrState.Idle;
                animator.ResetTrigger("Point");
                animator.ResetTrigger("Launch");
                break;
            case 1:
                State = ChrState.Point;
                animator.SetTrigger("Idle");
                animator.SetTrigger("Point");
                break;
            case 2:
                if (wpSpawner.AllowLaunch == true)
                {
                    State = ChrState.Launch;
                    animator.SetTrigger("Launch");
                }
                break;
        }
    }

    public enum ChrState
    {
        None = -1,
        Idle = 0,
        Point = 1,
        Launch = 2,
    }
}
