using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColadorDePlastico : Base_Weapon
{
    [Header("Unique Variables")]
    [SerializeField] AnimationCurve movementCurve;
    [SerializeField] float speed;
    [SerializeField] ParticleEffect particleEffect;

    float movementDuration;


    Vector3 posChecker;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(HitPosition, new Vector3(weaponDataSO.AttackRange.x, weaponDataSO.AttackRange.y));
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(posChecker, 0.25f);
    }

    protected override void SpecialEffect(Base_Enemy enemy)
    {
        if(IsWeaponEffective(enemy))
        {
            enemy.TakeDamage(0, true);
        }
    }

    public override void HitOnPosition(Vector3 hitPoint)
    {
        StopCoroutine(SpawnProtection);
        wpCollider.enabled = false;

        RigidBody.velocity = Vector3.Lerp(RigidBody.velocity, Vector3.zero, 5f);
        transform.position = Vector3.Lerp(transform.position, hitPoint + landPositionOffset, 5f);
        transform.rotation = Quaternion.Lerp(transform.rotation, landingRotation, 5f);
        StartCoroutine(MoveForward(hitPoint));
        
    }

    public override void DisableWeapon()
    {
        if (particleEffect != null) particleEffect.particles.Stop();
        base.DisableWeapon();
    }

    #region WeaponHit

    Vector3 LimitChecker()
    {
        Vector3 initialPos = transform.position;
        Vector3 finalPos = initialPos;

        while (!Physics2D.OverlapCircle(finalPos + Vector3.right, 0.25f, LayerMask.GetMask("BarrierGrid")))
        {
            finalPos += Vector3.right;
        }

        movementDuration = (finalPos.x - initialPos.x) / speed;

        return finalPos;
    }

    IEnumerator MoveForward(Vector3 hitPoint)
    {
        float timeElapsed = 0;
        Vector3 initalPos = transform.position;
        Vector3 targetPos = LimitChecker();
        //print($"from {transform.position} to {targetPos}");
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.4f);
        if(particleEffect != null) particleEffect.particles.Play();
        while (timeElapsed < movementDuration)
        {
            transform.position = Vector3.Lerp(initalPos, targetPos, movementCurve.Evaluate(timeElapsed / movementDuration));
            timeElapsed += Time.deltaTime;
            DamageOnce(transform.position);
            posChecker = targetPos;
            yield return null;
        }
        transform.position = targetPos;
        movementDuration = 0f;
        Invoke("DisableWeapon", EffectDuration);
    }

    #endregion
}
