using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMgr : SingletonBehaviour<CameraMgr>
{
    [Header("Set in Editor")]
    // lerpTime만에 Target으로 갑니다.
    [SerializeField] private float _lerpTime = 1f; 

    // 마우스나 WASD로 카메라를 움직이는 속도
    [SerializeField] private float _cameraMoveSpeed = 5f;

    // Camera가 최대한 줌인하여 _cameraMinHeightOffsetFronCube만큼 큐브로부터 가까워질 수 있다. 그 이하는 안된다.
    [SerializeField] private float _cameraMinHeightOffsetFromCube = 2f;

    // 마우스 스크롤으로 줌인하는 속도
    [SerializeField] private float _cameraHeightSpeed = 3f;

    // 게임 시작시, 카메라가 큐브로부터 떨어진 Offset
    // 이게 최대 카메라 줌아웃이 됩니다.
    [SerializeField /*DEBUG*/] private Vector3 _maxOffsetFromGround; // InitData

    // 게임 시작시, 카메라가 큐브로부터 떨어진 Offset
    // 이게 최대 카메라 줌인이 됩니다.
    [SerializeField /*DEBUG*/] private Vector3 _minOffsetFromGround; // InitData

    private Vector3 _currOffsetFromGround;

    // 레이캐스트를 쏴서 맞는 큐브의 Position을 기준으로
    // 얼마나의 offset을 떨어질지 정합니다.
    [SerializeField] private Vector3 _standardPosition; // InitData

    private Vector3 _screenXInWorld; // InitData

    private Vector3 _screenYInWorld; // InitData

    private StateMachine<CameraMgr> _stateMachine;


    /////////////////////////////////
    // Targeting
    [SerializeField]
    private Transform _target;

    private Func<bool> _unsetCondition;

    [SerializeField]
    private bool _unsetWhenArrivedTrigger = false;
    ////////////////////////////////

    [SerializeField]
    private float xMin, xMax, zMin, zMax, yMin, yMax;

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

        Cursor.lockState = CursorLockMode.Confined;

        foreach (var cube in FindObjectsOfType<Cube>())
        {
            xMin = Mathf.Min(cube.transform.position.x, xMin);
            xMax = Mathf.Max(cube.transform.position.x, xMax);
            zMin = Mathf.Min(cube.transform.position.z, zMin);
            zMax = Mathf.Max(cube.transform.position.z, zMax);
            yMin = Mathf.Min(cube.transform.position.y, yMin);
            yMax = Mathf.Max(cube.transform.position.y, yMax);
        }

        _screenXInWorld = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;
        _screenYInWorld = new Vector3(Camera.main.transform.up.x, 0f, Camera.main.transform.up.z).normalized;

        Ray cameraForwardRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(cameraForwardRay, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Cube")))
        {
            _standardPosition = hit.transform.GetComponent<Cube>().Platform.position;

            _maxOffsetFromGround = Camera.main.transform.position - _standardPosition;
            _minOffsetFromGround = Vector3.ClampMagnitude(_maxOffsetFromGround, _cameraMinHeightOffsetFromCube);
            _currOffsetFromGround = _maxOffsetFromGround;
        }
        else
        {
            Debug.Assert(false, "카메라의 중앙에 큐브가 있도록 시작해야 합니다.");
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
        _standardPosition += directionInWorld * _cameraMoveSpeed * Time.deltaTime;

        Ray cameraForwardRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(cameraForwardRay, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Cube")))
        {
            _standardPosition.y = hit.transform.GetComponent<Cube>().Platform.position.y;
        }

        HandleMouseScroll();

        Camera.main.transform.position = _standardPosition + _currOffsetFromGround;
    }

    private void HandleMouseScroll()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        float delta = _cameraHeightSpeed * scrollDelta * Time.deltaTime;
        Vector3 newOffset = _currOffsetFromGround + delta * Camera.main.transform.forward;
        if(newOffset.y < 0 || newOffset.magnitude < _minOffsetFromGround.magnitude)
        {
            _currOffsetFromGround = _minOffsetFromGround;
        }
        else
        {
            _currOffsetFromGround = Vector3.ClampMagnitude(newOffset, _maxOffsetFromGround.magnitude);
        }

    }

    public void ProcessTargetFollowing(Vector3 startPosition, float time)
    {
        HandleMouseScroll();

        float lerp = Mathf.Clamp(time / _lerpTime, 0f, 1f);

        _standardPosition = _target.position;

        Camera.main.transform.position = Vector3.Lerp(startPosition, _target.position + _currOffsetFromGround, lerp);

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
            return Vector3.Distance(Camera.main.transform.position, _standardPosition + _currOffsetFromGround) <= Mathf.Epsilon;
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
