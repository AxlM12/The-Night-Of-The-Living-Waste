using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death : StateMachineBehaviour
{
    Base_Enemy Enemy;
    Collider2D collider;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy = animator.GetComponent<Base_Enemy>();
        collider = animator.GetComponent<Collider2D>();

        collider.enabled = false;
        Enemy.SetAllowDamage(false);
        Enemy.StopAllCoroutines();
        animator.ResetTrigger("TookDamage");
        animator.SetBool("IsMoving", false);
        animator.ResetTrigger("IsAlterState");
        animator.SetTrigger("FinishAlterState");
        Enemy.transform.position = Enemy.Grid.WorldToCell(Enemy.transform.position) + (Enemy.Grid.cellSize / 2);
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound(Enemy.EnemyData.DeathSound);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Destroy(Enemy.gameObject);
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
