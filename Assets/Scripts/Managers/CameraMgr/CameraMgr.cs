using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SingletonBehaviour<CameraMgr>
{
    [SerializeField] private float _lerpTime = 1f; // lerpTime만에 Target으로 갑니다.

    [SerializeField] private float _cameraMoveSpeed = 5f;

    [SerializeField] private float _cameraMaxHeightOffset = 10f;

    [SerializeField] private float _cameraHeightSpeed = 3f;

    [SerializeField] private Vector3 _offsetFromGround;

    private Vector3 _screenXInWorld;

    private Vector3 _screenYInWorld;

    [SerializeField /*DEBUG*/] private Vector3 _cameraDirection;

    private StateMachine<CameraMgr> _stateMachine;

    private Vector3 DesiredPosition { 
        get {
            return _desiredPositionWithoutHeight - _cameraDirection * _cameraHeightOffset; 
        } 
    }

    private Vector3 _desiredPositionWithoutHeight;

    private float _cameraHeightOffset = 0f;

    /////////////////////////////////
    // Targeting
    private Transform _target;

    private Func<bool> _unsetCondition;

    private bool _unsetWhenArrivedTrigger = false;

    void Start()
    {
        _stateMachine = new StateMachine<CameraMgr>(new CameraMgr_NormalState_(this));
        _cameraDirection = Camera.main.transform.rotation * Vector3.forward;
        CalculateInitialOffsetFromYZero();
        CalculateScreenDirectionInWorld();
        _desiredPositionWithoutHeight = Camera.main.transform.position;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        _stateMachine.Run();
    }

    public void ProcessCameraMove()
    {
        Vector2 direction = Vector2.zero;
        HandleKeyInputWASD(ref direction);
        GetMousePositionIfEdge(ref direction);
        Vector3 directionInWorld = direction.x * _screenXInWorld + direction.y * _screenYInWorld;
        _desiredPositionWithoutHeight += _cameraMoveSpeed * Time.deltaTime * (directionInWorld.normalized);

        HandleMouseScroll();

        Camera.main.transform.position = DesiredPosition;
    }

    private void HandleMouseScroll()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        _cameraHeightOffset = Mathf.Clamp(_cameraHeightOffset - _cameraHeightSpeed * scrollDelta * Time.deltaTime, -_cameraMaxHeightOffset, 0f);
    }

    public void ProcessTargetFollowing(Vector3 startPosition, float time)
    {
        HandleMouseScroll();

        float lerp = Mathf.Clamp(time / _lerpTime, 0f, 1f);
        _desiredPositionWithoutHeight = _target.position + _offsetFromGround;
        Camera.main.transform.position = Vector3.Lerp(startPosition, DesiredPosition, lerp);

        if (IsTargetFollowingDone())
            _stateMachine.ChangeState(new CameraMgr_NormalState_(this), StateMachine<CameraMgr>.StateTransitionMethod.PopNPush);
    }

    private bool IsTargetFollowingDone()
    {
        if(_unsetWhenArrivedTrigger)
        {
            return Vector3.Distance(Camera.main.transform.position, DesiredPosition) <= Mathf.Epsilon;
        }

        if(_unsetCondition != null && _unsetCondition.Invoke())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CalculateScreenDirectionInWorld()
    {
        _screenXInWorld = Camera.main.transform.right;
        _screenXInWorld.y = 0f;
        _screenXInWorld.Normalize();
        _screenYInWorld = Camera.main.transform.up;
        _screenYInWorld.y = 0f;
        _screenYInWorld.Normalize();
    }

    private void CalculateInitialOffsetFromYZero()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        float height = Camera.main.transform.position.y;
        float xAngle = Quaternion.FromToRotation(Camera.main.transform.forward, Vector3.down).eulerAngles.x;
        Vector3 posFromScreenCenter = ray.GetPoint(height / Mathf.Cos(xAngle));
        _offsetFromGround = (Camera.main.transform.position - posFromScreenCenter) / 2f;
    }

    private Vector2 HandleKeyInputWASD(ref Vector2 direction)
    {
        // Camera Move by Mouse
        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1f;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1f;
        }

        return direction;
    }

    private Vector2 GetMousePositionIfEdge(ref Vector2 direction)
    {
        // Camera Move by Mouse
        if (IsMousePositionLeftMost())
        {
            direction.x = -1f;
        }
        else if (IsMousePositionRightMost())
        {
            direction.x = 1f;
        }
        if (IsMousePositionBottomMost())
        {
            direction.y = -1f;
        }
        else if (IsMousePositionTopMost())
        {
            direction.y = 1f;
        }

        return direction;
    }

    private bool IsMousePositionLeftMost() => Mathf.Abs(Input.mousePosition.x - 0.0f) < (Screen.width / 100f);
    private bool IsMousePositionBottomMost() => Mathf.Abs(Input.mousePosition.y - 0.0f) < (Screen.height / 100f);
    private bool IsMousePositionRightMost() => Mathf.Abs(Input.mousePosition.x - Screen.width) < (Screen.width / 100f);
    private bool IsMousePositionTopMost() => Mathf.Abs(Input.mousePosition.y - Screen.height) < (Screen.height / 100f);

    public void SetTarget(Unit unit, bool unsetWhenArrived = false, Func<bool> unsetCondition = null)
    {
        _target = unit.transform;
        _unsetWhenArrivedTrigger = unsetWhenArrived;
        _unsetCondition = unsetCondition;

        _stateMachine.ChangeState(new CameraMgr_TargetState_(this), StateMachine<CameraMgr>.StateTransitionMethod.PopNPush);
    }

    public void UnsetTarget()
    {
        _target = null;
    }

}
