using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iman : Base_Weapon
{
    [Header("Unique Variables")]
    [SerializeField] ParticleEffect particleEffect;

    public override void HitOnPosition(Vector3 hitPoint)
    {
        base.HitOnPosition(hitPoint);
        if(particleEffect != null) particleEffect.particles.Play();
    }

    protected override void DamageOnce(Vector3 hitPoint)
    {
        Collider2D[] Colliders = Physics2D.OverlapBoxAll(hitPoint, weaponDataSO.AttackRange, 0f);
        foreach (Collider2D col in Colliders)
        {
            if (col.gameObject.TryGetComponent(out Base_Enemy Enem))
            {
                //if (weaponDataSO.BaseDamage > 0) Enem.TakeDamage(weaponDataSO.BaseDamage, isInstantKill);
                if (useSpecialEffect) SpecialEffect(Enem);
                //print("EnemyFound");
            }
        }
        HitPosition = hitPoint;
    }

    public override void DisableWeapon()
    {
        if (particleEffect != null) particleEffect.particles.Stop();
        base.DisableWeapon();
    }

    protected override void SpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            enemy.TakeDamage(weaponDataSO.BaseDamage * 2, false);
            enemy.AlterStateBH.stateDuration = EffectDuration;
            enemy.EnemAnimator.SetTrigger("IsAlterState");
            //if (enemy.CurrentHitPoints - weaponDataSO.BaseDamage * 2 >= 0)
            //{
            //    enemy.AlterStateBH.stateDuration = EffectDuration;
            //    enemy.EnemAnimator.SetTrigger("IsAlterState");
            //}
            //enemy.TakeDamage(weaponDataSO.BaseDamage * 2, false);
        }
        else
        {
            enemy.TakeDamage(weaponDataSO.BaseDamage, isInstantKill);
        }
    }
}
