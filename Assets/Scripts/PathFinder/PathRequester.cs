using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequester : MonoBehaviour
{
    [SerializeField] PathFinder pathFinder;
    public enum RequesterState { Idle, Wait }
    public RequesterState rState = RequesterState.Idle;
    public void Request(Cube start, Cube destination, Action<List<Cube>> OnServe)
    {
        pathFinder.Request(this, new PathFinder.ReqInput(start, destination), OnServe);
    }
}
