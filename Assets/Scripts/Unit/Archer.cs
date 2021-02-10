using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Run();
    }
    protected override void SetRange()
    {
        basicAttackRange = new Range(new int[7, 7]{
            {   0,0,0,1,0,0,0 },
            {   0,0,1,1,1,0,0 },
            {   0,1,1,1,1,1,0 },
            {   1,1,1,1,1,1,1 },
            {   0,1,1,1,1,1,0 },
            {   0,0,1,1,1,0,0 },
            {   0,0,0,1,0,0,0 }
        });
        basicAttackSplash = new Range(new int[1, 1] { { 1 } });
        skillRange = null;
        skillSplash = null;
    }
}
