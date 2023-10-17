using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public static event Action<bool> GameOver;

    [Header("PlayerPrefs")]
    [SerializeField] private bool clearPlayerPrefs;

    [Header("Money Settings")]
    [SerializeField] private int addMoney = 0;
    private float moneyMultiplier = 1;

    private PlayerManager playerManager;
    private HcLevelManager hcLevelManager;
    private UIManager uIManager;
    private UpdateManager updateManager;
    private GridManager gridManager;
    private CamManager camManager;
    private ObjectPooler pooler;

    public float MoneyMultipler
    {
        get => moneyMultiplier;
        set => moneyMultiplier = value;
    }

    public int Money
    {
        get => PlayerPrefs.GetInt("TotalMoney", 0);
        set
        {
            float calculatedMoney = (float)value;
            if (value > 0)
            {
                calculatedMoney = (float)value * moneyMultiplier;
            }
            PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney", 0) + (int)calculatedMoney);
            UIManager.Instance.SetMoneyUI(Money, true);
        }
    }

    void Start()
    {
        if (clearPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            Money = addMoney;
        }

        if(Money <= 0) Money = addMoney;
        Money = 0;

        SetInstances();
        SetInits();
    }

    private void SetInstances()
    {
        hcLevelManager = HcLevelManager.Instance;
        uIManager = UIManager.Instance;
        pooler = ObjectPooler.Instance;
        updateManager = FindObjectOfType<UpdateManager>();
        camManager = FindObjectOfType<CamManager>();
    }

    private void SetInits()
    {
        hcLevelManager.Init();
        gridManager = FindObjectOfType<GridManager>();
        gridManager.Init();
        uIManager.Init();
        updateManager.Init();
        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.Init();
        camManager.Init();
    }

    private void DeInits()
    {
        uIManager.DeInit();
        updateManager.DeInit();
        playerManager.DeInit();
        camManager.DeInit();
        pooler.ClosePooledObjects();
    }

    public void OnUpgradePanel()
    {
        camManager.SetCamPosToPlayer();
        pooler.ClosePooledBalls();
        gridManager.DeInit();
    }

    public void OnStartTheGame()
    {
        playerManager.StartToRun();
    }

    public void OnLevelSucceed()
    {
        hcLevelManager.LevelUp();
        hcLevelManager.DeInit();
        DeInits();
        SetInits();
    }

    public void OnLevelFailed()
    {
        hcLevelManager.DeInit();
        DeInits();
        SetInits();
    }

    public void FinishTheGame(bool check)
    {
        if (check)
        {
            ActionManager.GameEnd?.Invoke(true);
        }
        else
        {
            ActionManager.GameEnd?.Invoke(false);
        }
        DeInits();
    }

    public static void Haptic(int type)
    {
        if (type == 0)
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        else if (type == 1)
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        }
        else if (type == 2)
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        }
    }
}
