using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Unit
{
    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        stateMachine.Run();
    }
    protected override void SetRange()
    {
        basicAttackRange = new Range(new int[3, 3]{
            {   0,  1,  0   },
            {   1,  1,  1   },
            {   0,  1,  0   }
        });
        basicAttackSplash = new Range(new int[1, 1] { { 1 } });
        skillRange = new Range(new int[7, 7]{
            {   0,0,0,1,0,0,0 },
            {   0,0,1,1,1,0,0 },
            {   0,1,1,1,1,1,0 },
            {   1,1,1,1,1,1,1 },
            {   0,1,1,1,1,1,0 },
            {   0,0,1,1,1,0,0 },
            {   0,0,0,1,0,0,0 }
        });
        skillSplash = new Range(new int[3, 3]{
            {   0,  1,  0   },
            {   1,  1,  1   },
            {   0,  1,  0   }
        });
    }
}
