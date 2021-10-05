using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ObserverPattern;

public enum UIType // UIType은 실제 클래스 이름과 동일해야합니다 뒤에다 추가하기... 세팅한 거 무너짐 ㅠㅠ
{
    #region BattleUI
    ActionBtn, ActionPanel, ActionPointPanel, BattleNextStateBtn, BattlePlayBtn,
    ItemPanel, ItemSlot, PopupPanel, StatusPanel, SummonPanel, TMBackBtn, TMEndTurnBtn, TurnPanel, TurnSlot, SummonBtn,
    DefeatPanel, VictoryPanel, DamageText,
    #endregion

    #region SelectUI
    SelectLoadSceneBtn, SelectCloseBtn, SelectSettingBtn, SelectUnitBtn, StageSelect, StageSelectPopup, StageSelectBtn, StagePlayerGold,
    #endregion

    #region MainUI
    DevSceneLoadBtn
    #endregion
}
public class UIComponent : MonoBehaviour // 모든 UI는 이것을 상속받아서 사용합니다
{
    [SerializeField] public UIType _uitype;
    //public void Awake()
    //{
    //    _uitype = UIMgr.TypeToUITypeConverter(GetType()); // UIType에 포함시키지 않은 경우 이 부분에 문제가 생깁니다
    //}
}
public abstract class PanelUIComponent : UIComponent
{
    public abstract void SetPanel(UIParam u = null);
    public abstract void UnsetPanel();
}
public interface ISceneLoadBtn
{
    public void LoadScene();
}
public interface ISelectionPopup
{
    public void OnClickYes();
    public void OnClickNo();
    public void OnClickClose();
}
public interface IPopup
{
    public void OnClickOK();
    public void OnClickClose();
}