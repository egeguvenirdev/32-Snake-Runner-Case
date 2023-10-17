using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PlayerSwerve : MonoBehaviour
{
    [SerializeField] private float _speedMultiplier = 1f;
    public event System.Action<float> OnSwerve;

    [Header("Controller")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float dirMultiplier = 10;

    private float multiplier = 1;
    private float dirMaxMagnitude = float.PositiveInfinity;
    private Vector2 deltaDir;
    private Vector2 joystickCenterPos;
    private float lastPos;

    private bool isControl = false;
    private Vector2 dir;
    private Vector2 dirOld;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            joystickCenterPos = (Vector2)Input.mousePosition;
            deltaDir = Vector2.zero;
            dirOld = Vector2.zero;
            isControl = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            joystickCenterPos = (Vector2)Input.mousePosition;
            deltaDir = Vector2.zero;
            dirOld = Vector2.zero;
            isControl = false;
        }

        if (isControl)
        {
            multiplier = dirMultiplier / Screen.width;
            dir = ((Vector2)Input.mousePosition - joystickCenterPos) * multiplier;
            float m = dir.magnitude;
            if (m > dirMaxMagnitude) dir = dir * dirMaxMagnitude / m;
            deltaDir = dir - dirOld;
            dirOld = dir;
            //lastPos += (deltaDir.x * moveSpeed * Time.deltaTime);
            OnSwerve?.Invoke(deltaDir.x * moveSpeed * Time.deltaTime);
        }
    }
}
