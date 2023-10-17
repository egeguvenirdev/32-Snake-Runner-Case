using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlayButton : ButtonBase
{
    [SerializeField] private GameObject gridButttons;
    [SerializeField] private GameObject panelElements;

    public override void Init()
    {
        ActionManager.GameStart += OnGameStart;
    }

    public override void DeInit()
    {
        ActionManager.GameStart -= OnGameStart;
    }

    private void OnGameStart()
    {
        panelElements.SetActive(true);
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        gridButttons.SetActive(true);
        panelElements.SetActive(false);
    }
}
