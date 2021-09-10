using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SingletonBehaviour<CameraMgr>
{
    [SerializeField] private float lerpTime = 1f; // lerpTime만에 Target으로 갑니다.

    [SerializeField] private float cameraMoveSpeed = 5f;

    private Func<bool> unsetCondition;
    
    private bool unsetWhenArrivedTrigger = false;

    private Vector3 offset;
   
    private Transform target;
    
    private float lerp = 0f;

    private Vector3 screenXInWorld;
    
    private Vector3 screenYInWorld;

    void Start()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        float height = Camera.main.transform.position.y;
        Vector3 posFromScreenCenter = ray.GetPoint(height / Mathf.Cos(Camera.main.transform.rotation.x));
        offset = Camera.main.transform.position - posFromScreenCenter;

        Cursor.lockState = CursorLockMode.Confined;

        screenXInWorld = Camera.main.transform.right;
        screenXInWorld.y = 0f;
        screenXInWorld.Normalize();
        screenYInWorld = Camera.main.transform.up;
        screenYInWorld.y = 0f;
        screenYInWorld.Normalize();
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        // NonTargeting
        if (target == null)
        {
            lerp = 0f;

            Vector2 direction = Vector2.zero;
            GetMousePositionIfEdge(ref direction);
            GeKeyInputWASD(ref direction); // 만약 WASD키가 눌리면 mouse는 override

            MoveCamera(direction);
        }

        // Targeting
        else
        {
            lerp = Mathf.Clamp((lerp + Time.deltaTime) / lerpTime, 0f, 1f);
            LerpToDesiredPos(target.position, lerp);

            if(CheckUnsettingTargetCondition(target.position))
            {
                unsetCondition = null;
                target = null;
            }
        }
    }

    private Vector2 GeKeyInputWASD(ref Vector2 direction)
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

    private bool CheckUnsettingTargetCondition(Vector3 targetPos)
    {
        // 도착하면 타겟을 해제하는 트리거가 set되어있고
        // 도착했으면 return true
        // 아직 도착하지 못했다면 return false
        if (unsetWhenArrivedTrigger)
        {
            Vector3 desiredCameraPos = targetPos + offset;
            if (Vector3.Distance(Camera.main.transform.position, desiredCameraPos) <= Mathf.Epsilon)
            {
                unsetWhenArrivedTrigger = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        // 따로 전달받은 unset predicate이 존재한다면 해당 predicate로 unset해야할지 판정.
        if(unsetCondition != null && unsetCondition.Invoke())
        {
            return true;
        }

        //전부 아니라면 unset은 안하는 것으로 return false
        return false;
    }

    private void LerpToDesiredPos(Vector3 targetPos, float lerp)
    {
        Vector3 desiredCameraPos = targetPos + offset;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, desiredCameraPos, lerp);
    }

    public void SetTarget(Unit unit, bool unsetWhenArrived = false, Func<bool> unsetCondition = null)
    {
        lerp = 0f;
        target = unit.transform;

        unsetWhenArrivedTrigger = unsetWhenArrived;

        if (unsetCondition != null)
            this.unsetCondition = unsetCondition;
    }

    public void UnsetTarget()
    {
        lerp = 0f;
        target = null;
    }

    public void MoveCamera(Vector2 screenDirection) // 화면을 기준으로 x,y
    {
        Vector3 worldDirection = screenDirection.x * screenXInWorld + screenDirection.y * screenYInWorld;
        Camera.main.transform.position += Time.deltaTime * cameraMoveSpeed * (worldDirection.normalized);
    }
}
