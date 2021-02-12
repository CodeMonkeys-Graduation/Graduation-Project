using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    public T owner;

    public enum StateTransitionMethod { PopNPush, JustPush, ReturnToPrev, ClearNPush }

    private volatile Stack<State<T>> stateStack = new Stack<State<T>>();
    public int StackCount { get => stateStack.Count; }

    public StateMachine(State<T> initialState)
    {
        stateStack.Push(initialState);
        initialState.Enter();
    }

    public void Run()
    {
        stateStack.Peek().Execute();
    }


    public bool IsStateType(System.Type type) => stateStack.Peek().GetType() == type;

    public void ChangeState(State<T> nextState, StateTransitionMethod method)
    {
        switch (method)
        {
            case StateTransitionMethod.PopNPush:
                {
                    State<T> prevState = stateStack.Pop();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }
                
            case StateTransitionMethod.JustPush:
                {
                    State<T> prevState = stateStack.Peek();
                    prevState.Exit();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }

            case StateTransitionMethod.ReturnToPrev:
                {
                    State<T> currState = stateStack.Pop();
                    currState.Exit();
                    State<T> prevState = stateStack.Peek();
                    prevState.Enter();
                    break;
                }

            case StateTransitionMethod.ClearNPush:
                {
                    State<T> currState = stateStack.Pop();
                    currState.Exit();
                    stateStack.Clear();
                    stateStack.Push(nextState);
                    nextState.Enter();
                    break;
                }
        }

    }

}
