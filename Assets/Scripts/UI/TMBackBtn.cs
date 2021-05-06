using UnityEngine;

public class TMBackBtn : MonoBehaviour
{
    public void OnClick_BackBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev);
}
