using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SingletonBehaviour<CameraMgr>
{
    public Camera _camera;

    [SerializeField] float lerpTime = 1f; // lerpTime만에 Target으로 갑니다.

    Func<bool> unsetCondition;
    bool unsetWhenArrivedTrigger = false;

    private Vector3 offset;
    public Transform target;
    private float lerp = 0f;
    void Start()
    {
        _camera = Camera.main;
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        float height = _camera.transform.position.y;
        Vector3 posFromScreenCenter = ray.GetPoint(height / Mathf.Cos(_camera.transform.rotation.x));
        offset = _camera.transform.position - posFromScreenCenter;
    }
    void LateUpdate()
    {
        // NonTargeting
        if (target == null)
        {
            lerp = 0f;
            return;
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

    private bool CheckUnsettingTargetCondition(Vector3 targetPos)
    {
        // 도착하면 타겟을 해제하는 트리거가 set되어있고
        // 도착했으면 return true
        // 아직 도착하지 못했다면 return false
        if (unsetWhenArrivedTrigger)
        {
            Vector3 desiredCameraPos = targetPos + offset;
            if (Vector3.Distance(_camera.transform.position, desiredCameraPos) <= Mathf.Epsilon)
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
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, desiredCameraPos, lerp);
    }

    public void SetTarget(Unit unit, bool unsetWhenArrived = false, Func<bool> unsetCondition = null)
    {
        lerp = 0f;
        target = unit.transform;

        unsetWhenArrivedTrigger = unsetWhenArrived;

        if (unsetCondition != null)
            this.unsetCondition = unsetCondition;
    }

    internal void UnsetTarget()
    {
        lerp = 0f;
        target = null;
    }
}
