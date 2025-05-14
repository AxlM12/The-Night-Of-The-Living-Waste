using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
public class Base_Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] protected EnemyData enemyData;
    public EnemyData EnemyData { get => enemyData; }

    [Header("External References")]
    [SerializeField] protected Grid grid;
    public Grid Grid { get => grid; set => grid = value; }
    [SerializeField] protected LayerMask DetectedLayers;
    [SerializeField] GameObject deathExplosion;
    [SerializeField] Vector3 explosionSizeMult;

    #region Behavior Variables

    [Header("Behaviour Variables")]
    [SerializeField] protected bool AllowDamage;
    [Space]
    [SerializeField] protected Vector2 MovementDuration;
    [Space]
    [SerializeField] protected bool RandomMoveInX;
    [SerializeField] protected bool RandomMoveInY;
    [Space]
    [SerializeField] protected bool JitterX;
    [SerializeField] protected bool JitterY;
    [Space]
    [SerializeField] protected AnimationCurve[] MovementCurves;

    #endregion

    #region States Behaviour

    [Header("States Behaviour")]
    [SerializeField] protected EnemyIdleBH idleBH;
    public EnemyIdleBH IdleBH { get; protected set; }

    [SerializeField] protected EnemyMovingBH movingBH;
    public EnemyMovingBH MovingBH { get; protected set; }

    [SerializeField] protected EnemyAlterStateBH alterStateBH;
    public EnemyAlterStateBH AlterStateBH { get; protected set; }

    [SerializeField] protected EnemyTakeDamageBH takeDamageBH;
    public EnemyTakeDamageBH TakeDamageBH { get; protected set; }

    //[SerializeField] EnemyDeathBH deathBH;
    //public EnemyDeathBH DeathBH { get => deathBH;}

    #endregion

    //Internal Variables
    public int CurrentHitPoints {  get; protected set; }
    protected Vector3Int MovementLimit;
    public Coroutine MoveCoroutine;

    //Internal References
    public Animator EnemAnimator {  get; protected set; }

    #region Unity Functions

    protected virtual void Awake()
    {
        IdleBH = Instantiate(idleBH);
        MovingBH = Instantiate(movingBH);
        AlterStateBH = Instantiate(alterStateBH);
        TakeDamageBH = Instantiate(takeDamageBH);
    }

    protected virtual void Start()
    {
        IdleBH.Initialize(this);
        MovingBH.Initialize(this);
        AlterStateBH.Initialize(this);
        TakeDamageBH.Initialize(this);

        EnemAnimator = GetComponent<Animator>();

        CurrentHitPoints = enemyData.MaxHitPoints;
        transform.position = grid.WorldToCell(transform.position) + (grid.cellSize / 2);
    }

    //private void Update()
    //{
    //    if (CurrentHitPoints <= 0)
    //    {
    //        EnemyDefeated();
    //    }
    //}

    protected virtual void OnDestroy()
    {
        //if(EnemySpawner.Instance != null)
        //{
        //    EnemySpawner.Instance.UpdateStatsOnEnemyDestroyed();
        //}
    }

    #endregion

    #region Intialize

    //Constructor
    //public Base_Enemy(Grid gridRef, EnemyData enemyDataRef)
    //{
    //    grid = gridRef;
    //    enemyData = enemyDataRef;
    //}

    //public Base_Enemy(Grid gridRef)
    //{
    //    grid = gridRef;
    //}

    //Init
    public void InitEnemy(Grid gridRef, EnemyData enemyDataRef)
    {
        grid = gridRef;
        enemyData = enemyDataRef;
    }

    public void InitEnemy(Grid gridRef)
    {
        grid = gridRef;
    }

    #endregion

    #region Enemy Damage Calculation

    public void SetAllowDamage(bool value)
    {
        AllowDamage = value;
    }

    public virtual void TakeDamage(int DamageTaken, bool IsInstantKill)
    {
        CurrentHitPoints -= DamageTaken;
        if (CurrentHitPoints <= 0 || IsInstantKill) EnemyDefeated();
        else EnemAnimator.SetTrigger("TookDamage");

        //if (AllowDamage && CurrentHitPoints > 0)
        //{
        //    CurrentHitPoints -= DamageTaken;
        //    if (CurrentHitPoints <= 0 || IsInstantKill) EnemyDefeated();
        //    else EnemAnimator.SetTrigger("TookDamage");
        //}
        //else
        //{
        //    EnemyDefeated();
        //}
    }

    protected virtual void EnemyDefeated()
    {
        StopAllCoroutines();
        EnemAnimator.ResetTrigger("IsAlterState");
        EnemAnimator.ResetTrigger("TookDamage");
        EnemAnimator.SetTrigger("FinishAlterState");
        EnemAnimator.SetBool("IsMoving", false);
        EnemAnimator.SetBool("IsDefeated", true);
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.UpdateStatsOnEnemyDefeated();
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateStatsOnEnemyDefeated(this);
        }
        //Destroy(gameObject);
    }

    protected void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Jitter and random axys

    protected virtual Vector3Int JitterAxys(Axis axis, Vector3Int movementVector)
    {
        switch (axis)
        {
            case Axis.X:
                return JitterXAxis(movementVector);
            case Axis.Y:
                return JitterYAxis(movementVector);
            default:
                return movementVector;
        }
    }

    protected virtual Vector3Int JitterYAxis(Vector3Int MovementVector)
    {
        int JitterAxis;
        Vector3Int JitterVector;

        JitterAxis = Random.Range(1, 11) % 2;
        switch (JitterAxis)
        {
            case 0:
                JitterVector = new Vector3Int(0, Mathf.Abs(MovementVector.y), 0);
                break;
            case 1:
                JitterVector = new Vector3Int(0, Mathf.Abs(MovementVector.y) * -1, 0);
                break;
            default:
                JitterVector = MovementVector;
                break;
        }

        return JitterVector;
    }

    protected virtual Vector3Int JitterXAxis(Vector3Int MovementVector)
    {
        int JitterAxis;
        Vector3Int JitterVector;

        JitterAxis = Random.Range(1, 11) % 2;
        switch (JitterAxis)
        {
            case 0:
                JitterVector = new Vector3Int(Mathf.Abs(MovementVector.x), 0, 0);
                break;
            case 1:
                JitterVector = new Vector3Int(Mathf.Abs(MovementVector.x) * -1, 0, 0);
                break;
            default:
                JitterVector = MovementVector;
                break;
        }

        return JitterVector;
    }

    #endregion

    #region Position Aviability

    protected virtual bool IsTileAviable(Vector3 TargetPos)
    {
        return !Physics2D.OverlapCircle(TargetPos, 0.15f, DetectedLayers);
    }

    protected virtual Vector3 GetAviablePosition(Axis Axis, Vector3 initialPosition, Vector3 targetPosition)
    {
        Vector3 movementDirection = (targetPosition - initialPosition).normalized;
        Vector3 currentPositoin = initialPosition + movementDirection;

        switch (Axis)
        {
            case Axis.X:
                switch (movementDirection.x)
                {
                    case 1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.x < targetPosition.x)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                    case -1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.x > targetPosition.x)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                }
                return currentPositoin;

            case Axis.Y:
                switch (movementDirection.y)
                {
                    case 1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.y < targetPosition.y)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                    case -1:
                        while (IsTileAviable(currentPositoin) && currentPositoin.y > targetPosition.y)
                        {
                            currentPositoin += movementDirection;
                        }
                        while (!IsTileAviable(currentPositoin))
                        {
                            currentPositoin -= movementDirection;
                        }
                        break;
                }
                return currentPositoin;

            default:
                Debug.LogError("Only X and Y are allowed");
                return Vector3.zero;
        }
    }

    //protected virtual Vector3 GetAviablePosition(Vector3 initialPosition, Vector3 targetPosition)
    //{
    //    Vector3 movementDirection = (targetPosition - initialPosition).normalized;
    //    Vector3 currentPositoin = initialPosition + movementDirection;

    //    switch (movementDirection)
    //    {
    //        case Vector3 vec when vec.x > 0f && vec.y > 0f:
    //            while (IsTileAviable(currentPositoin) && currentPositoin.x < targetPosition.x && currentPositoin.y < targetPosition.y)
    //            {
    //                currentPositoin += movementDirection;
    //            }
    //            while (!IsTileAviable(currentPositoin))
    //            {
    //                currentPositoin -= movementDirection;
    //            }
    //            break;
    //        case Vector3 vec when vec.x < 0f && vec.y < 0f:
    //            while (IsTileAviable(currentPositoin) && currentPositoin.x > targetPosition.x && currentPositoin.y > targetPosition.y)
    //            {
    //                currentPositoin += movementDirection;
    //            }
    //            while (!IsTileAviable(currentPositoin))
    //            {
    //                currentPositoin -= movementDirection;
    //            }
    //            break;
    //    }
    //    return currentPositoin;
    //}

    #endregion

    #region Enemy Movement

    public virtual IEnumerator MoveCR(float MovementTime)
    {
        yield return new WaitForSeconds(MovementTime);
        EnemAnimator.SetBool("IsMoving", true);

        Vector3 InitialPosition;
        Vector3 TargetPosition;
        //Vector3 MovementDirection;
        //Vector3 CurrentPos;
        float TimeElapsed;

        #region Movement In X

        var UseRandomMoveInX = RandomMoveInX ? MovementLimit.x = Random.Range(1, Mathf.Abs(enemyData.MovementVector.x + 1)) : MovementLimit.x = enemyData.MovementVector.x;

        InitialPosition = transform.position;
        if (JitterX)
        {
            TargetPosition = InitialPosition + JitterXAxis(MovementLimit);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(-MovementLimit.x, 0, 0);
        }

        TargetPosition = GetAviablePosition(Axis.X, InitialPosition, TargetPosition);
        //TargetPosition = GetAviablePosition(InitialPosition, TargetPosition);

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.x)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurves[0].Evaluate(TimeElapsed / MovementDuration.x));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        #region Movement In Y

        var UseRandomMoveInY = RandomMoveInY ? MovementLimit.y = Random.Range(1, Mathf.Abs(enemyData.MovementVector.y + 1)) : MovementLimit.y = enemyData.MovementVector.y;

        InitialPosition = transform.position;
        if (JitterY)
        {
            TargetPosition = InitialPosition + JitterYAxis(MovementLimit);
        }
        else
        {
            TargetPosition = InitialPosition + new Vector3Int(0, MovementLimit.y, 0);
        }

        TargetPosition = GetAviablePosition(Axis.Y, InitialPosition, TargetPosition);
        //TargetPosition = GetAviablePosition(InitialPosition, TargetPosition);

        TimeElapsed = 0f;
        while (TimeElapsed < MovementDuration.y)
        {
            transform.position = Vector3.Lerp(InitialPosition, TargetPosition, MovementCurves[1].Evaluate(TimeElapsed / MovementDuration.y));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = TargetPosition;

        #endregion

        EnemAnimator.SetBool("IsMoving", false);
    }

    public IEnumerator MoveForward(float movementTime, float times)
    {
        Vector3 initalPos = transform.position;
        Vector3 finalPos = initalPos + Vector3.left * times;
        float timeElapsed = 0f;

        while (timeElapsed < movementTime)
        {
            transform.position = Vector3.Lerp(initalPos, finalPos, timeElapsed / movementTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = finalPos;
    }

    public IEnumerator MoveForward(float movementTime, float yOffset, float times)
    {
        Vector3 initalPos = transform.position;
        Vector3 finalPos = initalPos + Vector3.left * times + new Vector3(0, yOffset, 0);
        float timeElapsed = 0f;

        while (timeElapsed < movementTime)
        {
            transform.position = Vector3.Lerp(initalPos, finalPos, timeElapsed / movementTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = finalPos;
    }

    #endregion

    #region Animation Events

    public void InstantiateExplosion()
    {
        Vector3 sizeMult = new Vector3(0.5f, 0.5f, 0f);
        if(explosionSizeMult != null && explosionSizeMult != Vector3.zero)
        {
            sizeMult = explosionSizeMult;
        }

        if (deathExplosion != null)
        {
            var explosion = Instantiate(deathExplosion, transform);
            explosion.transform.localScale += sizeMult;
        }
    }

    public void DestroyExplosion()
    {

    }

    #endregion

    protected virtual void EnemyAbility()
    {

    }
}
