using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class BarrilDePetroleo : Base_Enemy
{
    //protected override void Start()
    //{
    //    enemAnimator = GetComponent<Animator>();
    //    base.Start();
    //}

    public override IEnumerator MoveCR(float MovementTime)
    {
        yield return new WaitForSeconds(MovementTime);
        EnemAnimator.SetBool("IsMoving", true);

        Vector3 targetPosition = Vector3.zero;
        Vector3 initialPosition;

        initialPosition = transform.position;

        var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = Random.Range(1, Mathf.Abs(enemyData.MovementVector.x + 1)) : MovementLimit.x = enemyData.MovementVector.x;
        var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(1, Mathf.Abs(enemyData.MovementVector.y + 1)) : MovementLimit.y = enemyData.MovementVector.y;

        int DirectionIndicator = Random.Range(2, 5) % 2;
        switch (DirectionIndicator)
        {
            case 0: //Choose x
                if (JitterX)
                {
                    targetPosition = transform.position + JitterAxys(Axis.X, MovementLimit);
                }
                else
                {
                    targetPosition = initialPosition + MovementLimit;
                }
                targetPosition.y = initialPosition.y;
                targetPosition = GetAviablePosition(Axis.X, initialPosition, targetPosition);
                break;
            case 1: //Choose y
                if (JitterY)
                {
                    targetPosition = transform.position + JitterAxys(Axis.Y, MovementLimit);
                }
                else
                {
                    targetPosition = initialPosition + MovementLimit;
                }
                targetPosition.x = initialPosition.x;
                targetPosition = GetAviablePosition(Axis.Y, initialPosition, targetPosition);
                break;
        }

        float TimeElapsed;
        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.x)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, MovementCurves[0].Evaluate(TimeElapsed / MovementDuration.x));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        initialPosition = targetPosition;

        switch (DirectionIndicator)
        {
            case 0:
                targetPosition.y += Random.Range(-1f, 1f);
                targetPosition = GetAviablePosition(Axis.Y, initialPosition, targetPosition);
                break;
            case 1:
                targetPosition.x += Random.Range(-1f, 1f);
                targetPosition = GetAviablePosition(Axis.X, initialPosition, targetPosition);
                break;
        }

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.x)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, MovementCurves[1].Evaluate(TimeElapsed / MovementDuration.y));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        EnemAnimator.SetBool("IsMoving", false);
    }
}
