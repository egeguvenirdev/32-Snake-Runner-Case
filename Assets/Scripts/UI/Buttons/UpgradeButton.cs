using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : ButtonBase
{
    [SerializeField] private GameObject gridButtons;
    [SerializeField] private GameObject upgradeButtons;
    private GameManager gameManager;

    public override void Init()
    {
        gameManager = GameManager.Instance;
    }

    public override void DeInit()
    {

    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        gameManager.OnUpgradePanel();
        upgradeButtons.SetActive(true);
        gridButtons.SetActive(false);
    }
}
