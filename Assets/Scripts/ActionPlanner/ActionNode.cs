using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode
{
    public ActionNode _parent;
    public APGameState _gameState;
    public int _score;

    public abstract bool Perform();
    public abstract bool Simulate();
}

public class ActionNode_Move : ActionNode
{
    public ActionNode_Move(APGameState gameState)
    {
        _gameState = gameState.Clone();
    }
    public override bool Perform()
    {
        return true;
    }

    public override bool Simulate()
    {
        return true;
    }
}

