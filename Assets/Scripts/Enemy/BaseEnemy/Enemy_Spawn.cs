using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawn : StateMachineBehaviour
{
    Base_Enemy enemy;
    Collider2D collider;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Base_Enemy>();
        collider = animator.GetComponent<Collider2D>();
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound(enemy.EnemyData.MainSound.Event);
        //enemy.StartCoroutine(enemy.MoveForward(2f, 1f));
        //enemy.StartCoroutine(enemy.MoveForward(animator.GetCurrentAnimatorClipInfo(0).Length, 1f));
        enemy.StartCoroutine(enemy.MoveForward(animator.GetCurrentAnimatorStateInfo(0).length, 1f));
        //collider.enabled = false;
        collider.offset = Vector2.left * 2;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //collider.enabled = true;
        collider.offset = Vector2.zero;
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
