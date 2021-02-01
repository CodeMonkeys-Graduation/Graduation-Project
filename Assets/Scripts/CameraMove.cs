using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform target;
    public Vector3 offset;

    void Awake()
    {
        target = GameObject.Find("SwordMan").transform;
    }
    void Update()
    {
        transform.position = target.position + offset;
    }
    
}
