using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private RunnerScript runnerScript;
    [SerializeField] private MiniGamePlayerController miniGameController;
    [SerializeField] private BallBody ballBody;

    [Header("Balls")]
    [SerializeField] private GameObject[] balls;

    [Header("Upgrade Settings")]
    [SerializeField] private Transform character;
    [SerializeField] private Transform mainBall;
    private float playerPowerMultiplier = 1;

    [Header("Fight Values")]
    [SerializeField] private float playerPower = 3;
    private float gateSizeValue;
    private float gatePowerValue;
    private float currentScale;
    private float selectedBallLevel;
    private GameManager gameManager;
    private MiniGame miniGame;

    public Transform GetChararacterTransform
    {
        get => character;
    }

    public void Init()
    {
        gameManager = GameManager.Instance;
        miniGame = FindObjectOfType<MiniGame>();
        ballBody.Init();
        ActionManager.BallSelect += OnBallSelect;
        ActionManager.MiniGameStarting += OnMiniGameStart;
        ActionManager.GameEnd += OnMiniGameEnd;
    }

    public void DeInit()
    {
        ActionManager.BallSelect -= OnBallSelect;
        ActionManager.MiniGameStarting -= OnMiniGameStart;
        ActionManager.GameEnd -= OnMiniGameEnd;
    }

    public void StartToRun()
    {
        runnerScript.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        UpgradeType gateType;
        int gateValue;

        if (other.transform.parent.TryGetComponent(out CollectableBase collectable))
        {
            collectable.OnCollected(mainBall);
        }

        if (other.transform.parent.TryGetComponent(out StandartEnemy standartEnemy))
        {
            standartEnemy.OnHit(mainBall.transform, out gateType, out gateValue);
            InGameUpgrades(gateType, gateValue);
        }
    }

    public void OnRoadFinish()
    {
        ballBody.DeInit();
        runnerScript.DeInit();
    }

    #region MiniGame
    public void OnMiniGameStart()
    {
        miniGame.Init();
        StartCoroutine(MoveIntoArena(miniGame.GetBallMoveTransform.position));
    }

    private IEnumerator MoveIntoArena(Vector3 target)
    {
        miniGameController.transform.DOMove(target, 1.5f);
        yield return new WaitForSeconds(1.5f);
        ActionManager.MiniGameStarted?.Invoke();
        OnMiniGameStarted();
    }

    public void OnMiniGameStarted()
    {
        miniGameController.Init(currentScale, gatePowerValue * playerPowerMultiplier);
    }

    public void OnMiniGameEnd(bool check)
    {
        miniGameController.DeInit();
    }
    #endregion

    #region Upgrade
    public void OnBallSelect(int ballLevel)
    {
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].SetActive(false);
        }

        if (ballLevel == 0)
        {
            balls[0].SetActive(true);
            return;
        }
        selectedBallLevel = ballLevel;
        balls[ballLevel - 1].SetActive(true);
    }

    public void OnBallAdded(int ballLevel, bool isPowerUpgrade)
    {
        if (isPowerUpgrade) gatePowerValue += ballLevel;
        gatePowerValue += selectedBallLevel;

        gateSizeValue = ballLevel * 0.05f;
        selectedBallLevel *= 0.05f;
        currentScale = (gateSizeValue + mainBall.localScale.x);
        mainBall.DOScale(currentScale, 0f);
        mainBall.DOPunchScale(Vector3.one * currentScale * 0.2f, 0.3f);
    }

    public void InGameUpgrades(UpgradeType gateType, int value)
    {
        if (gateType == UpgradeType.Size)
        {
            OnBallAdded(value, false);
        }

        if (gateType == UpgradeType.Power)
        {
            gatePowerValue += value;
        }
    }

    public void OnUpgrade(UpgradeType type, float value)
    {
        switch (type)
        {
            case UpgradeType.Income:
                IncomeUpgrade(value);
                break;
            case UpgradeType.Power:
                PowerUpgrade(value);
                break;
            case UpgradeType.Size:
                SizeUpgrade(value);
                break;
            default:
                Debug.Log("NOTHING");
                break;
        }
    }

    private void IncomeUpgrade(float value)
    {
        if (gameManager == null) gameManager = GameManager.Instance;
        if (value < 1)
        {
            gameManager.MoneyMultipler = 1;
            return;
        }
        gameManager.MoneyMultipler = value;
    }

    private void PowerUpgrade(float value)
    {
        playerPowerMultiplier = value;
    }

    private void SizeUpgrade(float value)
    {
        if (value <= 1.5f)
        {
            character.DOScale(value, 0f);
            mainBall.DOScale(value, 0f);
        }
        if (value > 2) value = 1.5f;
        mainBall.DOScale(value, 0f);
        mainBall.DOPunchScale(Vector3.one * value * 0.2f, 0.3f);
    }

    #endregion
}
