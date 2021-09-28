using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float xMin, xMax, zMin, zMax;
    void Start()
    {
        InitData();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Update()
    {
        _stateMachine.Run();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitData();
    }

    private void InitData()
    {
        _stateMachine = new StateMachine<CameraMgr>(new CameraMgr_NormalState_(this));
        _cameraDirection = Camera.main.transform.rotation * Vector3.forward;
        CalculateInitialOffsetFromYZero();
        CalculateScreenDirectionInWorld();
        _desiredPositionWithoutHeight = Camera.main.transform.position;
        Cursor.lockState = CursorLockMode.Confined;

        foreach (var cube in FindObjectsOfType<Cube>())
        {
            xMin = Mathf.Min(cube.transform.position.x, xMin);
            xMax = Mathf.Max(cube.transform.position.x, xMax);
            zMin = Mathf.Min(cube.transform.position.z, zMin);
            zMax = Mathf.Max(cube.transform.position.z, zMax);
        }
    }

    private void OnDrawGizmos()
    {
        // b *--------------------------------* d
        //   |                                |
        //   |                                |
        //   |                                |
        //   |                                |
        // a *--------------------------------* c

        Ray aRay = new Ray(new Vector3(xMin, 0f, zMin), -Camera.main.transform.forward);
        Ray bRay = new Ray(new Vector3(xMin, 0f, zMax), -Camera.main.transform.forward);
        Ray cRay = new Ray(new Vector3(xMax, 0f, zMin), -Camera.main.transform.forward);
        Ray dRay = new Ray(new Vector3(xMax, 0f, zMax), -Camera.main.transform.forward);

        float cameraHeight = Camera.main.transform.position.y;
        float xAngle = Quaternion.FromToRotation(-Camera.main.transform.forward, Vector3.up).eulerAngles.x;
        Vector3 aRayCameraHeight = aRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));
        Vector3 bRayCameraHeight = bRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));
        Vector3 cRayCameraHeight = cRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));
        Vector3 dRayCameraHeight = dRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));

        Bounds bounds = new Bounds((aRayCameraHeight + bRayCameraHeight + cRayCameraHeight + dRayCameraHeight) / 4f,
            new Vector3(Mathf.Abs(xMax - xMin), 1f, Mathf.Abs(zMax - zMin)));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    private Vector3 ClampCameraPositionInConfinement(Vector3 nextPosition)
    {
        Ray aRay = new Ray(new Vector3(xMin, 0f, zMin), -Camera.main.transform.forward);
        Ray bRay = new Ray(new Vector3(xMin, 0f, zMax), -Camera.main.transform.forward);
        Ray cRay = new Ray(new Vector3(xMax, 0f, zMin), -Camera.main.transform.forward);
        Ray dRay = new Ray(new Vector3(xMax, 0f, zMax), -Camera.main.transform.forward);

        float cameraHeight = nextPosition.y;
        float xAngle = Quaternion.FromToRotation(-Camera.main.transform.forward, Vector3.up).eulerAngles.x;
        Vector3 aRayCameraHeight = aRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));
        Vector3 bRayCameraHeight = bRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));
        Vector3 cRayCameraHeight = cRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));
        Vector3 dRayCameraHeight = dRay.GetPoint(cameraHeight / Mathf.Cos(Mathf.Deg2Rad * xAngle));

        Bounds bounds = new Bounds((aRayCameraHeight + bRayCameraHeight + cRayCameraHeight + dRayCameraHeight) / 4f,
            new Vector3(Mathf.Abs(xMax - xMin), 1f, Mathf.Abs(zMax - zMin)));

        Vector3 result = nextPosition;
        if (!bounds.Contains(result))
        {
            result = bounds.ClosestPoint(result);
        }

        return result;
    }

    public void ProcessCameraMove()
    {
        Vector2 direction = Vector2.zero;
        HandleKeyInputWASD(ref direction);
        GetMousePositionIfEdge(ref direction);
        Vector3 directionInWorld = direction.x * _screenXInWorld + direction.y * _screenYInWorld;
        _desiredPositionWithoutHeight = ClampCameraPositionInConfinement(_desiredPositionWithoutHeight + _cameraMoveSpeed * Time.deltaTime * (directionInWorld.normalized));

        HandleMouseScroll();

        Camera.main.transform.position = DesiredPosition;
    }

    private void HandleMouseScroll()
    {
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Cube")))
        {
            float distanceFromCube = Vector3.Distance(hit.point, Camera.main.transform.position);
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            _cameraHeightOffset = Mathf.Clamp(
                _cameraHeightOffset - _cameraHeightSpeed * scrollDelta * Time.deltaTime,
                -Mathf.Abs(distanceFromCube - _cameraMaxHeightOffset),
                0f);
        }
        
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

    public void ShakeCamera(float duration, float strength, int vibrato)
    {
        Camera.main.DOShakePosition(duration, strength, vibrato);
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
        Vector3 posFromScreenCenter = ray.GetPoint(height / Mathf.Cos(Mathf.Deg2Rad * xAngle));
        _offsetFromGround = (Camera.main.transform.position - posFromScreenCenter);
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

        _stateMachine.ChangeState(new CameraMgr_NormalState_(this), StateMachine<CameraMgr>.StateTransitionMethod.PopNPush);
    }

}
