using UnityEngine;

public class TMEndTurnBtn : MonoBehaviour
{
    TurnMgr _turnMgr;

    public void SetTMEndTurnBtn()
    {
        _turnMgr = FindObjectOfType<TurnMgr>();
    }
    public void OnClick_EndTurnBtn()
        => _turnMgr.NextTurn();
}
