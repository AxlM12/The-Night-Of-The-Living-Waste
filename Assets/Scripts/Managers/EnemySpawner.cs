using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float SpawnDelay;
    [SerializeField] int EnemyCap;

    [Header("Enemies To Spawn")]
    [SerializeField] List<Base_Enemy> aviableEnemies;

    [Header("External References")]
    [SerializeField] GridManager gridManager;

    [Header("Events")]
    public UnityEvent OnEnemySpawned;

    //Internal Variables
    float CurrentSpawnDelay;
    public TrashCan currentTrashCan;
    [field: SerializeField] public int CurrentEnemyCount { get; private set; }
    [field: SerializeField] public int DefeatedEnemyCount { get; private set; }

    //SingletonInstance
    public static EnemySpawner Instance;

    #region UnityFunctions

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DefeatedEnemyCount = 0;
        CurrentSpawnDelay = 0f;
        CurrentEnemyCount = 0;
    }

    void Update()
    {
        CurrentEnemyCount = gridManager.CountEnemiesOnGrid();
        if (EnemyCap <= 0/* && GameManager.Instance.CurrentLevelState != GameManager.LevelState.Soft*/) EnemyCap = 1;

        CurrentSpawnDelay += Time.deltaTime;
        if (CurrentSpawnDelay > SpawnDelay && CurrentEnemyCount < EnemyCap && GameManager.Instance.RemainingEnemies > 0)
        {
            OnEnemySpawned.Invoke();
            CurrentSpawnDelay = 0f;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnEnemySpawned.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.left, 0.25f);
        Gizmos.DrawWireSphere(transform.position + (Vector3.left * 2), 0.25f);
    }

    #endregion

    #region Update Variables

    public void UpdateStatsOnEnemyDefeated()
    {
        DefeatedEnemyCount++;
    }

    public void UpdateStatsOnEnemySpawned()
    {
        CurrentEnemyCount++;
    }

    public void UpdateCurrentTrashCan()
    {
        SpawnTile spawnTile;

        currentTrashCan = null;
        spawnTile = Physics2D.OverlapCircle(transform.position, 0.2f).GetComponent<SpawnTile>();
        currentTrashCan = spawnTile.GetComponentInChildren<TrashCan>();

        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.25f);
        //var trashCan = colliders.FirstOrDefault(c => c.GetComponent<TrashCan>());
        //currentTrashCan = trashCan.GetComponent<TrashCan>();
    }

    #endregion

    #region Spawn Enemy

    Base_Enemy SelectEnemyToSpawn()
    {
        Base_Enemy selectedEnemy = null;

        switch (GameManager.Instance.CurrentLevelState)
        {
            case GameManager.LevelState.Soft:
                SpawnDelay = (float)Random.Range(5, 7);
                EnemyCap = GameManager.Instance.TotalEnemiesOnLevel / 5;
                selectedEnemy = SelectEnemyBasedOnPorcentage(90, 10, 0, 0);
                break;
            case GameManager.LevelState.Medium:
                SpawnDelay = (float)Random.Range(3, 5);
                EnemyCap = GameManager.Instance.TotalEnemiesOnLevel / 4;
                selectedEnemy = SelectEnemyBasedOnPorcentage(15, 50, 30, 0);
                break;
            case GameManager.LevelState.Hard:
                SpawnDelay = (float)Random.Range(1, 3);
                EnemyCap = GameManager.Instance.TotalEnemiesOnLevel / 3;
                selectedEnemy = SelectEnemyBasedOnPorcentage(30, 50, 35, 0);
                break;
            case GameManager.LevelState.Finish:
                break;
        }

        if (selectedEnemy != null)
        {
            print($"Enemy {selectedEnemy}, enemy category {selectedEnemy.EnemyData.EnemyCategory}");
            return selectedEnemy;
        }
        else
        {
            return aviableEnemies[Random.Range(0, aviableEnemies.Count())];
        }

        Base_Enemy SelectEnemyBasedOnPorcentage(float LivingChance, float WalkingChance, float ConsientChance, float SelfconscientChance)
        {
            EnemyData.EnemyCategories selectedCategory = EnemyData.EnemyCategories.None;

            EnemyData.EnemyCategories[] catogoriesForSpawn = { 
                EnemyData.EnemyCategories.Viviente,
                EnemyData.EnemyCategories.Andante,
                EnemyData.EnemyCategories.Consiente, 
                EnemyData.EnemyCategories.Autoconsciente };

            float[] Chances = { LivingChance, WalkingChance, ConsientChance, SelfconscientChance };
            float totalChances = 0;

            float numberForAdding = 0;
            float randomNumber = Random.Range(0f, 1f);

            foreach(float num in Chances)
            {
                totalChances += num;
            }

            for (int i = 0; i < catogoriesForSpawn.Length; i++)
            {
                if (Chances[i]/ totalChances + numberForAdding >= randomNumber)
                {
                    print($"Spawn {catogoriesForSpawn[i]}");
                    selectedCategory = catogoriesForSpawn[i];
                }
                else
                {
                    numberForAdding += Chances[i] / totalChances;
                }
            }

            List<Base_Enemy> enemySpawnPool = aviableEnemies.Where(e => e.EnemyData.EnemyCategory == selectedCategory).ToList();

            if (enemySpawnPool.Count() > 0) return enemySpawnPool[Random.Range(0, enemySpawnPool.Count())];
            else return null;
        }
    }

    Base_Enemy SelectEnemyToSpawn(EnemyData.EnemyCategories category)
    {
        return aviableEnemies.FirstOrDefault(e => e.EnemyData.EnemyCategory == category);
    }

    public void SpawnEnemy()
    {
        Base_Enemy EnemySelected = SelectEnemyToSpawn();

        //do
        //{
        //    //int spawnHeight = Random.Range(0, GridSize.y - 1);
        //    int spawnHeight = Random.Range(0, gridManager.GetGridSize().y - 1);
        //    transform.position = new Vector3(transform.position.x, spawnHeight + gridManager.GridCellCenter().y, 0f);
        //} while (Physics2D.OverlapCircle(transform.position + Vector3.left, 0.25f, LayerMask.GetMask("Enemy")) || Physics2D.OverlapCircle(transform.position + (Vector3.left * 2), 0.25f, LayerMask.GetMask("Enemy")));

        int spawnHeight = Random.Range(0, gridManager.GetGridSize().y - 1);
        transform.position = new Vector3(transform.position.x, spawnHeight + gridManager.GridCellCenter().y, 0f);

        if (Physics2D.OverlapCircle(transform.position + Vector3.left, 0.25f, LayerMask.GetMask("Enemy")) || Physics2D.OverlapCircle(transform.position + (Vector3.left * 2), 0.25f, LayerMask.GetMask("Enemy")))
        {
            return;
        }

        UpdateCurrentTrashCan();

        //if (GameManager.Instance.RemainingEnemies == 1 && aviableEnemies.Find(e => e.EnemyData.EnemyCategory == EnemyData.EnemyCategories.Autoconsciente))
        //{
        //    EnemySelected = SelectEnemyToSpawn(EnemyData.EnemyCategories.Autoconsciente);
        //    transform.position = new Vector3(transform.position.x, gridManager.GetGridSize().y/2, 0f);
        //    Base_Enemy EnemySpawned = Instantiate(EnemySelected, transform.position + Vector3.left * EnemySelected.transform.localScale.x, Quaternion.identity);
        //    EnemySpawned.InitEnemy(gridManager.Grid);

        //    //if(currentTrashCan != null) currentTrashCan.UpdateVisual(3);
        //}
        //else
        //{
        //    Base_Enemy EnemySpawned = Instantiate(EnemySelected, transform.position, Quaternion.identity);
        //    EnemySpawned.InitEnemy(gridManager.Grid);
        //    if (currentTrashCan != null)
        //    {
        //        currentTrashCan.UpdateVisual(GameManager.Instance.CurrentLevelState);
        //        currentTrashCan.animator.SetTrigger("IsOpen");
        //        currentTrashCan.PlaySound();
        //    }
        //}

        switch (GameManager.Instance.RemainingEnemies)
        {
            case 1:
                if (aviableEnemies.Find(e => e.EnemyData.EnemyCategory == EnemyData.EnemyCategories.Autoconsciente))
                {
                    BossSpawn();
                }
                else
                {
                    NormalSpawn(3);
                }
                break;
            default:
                NormalSpawn((int)GameManager.Instance.CurrentLevelState);
                break;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateStatsOnEnemySpawned();
        }

        CurrentEnemyCount++;

        void NormalSpawn(int trashcanVisualIndex)
        {
            Base_Enemy EnemySpawned = Instantiate(EnemySelected, transform.position, Quaternion.identity);
            EnemySpawned.InitEnemy(gridManager.Grid);
            if (currentTrashCan != null)
            {
                currentTrashCan.UpdateVisual(trashcanVisualIndex);
                currentTrashCan.animator.SetTrigger("IsOpen");
                currentTrashCan.PlaySound();
            }
        }

        void BossSpawn()
        {
            EnemySelected = SelectEnemyToSpawn(EnemyData.EnemyCategories.Autoconsciente);
            transform.position = new Vector3(transform.position.x, gridManager.GetGridSize().y / 2, 0f);
            Base_Enemy EnemySpawned = Instantiate(EnemySelected, transform.position + Vector3.left * EnemySelected.transform.localScale.x, Quaternion.identity);
            EnemySpawned.InitEnemy(gridManager.Grid);
        }
    }

    #endregion
}
