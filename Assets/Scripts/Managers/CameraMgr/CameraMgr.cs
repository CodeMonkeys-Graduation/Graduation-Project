using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : SingletonBehaviour<CameraMgr>
{
    public enum CameraState
    {
        NonTargeting,
        Targeting
    }
    public CameraState cameraState; // 디버그용

    public Camera _camera;

    [SerializeField] float lerpTime = 1f; // lerpTime만에 Target으로 갑니다.

    private Vector3 offset;
    Transform target;
    private float lerp = 0f;
    void Start()
    {
        _camera = Camera.main;
        cameraState = CameraState.NonTargeting;
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
            cameraState = CameraState.NonTargeting;
            lerp = 0f;
            return;
        }

        // Targeting
        cameraState = CameraState.Targeting;
        lerp = Mathf.Clamp((lerp + Time.deltaTime) / lerpTime, 0f, 1f);
        LerpToDesiredPos(target.position, lerp);
    }

    private void LerpToDesiredPos(Vector3 targetPos, float lerp)
    {
        Vector3 desiredCameraPos = targetPos + offset;
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, desiredCameraPos, lerp);
    }

    public void SetTarget(Unit unit)
    {
        lerp = 0f;
        target = unit.transform;
    }
    public void UnsetTarget(Unit unit) => target = null;
}
