using UnityEngine;

public class TMEndTurnBtn : MonoBehaviour
{
    public void OnClick_EndTurnBtn()
        => TurnMgr.Instance.NextTurn();
}
