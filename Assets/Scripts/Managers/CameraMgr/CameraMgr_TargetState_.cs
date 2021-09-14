using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr_TargetState_ : State<CameraMgr>
{
    private float time = 0f;

    private Vector3 startPosition;

    public CameraMgr_TargetState_(CameraMgr owner)
        : base(owner)
    {
    }

    public override void Enter()
    {
        startPosition = Camera.main.transform.position;
    }

    public override void Execute()
    {
        time += Time.deltaTime;
        owner.ProcessTargetFollowing(startPosition, time);
    }

    public override void Exit()
    {
    }
}
