using UnityEngine;

public class TMEndTurnBtn : MonoBehaviour
{
    [SerializeField] TurnMgr turnMgr;
    public void OnClick_EndTurnBtn()
    {
        turnMgr.NextTurn();
    }
}
