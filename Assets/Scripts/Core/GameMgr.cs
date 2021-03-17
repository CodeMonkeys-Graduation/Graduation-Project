using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    [SerializeField] List<Unit> unitsToPosition;

    // StateMachine //

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Positioning
    // Enter : SummonUI들에 units들 적재 & Set(이건 SummonUI에 만들어야할듯) 호출
    // 
    // Exit: -

    // Battle
    // Enter : TurnMgr NextTurn호출
    // 
    // Exit: -
}
