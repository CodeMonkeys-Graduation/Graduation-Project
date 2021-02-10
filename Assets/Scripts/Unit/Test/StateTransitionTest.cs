//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class StateTransitionTest : MonoBehaviour
//{
//    public bool ToAttack;
//    public bool ToIdle;
//    public bool ToRun;
//    [SerializeField] Unit unit;

//    private void Update()
//    {
//        if(ToAttack)
//        {
//            unit.stateMachine.ChangeState(new UnitAttack(unit, null), StateMachine<Unit>.StateChangeMethod.PopNPush);
//            ToAttack = false;
//        }
//        if (ToIdle)
//        {
//            unit.stateMachine.ChangeState(new UnitIdle(unit), StateMachine<Unit>.StateChangeMethod.PopNPush);
//            ToIdle = false;
//        }
//        if (ToRun)
//        {
//            unit.stateMachine.ChangeState(new UnitRun(unit, null), StateMachine<Unit>.StateChangeMethod.PopNPush);
//            ToRun = false;
//        }
//    }
//}
