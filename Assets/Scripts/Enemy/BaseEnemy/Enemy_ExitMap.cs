using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ExitMap : StateMachineBehaviour
{
    Base_Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Base_Enemy>();

        enemy.EnemAnimator.SetBool("IsMoving", false);
        animator.ResetTrigger("TookDamage");
        enemy.SetAllowDamage(false);
        enemy.StopAllCoroutines();
        enemy.StartCoroutine(enemy.MoveForward(5f, animator.GetFloat("ExitYOffset"), 6f));
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound(enemy.EnemyData.MainSound);

        Destroy(enemy.gameObject, 4.2f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
