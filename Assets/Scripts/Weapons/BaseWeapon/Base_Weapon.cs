using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Rigidbody2D))]
public class Base_Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] protected WeaponData weaponDataSO;
    public WeaponData WeaponDataSO { get => weaponDataSO; }

    [Header("Weapon Behaviour Variables")]
    [SerializeField] protected bool isInstantKill;
    [SerializeField] protected bool useSpecialEffect;
    public State wpState;
    [field: SerializeField] public float EffectDuration {  get; protected set; }
    [SerializeField] DamageType damageType;
    [SerializeField] protected bool fixedLandingRotation;
    [SerializeField] protected Quaternion landingRotation;
    [SerializeField] protected Vector3 landPositionOffset;

    //Internal Referemces
    public Rigidbody2D RigidBody { get; protected set; }
    public ParticleSystem Particles {  get; protected set; }
    protected Animator animator;

    //Internal Variables
    protected Vector3 HitPosition;
    public EventInstance EffectSound;
    public Coroutine SpawnProtection;
    protected Collider2D wpCollider;

    #region Unity Functions

    protected virtual void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Particles = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
        wpCollider = GetComponent<Collider2D>();

        wpState = State.Standby;
    }

    //private void Update()
    //{
    //    switch (wpState)
    //    {
    //        case State.Standby:
    //            //transform.rotation = Quaternion.identity;
    //            CancelInvoke("SpawnProtection");
    //            break;
    //        case State.Active:
    //            Invoke("SpawnProtection", 10f);
    //            break;
    //        case State.Recharging:
    //            CancelInvoke("SpawnProtection");
    //            //transform.rotation = new Quaternion(0f,0f,0f,0f);
    //            break;
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(HitPosition, new Vector3(weaponDataSO.AttackRange.x, weaponDataSO.AttackRange.y));
    }

    #endregion

    public void ResetTransform()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public IEnumerator SpawnProtectionCR(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Finish");
        wpState = State.Recharging;
        EffectSound.stop(STOP_MODE.ALLOWFADEOUT);
        EffectSound.release();
        Invoke("SetToStandBy", weaponDataSO.BaseReloadTime);
        gameObject.SetActive(false);
    }

    public virtual void DisableWeapon()
    {
        animator.SetTrigger("Finish");
        wpState = State.Recharging;
        EffectSound.stop(STOP_MODE.ALLOWFADEOUT);
        EffectSound.release();
        Invoke("SetToStandBy", weaponDataSO.BaseReloadTime);
        gameObject.SetActive(false);
    }

    public void SetToStandBy()
    {
        wpState = State.Standby;
        wpCollider.enabled = true;
        animator.ResetTrigger("Finish");
        transform.rotation = Quaternion.identity;
    }

    //public IEnumerator Recharge()
    //{
    //    yield return new WaitForSeconds(weaponDataSO.BaseReloadTime);
    //}

    #region Weapon Hit

    public virtual void SetHitPoint(Vector3 Point)
    {
        HitPosition = Point;
    }

    public virtual void DisplayWeaponAffectedArea(GameObject displayIndicator)
    {
        displayIndicator.transform.localScale = new Vector3(weaponDataSO.AttackRange.x, weaponDataSO.AttackRange.y, 0f);
    }

    public virtual void HitOnPosition(Vector3 hitPoint)
    {
        StopCoroutine(SpawnProtection);
        wpCollider.enabled = false;

        RigidBody.velocity = Vector3.Lerp(RigidBody.velocity, Vector3.zero, 5f);
        transform.position = Vector3.Lerp(transform.position, hitPoint + landPositionOffset, 5f);
        if(fixedLandingRotation) transform.rotation = Quaternion.Lerp(transform.rotation, landingRotation, 5f);
        animator.SetTrigger("Hit");
        switch (damageType)
        {
            case DamageType.Instant:
                DamageOnce(hitPoint); 
                break;
            case DamageType.Overtime: 
                DamageOvertime(hitPoint, EffectDuration); 
                break;
            default:
                DamageOnce(hitPoint);
                break;
        }
        Invoke("DisableWeapon", EffectDuration);
    }

    protected virtual void DamageOnce(Vector3 hitPoint)
    {
        Collider2D[] Colliders = Physics2D.OverlapBoxAll(hitPoint, weaponDataSO.AttackRange, 0f);
        foreach (Collider2D col in Colliders)
        {
            if (col.gameObject.TryGetComponent(out Base_Enemy Enem))
            {
                if (weaponDataSO.BaseDamage > 0) Enem.TakeDamage(weaponDataSO.BaseDamage, isInstantKill);
                if (useSpecialEffect) SpecialEffect(Enem);
                //print("EnemyFound");
            }
        }
        HitPosition = hitPoint;
    }

    protected virtual void DamageOvertime(Vector3 hitPoint, float damageTime)
    {
        StartCoroutine(damageOverTime());

        IEnumerator damageOverTime()
        {
            float currentDmgTime = 0;
            while (currentDmgTime < damageTime)
            {
                Collider2D[] Colliders = Physics2D.OverlapBoxAll(hitPoint, weaponDataSO.AttackRange, 0f);
                HitPosition = hitPoint;
                foreach (Collider2D col in Colliders)
                {
                    if (col.gameObject.TryGetComponent(out Base_Enemy Enem))
                    {
                        if (weaponDataSO.BaseDamage > 0) Enem.TakeDamage(weaponDataSO.BaseDamage, isInstantKill);
                        if (useSpecialEffect) SpecialEffect(Enem);
                    }
                }
                currentDmgTime += Time.deltaTime;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    protected virtual void SpecialEffect(Base_Enemy enemy)
    {
        if (IsWeaponEffective(enemy))
        {
            print("Special Effect");
        }
    }

    protected bool IsWeaponEffective(Base_Enemy enemy)
    {
        if(!weaponDataSO.AffectAllMaterials || !weaponDataSO.AffectAllCategories)
        {
            var AffectedByMaterial = weaponDataSO.AffectedEnemyMaterials.Intersect(enemy.EnemyData.EnemyMaterial);
            bool AffectedByCategory = weaponDataSO.AffectedEnemyCategories.Contains(enemy.EnemyData.EnemyCategory);
            if ((AffectedByMaterial.Count() > 0 || weaponDataSO.AffectAllMaterials) && (AffectedByCategory || weaponDataSO.AffectAllCategories)) return true;
            else return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

    public enum DamageType
    {
        None,
        Instant,
        Overtime
    }

    public enum State
    {
        Standby,
        Active,
        Recharging
    }
}
