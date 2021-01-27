using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsuedoCube : Cube
{
    [SerializeField] [Range(0, 1)] float blinkIntensity = 0f;
    [SerializeField] bool blinking = false;
    private void Awake()
    {
        groundHeight = transform.position.y;
    }

    //private void Update()
    //{
    //    if (blinking)
    //        SetBlink(blinkIntensity);
    //    else
    //        StopBlink();
    //}



}
