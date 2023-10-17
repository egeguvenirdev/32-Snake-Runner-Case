using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private bool canUpdateGame;

    public void Init()
    {
        canUpdateGame = true;
    }

    public void DeInit()
    {
        canUpdateGame = false;
    }

    void Update()
    {
        if(canUpdateGame) ActionManager.ManagerUpdate?.Invoke(Time.deltaTime);
    }
}
