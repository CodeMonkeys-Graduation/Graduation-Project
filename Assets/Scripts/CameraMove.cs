using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private static CameraMove instance;
    public static CameraMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraMove>();
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
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
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        cameraState = CameraState.NonTarget;
    }
    void FixedUpdate()
    {
        if (cameraState == CameraState.NonTarget)
        {
            return;
        }
        else if (cameraState == CameraState.UnitMove)
        {
            transform.position = target.position + offset;
        }
        else if (cameraState == CameraState.UnitChange)
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