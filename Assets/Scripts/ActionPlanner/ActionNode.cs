using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode
{
    public ActionNode parent;
    public GameState gameState;
    public int score;

    public abstract bool Perform();

}
