using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float playerFollowSpeed = 0.125f;
    [SerializeField] private float clampLocalX = 1.5f;

    [Header("Following Settings")]
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 playerFollowPos;
    [SerializeField] private Vector3 gridFollowPos;
    [SerializeField] private Vector3 miniGameFollowPos;
    [Space]
    [SerializeField] private Vector3 playerFollowRot;
    [SerializeField] private Vector3 gridFollowRot;
    [SerializeField] private Vector3 miniGameFollowRot;
    [Space]
    [SerializeField] private float camMoveDuration;

    private PlayerManager playerManager;
    private Transform player;

    public float GetCamMoveDuration
    {
        get => camMoveDuration;
    }

    public void Init()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        SetCamPosToGrid();
        transform.position = Vector3.zero;
        ActionManager.MiniGameStarting += OnMiniGameStart;
    }

    public void DeInit()
    {
        playerManager = null;
        player = null;
        ActionManager.MiniGameStarting -= OnMiniGameStart;
    }

    public void SetCamPosToGrid()
    {
        cam.DOLocalRotate(gridFollowRot, 0);
        cam.DOLocalMove(gridFollowPos, 0);
    }

    public void SetCamPosToPlayer()
    {
        cam.DOLocalRotate(playerFollowRot, camMoveDuration);
        cam.DOLocalMove(playerFollowPos, camMoveDuration).OnComplete(() =>
        {
            player = playerManager.GetChararacterTransform;
        });
    }

    private void OnMiniGameStart()
    {
        transform.DOMoveX(0, camMoveDuration);
        cam.DOLocalRotate(miniGameFollowRot, camMoveDuration);
        cam.DOLocalMove(miniGameFollowPos, camMoveDuration).OnComplete(() =>
        {
            player = playerManager.GetChararacterTransform;
        });

        playerManager = null;
        player = null;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            Vector3 pos = Vector3.Lerp(transform.position, targetPosition, playerFollowSpeed);
            pos.z = targetPosition.z;

            //pos.x = Mathf.Clamp(targetPosition.x, -clampLocalX, clampLocalX);
            transform.position = pos;

        }
    }
}