using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBase : MonoBehaviour
{
    [Header("Properties")]
    //[SerializeField] protected EnemyInfos enemyInfos;
    [SerializeField] private ParticleSystem[] hitParticles;
    [SerializeField] protected int enemylevel = 1;
    protected float size;
    protected float power;
    protected int money;

    [Header("Animation")]
    [SerializeField] protected SimpleAnimancer animancer;

    [SerializeField] private AnimationClip run;
    [SerializeField] private AnimationClip fall;
    [SerializeField] private AnimationClip win;

    protected bool canMove = false;
    protected bool isRunning = false;
    private Rigidbody rb;
    private Collider col;

    private GameManager gameManager;

    public bool IsBoss
    {
        get => IsBoss;
    }

    public void Init()
    {
        gameManager = GameManager.Instance;
        SetProperties();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        ActionManager.GameEnd += OnGameEnd;
        ActionManager.ManagerUpdate += MoveTowardsPlayer;
        ActionManager.BossMove += OnBossMove;
    }

    public virtual void DeInit()
    {
        StopAllCoroutines();
        StopDragging();
        canMove = false;
        ActionManager.GameEnd -= OnGameEnd;
        ActionManager.ManagerUpdate -= MoveTowardsPlayer;
        ActionManager.BossMove -= OnBossMove;
    }

    public void MoveTowardsPlayer(float deltaTime)
    {
        if (!canMove) return;


        rb.AddForce(Vector3.right * power * deltaTime / 50, ForceMode.VelocityChange);
    }

    public float TakeHit(float hitAmount)
    {
        hitParticles[Random.Range(0, hitParticles.Length)].Play();
        StopDragging();
        rb.AddForce(Vector3.left * hitAmount / size / 25, ForceMode.VelocityChange);
        StartCoroutine(FallCo());
        return power;
    }

    private IEnumerator FallCo()
    {
        animancer.PlayAnimation(fall);
        canMove = false;
        yield return new WaitForSeconds(fall.length + 0.1f);
        canMove = true;
        animancer.PlayAnimation(run);
        StopDragging();
    }

    public void StopDragging()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cliff"))
        {
            gameManager.Money = money;
            Die();
        }
    }

    private void OnBossMove()
    {
        canMove = true;
        animancer.PlayAnimation(run);
    }

    private void OnGameEnd(bool playerWin)
    {
        DeInit();
        
        if (playerWin)
        {
            OnPlayerWin();
            return;
        }
        OnPlayerLose();
    }

    private void OnPlayerWin()
    {
        animancer.PlayAnimation(fall);
    }

    private void OnPlayerLose()
    {
        animancer.PlayAnimation(win);
    }

    private void Die()
    {
        rb.useGravity = true;
        col.enabled = false;
        StopAllCoroutines();
        ActionManager.GameEnd?.Invoke(true);
    }

    private void SetProperties()
    {
        /*if (enemylevel > enemyInfos.GetCharacterPrefs.Length) enemylevel = enemyInfos.GetCharacterPrefs.Length;
        EnemyInfos.CharacterPref currentLevel = enemyInfos.GetCharacterPrefs[enemylevel - 1];

        speed = currentLevel.speed;
        size = currentLevel.size;
        power = currentLevel.power;*/

        var enemyConfig = EnemyConfigUtility.GetEnemyConfigByLevel((byte)enemylevel);
        size = enemyConfig.Size;
        power = enemyConfig.Power;
        money = enemyConfig.Money;
    }
}