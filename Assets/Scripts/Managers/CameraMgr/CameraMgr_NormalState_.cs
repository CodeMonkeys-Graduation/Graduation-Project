using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr_NormalState_ : State<CameraMgr>
{
    public CameraMgr_NormalState_(CameraMgr owner)
        : base(owner)
    {
    }

    public override void Enter()
    {
    }

    public override void Execute()
    {
        owner.ProcessCameraMove();
    }

    public override void Exit()
    {
    }
}
