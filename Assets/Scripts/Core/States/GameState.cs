using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : State<GameMgr>
{
    public GameState(GameMgr owner) : base(owner)
    {
        
    }
}
