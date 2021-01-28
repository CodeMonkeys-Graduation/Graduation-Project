using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    /// <summary>
    /// State에 Enter할때 한번 호출
    /// </summary>
    void Enter();

    /// <summary>
    /// owner의 Update에서 프레임마다 호출합니다.
    /// </summary>
    void Execute();

    /// <summary>
    /// State에서 Exit할때 한번 호출
    /// </summary>
    void Exit();

    /// <summary>
    /// owner의 enum StateType으로 정의한 값을 리턴하세요.
    /// </summary>
    int GetStateType();
}
