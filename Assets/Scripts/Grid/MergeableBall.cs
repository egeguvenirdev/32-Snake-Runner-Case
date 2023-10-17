using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MergeableBall : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private ParticleSystem mergeParticle;
    [SerializeField] private float bodyPartPlacementSpeed;
    [SerializeField] private int ballLevel = 1;
    private int maksBallLevel = 5;

    //Movement
    public bool isPlaced = true;
    protected bool isDragging = false;
    protected Vector3 mouseFirstPos;
    protected Vector3 mouseSecondPos;
    protected Camera mainCam;
    protected GameObject target;

    protected ObjectPooler pooler;
    [SerializeField] protected Collider col1;

    public int GetBallLevel
    {
        get => ballLevel;
    }

    public void Init()
    {
        isPlaced = true;
        mainCam = Camera.main;
        mergeParticle.Play();
    }

    private void OnMouseDown()
    {
        if (isPlaced)
        {
            mouseFirstPos = Input.mousePosition;
        }
    }

    private void OnMouseDrag()
    {
        mouseSecondPos = Input.mousePosition;

        float tempDistance = Vector2.Distance(mouseFirstPos, mouseSecondPos);

        if (isPlaced && tempDistance > 50 && tempDistance < 100 /*&& Input.touchCount == 1*/)
        {
            isPlaced = false;
        }

        if (!isPlaced)
        {
            FollowTheMouse();
        }
    }

    protected virtual void OnMouseUp()
    {
        if (!isPlaced)
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 0f);
            GameManager.Haptic(0);
            isPlaced = true;
            isDragging = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f, collisionLayer);

            if (colliders.Length == 2)
            {
                if (maksBallLevel == ballLevel)
                {
                    ActionManager.FindEmptyGrid?.Invoke(gameObject);
                    return;
                }
                MergeableBall otherPart = colliders[1].GetComponent<MergeableBall>();

                if (otherPart == this)
                {
                    otherPart = colliders[0].GetComponent<MergeableBall>();
                }

                bool otherIsPlaced = otherPart.isPlaced;
                int otherRocketNumber = otherPart.GetBallLevel;

                if (ballLevel == otherRocketNumber && otherIsPlaced)
                {
                    if (pooler == null) pooler = ObjectPooler.Instance;
                    MergeableBall newBall = pooler.GetPooledBall(ballLevel + 1);
                    newBall.gameObject.SetActive(true);
                    newBall.Init();
                    newBall.transform.position = otherPart.transform.position;
                    newBall.transform.rotation = otherPart.transform.rotation;
                    PlayDoPunch(newBall.transform);

                    colliders[0].transform.localPosition = Vector3.up * 100;
                    colliders[0].gameObject.SetActive(false);
                    colliders[1].transform.localPosition = Vector3.up * 100;
                    colliders[1].gameObject.SetActive(false);

                    ActionManager.MergeColorCheck?.Invoke(false);
                }
                else
                {
                    RaycastHit hitGrid;

                    if (Physics.Raycast(transform.position, Vector3.down, out hitGrid, Mathf.Infinity, layerMask))
                    {
                        MergeGrid selecterGrid = hitGrid.transform.GetComponent<MergeGrid>();
                        if (selecterGrid != null && selecterGrid.GetIsSelecterGrid)
                        {
                            gameObject.SetActive(false);
                        }
                    }
                }
            }

            target = null;
            target = ThrowRaycast();
            if (target == null)
            {
                ActionManager.FindEmptyGrid?.Invoke(gameObject);
                return;
            }

            MergeGrid targetGrid = target.GetComponent<MergeGrid>();

            if (targetGrid == null) return;

            if (!targetGrid.GetIsSelecterGrid)
            {
                MergeGrid grid = target.GetComponent<MergeGrid>();
                ActionManager.MergeColorCheck?.Invoke(false);
                if (grid.EmptyCheck)
                {
                    PlayDoPunch(transform);
                    transform.position = grid.transform.position;
                    return;
                }
                else
                {
                    ActionManager.FindEmptyGrid?.Invoke(gameObject);
                    return;
                }
            }

            else
            {
                MergeGrid selecterGrid = target.GetComponent<MergeGrid>();
                if (selecterGrid.GetIsSelecterGrid)
                {
                    ActionManager.BallSelect?.Invoke(ballLevel);
                    ActionManager.MergeColorCheck?.Invoke(false);
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private GameObject ThrowRaycast()
    {
        RaycastHit hitGrid;
        RaycastHit hitPlayer;

        if (Physics.Raycast(transform.position, Vector3.down, out hitGrid, Mathf.Infinity, layerMask))
        {
            return hitGrid.collider.gameObject;
        }

        if (Physics.Raycast(transform.position - new Vector3(0, 0, 5), Vector3.down, out hitPlayer, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.red);
            return hitPlayer.collider.gameObject;
        }

        return null;
    }

    private void FollowTheMouse()
    {
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0f);
        ActionManager.MergeColorCheck?.Invoke(true);
        isDragging = true;

        float z = transform.localPosition.z;
        Vector3 myScreenPos;
        Vector3 targetPoint;

        myScreenPos = mainCam.WorldToScreenPoint(transform.position);
        targetPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myScreenPos.z));
        transform.DOMoveY(0.1f, 0f);
        transform.position = targetPoint;
    }

    private void PlayDoPunch(Transform refObject)
    {
        refObject.DOScale(new Vector3(1f, 1f, 1f), 0f);
        refObject.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2);
    }
}