using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public enum CameraState
    {
        NonTarget,
        UnitMove,
        UnitChange
    }
    public Vector3 offset;
    CameraState cameraState;
    Transform target;
    void Awake()
    {
        cameraState = CameraState.NonTarget;
    }
    void FixedUpdate()
    {
        if (cameraState == CameraState.NonTarget) // 타겟 미지정
        {
            return;
        }
        else if (cameraState == CameraState.UnitMove) // 타겟을 지정했고, 현재 타겟이 움직이는 중임
        {
            transform.position = target.position + offset;
        }
        else if (cameraState == CameraState.UnitChange) // 타겟을 바꾸었음
        {
            Vector3 unitPos = new Vector3(target.position.x, target.position.y, target.position.z);
            unitPos += offset;
            transform.position = Vector3.Lerp(transform.position, unitPos, Time.deltaTime * 2f);

            if (Mathf.Abs(transform.position.y - unitPos.y) <= Mathf.Epsilon)
            {
                cameraState = CameraState.UnitMove;
                Debug.Log("Camera Move Completed");
            }
        }
    }
    public void ChangeTarget(Unit unit)
    {
        cameraState = CameraState.UnitChange;
        target = unit.transform;
    }
}
