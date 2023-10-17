using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : ButtonBase
{
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
        gameManager.OnStartTheGame();
        upgradeButtons.SetActive(false);
    }
}
