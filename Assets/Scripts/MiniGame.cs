using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    [SerializeField] private Transform ballMoveTransform;
    [SerializeField] private BossBase boss;

    public Transform GetBallMoveTransform
    {
        get => ballMoveTransform;
    }

    public BossBase GetBoss
    {
        get => boss;
    }

    public void Init()
    {
        boss.Init();
    }
}
