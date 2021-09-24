using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
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
        basicAttackRange = new Range(new int[9, 9]{
            {   0,0,0,0,1,0,0,0,0 },
            {   0,0,0,1,1,1,0,0,0 },
            {   0,0,1,1,1,1,1,0,0 },
            {   0,1,1,1,1,1,1,1,0 },
            {   1,1,1,1,1,1,1,1,1 },
            {   0,1,1,1,1,1,1,1,0 },
            {   0,0,1,1,1,1,1,0,0 },
            {   0,0,0,1,1,1,0,0,0 },
            {   0,0,0,0,1,0,0,0,0 },
        });
        basicAttackSplash = new Range(new int[1, 1] { 
            { 1 },
        });
    }
}
