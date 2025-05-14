using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BarrierTile : GridTile
{
    [SerializeField] bool destroyEnemies;
    //[SerializeField] bool destroyWeapons;
    [SerializeField] float destroyDelay;
    [SerializeField] bool drawGizmos;

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (IsEnemyOnTile(collision, out Base_Enemy enemy))
        //{
        //    //switch (destroyAll)
        //    //{
        //    //    case false:
        //    //        if (destroyCategories.Contains(collidingEnemy.EnemyData.EnemyCategory))
        //    //        {
        //    //            Destroy(collidingEnemy, destroyDelay);
        //    //            GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
        //    //        }
        //    //        break;
        //    //    case true:
        //    //        Destroy(collidingEnemy, destroyDelay);
        //    //        GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
        //    //        break;
        //    //}
        //    Destroy(enemy, destroyDelay);
        //    GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
        //}
        if (destroyEnemies && collision.gameObject.TryGetComponent(out Base_Enemy enemy))
        {
            Destroy(enemy.gameObject, destroyDelay);
            GameManager.Instance.UpdateStatsOnEnemyOutOfGameArea();
            Debug.LogWarning("Enemy Out Of Game Area at", gameObject);
        }

        //if(destroyWeapons && collision.gameObject.TryGetComponent(out Base_Weapon weapon))
        //{
        //    if(weapon.wpState == Base_Weapon.State.Active) weapon.DisableWeapon();
        //    weapon.DisableWeapon();
        //    GameManager.Instance.CurrentRecyclePoints += weapon.WeaponDataSO.BaseUseCost;
        //    Debug.LogWarning("Weapon Out Of Area at", gameObject);
        //}
    }

}
