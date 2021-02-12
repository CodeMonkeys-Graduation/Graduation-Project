using UnityEngine;

public class TMBackBtn : MonoBehaviour
{
    [SerializeField] TurnMgr turnMgr;
    public void OnClick_BackBtn()
    {
        Debug.Log("OnClick_BackBtn");
        turnMgr.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev);
    }
}
