using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallBody : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerManager player;

    [Header("Snake Parts")]
    [SerializeField] private GameObject snakeHeadTail;

    [Header("Snake Properties")]
    [SerializeField] private float bodySpeed = 5;
    [SerializeField] private int gap = 5;

    [Header("Path End Settings")]
    [SerializeField] private float jumpDuration = 0.1f;
    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private float moveForwardDuration = 0.1f;

    // Lists
    [SerializeField] private List<Transform> balls = new List<Transform>();
    [SerializeField] private List<Vector3> snakeTrail = new List<Vector3>();

    public void Init()
    {
        ActionManager.ManagerUpdate += OnPlayerMove;
        ActionManager.BallCollected += OnBallCollected;
    }

    public void DeInit()
    {
        ActionManager.ManagerUpdate -= OnPlayerMove;
        ActionManager.BallCollected -= OnBallCollected;
        StartCoroutine(OnPathEnd());
    }

    private void OnPlayerMove(float deltaTime)
    {
        snakeTrail.Insert(0, snakeHeadTail.transform.position + new Vector3(0, 0, 0.85f));

        int index = 0;
        if (snakeTrail.Count > 1)
        {
            foreach (var ball in balls)
            {
                if (index <= snakeTrail.Count)
                {
                    Vector3 point = snakeTrail[Mathf.Clamp(index * gap, 0, snakeTrail.Count - 1)];

                    // Move body towards the point along the snakes path
                    Vector3 moveDirection = point - ball.transform.position;
                    ball.transform.position += moveDirection * bodySpeed * deltaTime;

                    // Rotate body towards the point along the snakes path
                    ball.transform.LookAt(point);

                    index++;
                }
            }
        }
    }

    public void OnBallCollected(Transform newBall)
    {
        newBall.parent = transform;
        if (balls.Count == 0)
        {
            newBall.position = snakeHeadTail.transform.position;
        }
        else
        {
            newBall.position = balls[balls.Count - 1].transform.position;
        }
        balls.Add(newBall);
    }

    public IEnumerator OnPathEnd()
    {
        while (balls.Count >= 2)
        {
            //move
            balls[1].DOJump(balls[0].position, jumpPower, 1, jumpDuration);

            if (balls.Count >= 3)
            {
                for (int i = balls.Count - 1; i >= 2; i--)
                {
                    balls[i].DOMove(balls[i - 1].position, jumpDuration);
                }
            }

            yield return new WaitForSeconds(jumpDuration);

            //remove
            if (balls.Count >= 2)
            {
                player.OnBallAdded(balls[1].GetComponent<BallCollectable>().GetBallLevel, true);
                balls[1].gameObject.SetActive(false);
                balls.RemoveAt(1);
            }

            if (balls.Count == 1)
            {
                ActionManager.MiniGameStarting?.Invoke();
            }

        }
        if (balls.Count == 1)
        {
            ActionManager.MiniGameStarting?.Invoke();
        }
    }
}