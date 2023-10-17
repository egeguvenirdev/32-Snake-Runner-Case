using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallCollectable : CollectableBase, ICollectable
{
    [Header("Components")]
    [SerializeField] private SphereCollider col;

    [Header("Ball Level")]
    [SerializeField] private int ballLevel = 1;

    [Header("Jump Settings")]
    [SerializeField] private float jumpDuration = 0.2f;
    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private int jumpCount = 1;

    public int GetBallLevel
    {
        get => ballLevel;
    }

    public override void OnCollected(Transform target)
    {
        col.enabled = false;
        transform.parent = target;
        GameManager.Haptic(0);
        transform.DOLocalJump(Vector3.zero, jumpPower, jumpCount, jumpDuration).OnComplete( () => 
        {
            ActionManager.BallCollected?.Invoke(transform);
        });
    }
}
