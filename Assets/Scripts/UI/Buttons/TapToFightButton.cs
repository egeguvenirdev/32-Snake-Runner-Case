using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToFightButton : ButtonBase
{
    [SerializeField] private GameObject panelElements;

    public override void Init()
    {
        ActionManager.MiniGameStarted += OnMiniGameStart;
    }

    public override void DeInit()
    {
        ActionManager.MiniGameStarted -= OnMiniGameStart;
    }

    private void OnMiniGameStart()
    {
        panelElements.SetActive(true);
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        ActionManager.BossMove?.Invoke();
        panelElements.SetActive(false);
    }
}
