using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hit : StateMachineBehaviour
{
    Base_Weapon weapon;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weapon = animator.GetComponent<Base_Weapon>();
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound(weapon.WeaponDataSO.HitSound);
        weapon.Particles.Play();
        if (weapon.WeaponDataSO.EffectSound != null)
        {
            weapon.EffectSound = AudioManager.Instance.CreateEventInstance(weapon.WeaponDataSO.EffectSound.Event);
            weapon.EffectSound.start();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weapon.EffectSound.stop(STOP_MODE.ALLOWFADEOUT);
        weapon.EffectSound.release();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
