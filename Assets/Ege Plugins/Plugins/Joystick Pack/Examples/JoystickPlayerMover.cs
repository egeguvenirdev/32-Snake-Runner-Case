using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerMover : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float bodySpeed = 5;
    [SerializeField] private float localMoverSpeed = 5;
    [SerializeField] private float clampValue = 4.8f;
    [SerializeField] private int gap = 5;
    [SerializeField] private VariableJoystick variableJoystick;
    //[SerializeField] private AnimController animController;
    [SerializeField] private Transform localMover;
    [SerializeField] private Transform character;
    [SerializeField] private LayerMask mask;

    private List<Vector3> PositionsHistory = new List<Vector3>();
    private bool canMove;
    private Transform targetPos;

    public void Init()
    {
        canMove = true;
    }

    public void DeInit()
    {
        canMove = false;
    }


    private void Update()
    {
        if (!canMove) return;

        //targetPos = DetectEnemies();

        if (targetPos != null) character.LookAt(targetPos);

        if (Input.GetMouseButton(0) && variableJoystick.Direction != Vector2.zero)
        {
            OnPlayerMove();
            //animController.PlayRunAnimation(1);
            //lookat
            float angle = Vector3.Angle(new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical), Vector3.forward);
            if (variableJoystick.Horizontal < 0) angle *= -1;
            localMover.rotation = Quaternion.Euler(0f, angle, 0f);
            localMover.position += localMover.forward * localMoverSpeed * Time.deltaTime;

            Vector3 pos = localMover.localPosition;
            //pos.x = Mathf.Clamp(pos.x, -clampValue, clampValue);
            //pos.z = Mathf.Clamp(pos.z, -clampValue, 7);
            localMover.localPosition = pos;
            return;
        }
        //animController.PlayIdleAnimation(1);
    }

    private void OnPlayerMove()
    {
        if (Input.GetMouseButton(0))
        {
            PositionsHistory.Insert(0, localMover.position);
            Vector3 point = PositionsHistory[Mathf.Clamp(1 * gap, 0, PositionsHistory.Count - 1)];
            Vector3 moveDirection = point - character.transform.position;
            character.position += moveDirection * bodySpeed * Time.deltaTime;
        }
        int index = 0;

        if (PositionsHistory.Count > 1)
        {
            if (index <= PositionsHistory.Count)
            {
                Vector3 point = PositionsHistory[Mathf.Clamp(index * gap, 0, PositionsHistory.Count - 1)];

                // Move body towards the point along the snakes path
                Vector3 moveDirection = point - character.position;
                character.position += moveDirection * bodySpeed * Time.deltaTime;

                // Rotate body towards the point along the snakes path

                if (targetPos == null) character.LookAt(point);
                index++;
            }
        }
    }

    private Transform DetectEnemies()
    {
        var colliders = Physics.OverlapSphere(transform.position, 100f, mask);
        Transform target = null;
        float distance = 1000f;

        foreach (var collider in colliders)
        {
            if (target == null)
            {
                target = collider.transform;
                distance = (transform.position - collider.transform.position).sqrMagnitude;
            }

            if ((transform.position - collider.transform.position).sqrMagnitude < distance)
            {
                distance = (transform.position - collider.transform.position).sqrMagnitude;
                target = collider.transform;
            }
        }

        return target;
    }
}