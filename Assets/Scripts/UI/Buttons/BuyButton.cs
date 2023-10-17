using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuyButton : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Color32 green;

    private GridManager gridManager;
    private GameManager gameManager;
    private UIManager uiManager;
    private ObjectPooler pooler;

    [Header("Button Prices")]
    [SerializeField] private int startPrice;
    [SerializeField] private int incrementalPrice;

    [Header("Panel Settings")]
    [SerializeField] private TMP_Text priceText;

    public void Init()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        pooler = ObjectPooler.Instance;
        gridManager = FindObjectOfType<GridManager>();

        ActionManager.MergeColorCheck += OnButtonCondition;
        ActionManager.ButtonCheck += OnButtonCondition;

        SetButtons();

        MergeableBall mergeBall = pooler.GetPooledBall(1);
        mergeBall.gameObject.SetActive(true);
        mergeBall.Init();
        ActionManager.FindEmptyGrid?.Invoke(mergeBall.gameObject);
    }

    public void DeInit()
    {
        ActionManager.MergeColorCheck -= OnButtonCondition;
        ActionManager.ButtonCheck -= OnButtonCondition;
        ClearPlayerPrefs();
    }

    public void OnButtonClick()
    {
        GameManager.Haptic(0);
        gameManager.Money = -CurrentPrice;

        transform.DOKill(true);
        transform.DOScale(Vector3.one, 0);
        button.enabled = false;

        MergeableBall mergeBall = pooler.GetPooledBall(1);
        mergeBall.gameObject.SetActive(true);
        mergeBall.GetComponent<MergeableBall>().Init();
        ActionManager.FindEmptyGrid?.Invoke(mergeBall.gameObject);
        transform.DOPunchScale(Vector3.one, 0, 6);
        transform.DOPunchScale(Vector3.one * 0.21f, 0.25f, 6).SetUpdate(true).OnComplete(() => { OnButtonCondition(false); });

        SetButtonPrice();
        SetButtons();

    }

    public void SetButtons()
    {
        priceText.text = "" + uiManager.FormatFloatToReadableString(CurrentPrice);
        //OnButtonCondition(true);
    }

    private void OnButtonCondition(bool check)
    {
        if (check)
        {
            CloseButton();
            return;
        }
        if (gridManager.EmptyCheck() && gameManager.Money >= CurrentPrice)
        {
            OpenButton();
            return;
        }
        CloseButton();
        SetButtons();
    }

    private void OpenButton()
    {
        button.enabled = true;
        image.color = green;
    }

    private void CloseButton()
    {
        button.enabled = false;
        image.color = Color.gray;
    }

    private void SetButtonPrice()
    {
        CurrentPrice = startPrice + IncrementalPrice;
        IncrementalPrice = IncrementalPrice;
        priceText.text = "" + uiManager.FormatFloatToReadableString(CurrentPrice);
    }

    public int CurrentPrice
    {
        get => PlayerPrefs.GetInt(ConstantVariables.BuyBallButton.BuyButton + ConstantVariables.UpgradePrices.CurrentPrice, startPrice);
        set => PlayerPrefs.SetInt(ConstantVariables.BuyBallButton.BuyButton + ConstantVariables.UpgradePrices.CurrentPrice, value);
    }

    public int IncrementalPrice
    {
        get => PlayerPrefs.GetInt(ConstantVariables.BuyBallButton.BuyButton + ConstantVariables.UpgradePrices.IncrementalPrice, incrementalPrice);
        set
        {
            PlayerPrefs.SetInt(ConstantVariables.BuyBallButton.BuyButton + ConstantVariables.UpgradePrices.IncrementalPrice, 
                PlayerPrefs.GetInt(ConstantVariables.BuyBallButton.BuyButton + ConstantVariables.UpgradePrices.IncrementalPrice, incrementalPrice) + value);
        }
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(ConstantVariables.BuyBallButton.BuyButton + ConstantVariables.UpgradePrices.CurrentPrice);
        PlayerPrefs.DeleteKey(ConstantVariables.BuyBallButton.BuyButton + ConstantVariables.UpgradePrices.IncrementalPrice);
    }
}
