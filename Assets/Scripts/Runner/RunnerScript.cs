using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RunnerScript : MonoBehaviour
{
    [Header("Scripts and Transforms")]
    [SerializeField] private Transform model;
    [SerializeField] private Transform localMoverTarget;
    [SerializeField] private PlayerSwerve playerSwerve;

    [Header("Path Settings")]
    [SerializeField] private float clampLocalX = 2f;  

    [Header("Run Settings")]
    [SerializeField] private float runSpeed = 2;
    [SerializeField] private float localTargetswipeSpeed = 2f;
    [SerializeField] private float swipeLerpSpeed = 2f;
    [SerializeField] private float swipeRotateLerpSpeed = 2f;

    private Vector3 oldPosition;
    private bool canRun = false;
    private bool canSwerve = false;
    private bool canFollow = true;

    public void Init()
    {
        playerSwerve.OnSwerve += PlayerSwipe_OnSwerve;
        ActionManager.ManagerUpdate += FollowLocalMoverTarget;
        StartToRun(true);
    }

    public void DeInit()
    {
        playerSwerve.OnSwerve -= PlayerSwipe_OnSwerve;
        ActionManager.ManagerUpdate -= FollowLocalMoverTarget;
        StartToRun(false);
    }

    private void StartToRun(bool checkRun)
    {
        if (checkRun)
        {
            canRun = true;
            canSwerve = true;
            canFollow = true;
        }
        else
        {
            canRun = false;
            canSwerve = false;
            canFollow = false;
        }
    }

    private void PlayerSwipe_OnSwerve(float direction)
    {
        if (canSwerve)
        {
            localMoverTarget.localPosition += Vector3.right * direction * localTargetswipeSpeed * Time.deltaTime;
            ClampLocalPosition();
        }
    }

    void ClampLocalPosition()
    {
        Vector3 pos = localMoverTarget.localPosition;
        pos.x = Mathf.Clamp(pos.x, -clampLocalX, clampLocalX);
        localMoverTarget.localPosition = pos;
    }

    void FollowLocalMoverTarget(float deltaTime)
    {
        if (canRun && canFollow)
        {
            if (canRun) localMoverTarget.Translate(transform.forward * deltaTime * runSpeed);

            /*Vector3 direction = localMoverTarget.localPosition - oldPosition;
            model.transform.forward = Vector3.Lerp(model.transform.forward, direction, swipeRotateLerpSpeed * Time.deltaTime);*/

            //swipe the object
            Vector3 nextPos = new Vector3(localMoverTarget.localPosition.x, model.localPosition.y, localMoverTarget.localPosition.z); ;
            model.localPosition = Vector3.Lerp(model.localPosition, nextPos, swipeLerpSpeed * deltaTime);
        }
    }
}
