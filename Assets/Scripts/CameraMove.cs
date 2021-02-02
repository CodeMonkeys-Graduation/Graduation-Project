using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public enum CameraState
    {
        NonTargeting,
        Targeting
    }
    CameraState cameraState; // 디버그용

    [SerializeField] Vector3 offset;
    Transform target;
    [SerializeField] float lerpTime = 1f; // lerpTime만에 Target으로 갑니다.
    float lerp = 0f;
    void Awake()
    {
        cameraState = CameraState.NonTargeting;
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
        transform.position = Vector3.Lerp(transform.position, desiredCameraPos, lerp);
    }

    public void SetTarget(Unit unit) => target = unit.transform;
    public void UnsetTarget(Unit unit) => target = null;
}
