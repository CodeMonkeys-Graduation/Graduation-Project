using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using ObserverPattern;

public abstract class UIParam // Event성 파라미터를 넘길 필요없는 UI는 이 UIParam만 상속받을 것
{
    public abstract UIType _uitype
    {
        get;
    }
}

public class UIActionParam : UIParam, EventParam
{
    UIParam p;
    public List<Unit.ActionSlot> _actionSlots;
    public int _actionPointRemain;
    public Dictionary<ActionType, UnityAction> _btnEvents;
    public UIActionParam(List<Unit.ActionSlot> actionSlots, int actionPointRemain, Dictionary<ActionType, UnityAction> btnEvents)
    {
        _actionSlots = actionSlots;
        _actionPointRemain = actionPointRemain;
        _btnEvents = btnEvents;
    }

    public override UIType _uitype
    {
        get { return UIType.ActionPanel; }
    }
}

public class UIActionPointParam : UIParam, EventParam
{
    public int _point;
    public UIActionPointParam(int point) => _point = point;
    public override UIType _uitype
    {
        get { return UIType.ActionPointPanel; }
    }
}

public class UIItemParam : UIParam, EventParam
{
    public Dictionary<Item, int> _itemCounter;
    public Action<Item> _onClickItemSlot;
    public UIItemParam(Dictionary<Item, int> itemCounter, Action<Item> onClickItemSlot)
    {
        _itemCounter = itemCounter;
        _onClickItemSlot = onClickItemSlot;
    }
    public override UIType _uitype
    {
        get { return UIType.ItemPanel; }
    }
}

public class UIPopupParam : UIParam, EventParam
{
    public string _content;
    public Vector3 _pos;
    public UnityAction _yes;
    public UnityAction _no;

    public UIPopupParam(string content, Vector3 pos, UnityAction yes, UnityAction no)
    {
        _content = content;
        _pos = pos;
        _yes = yes;
        _no = no;
    }
    public override UIType _uitype
    {
        get { return UIType.PopupPanel; }
    }
}

public class UIStatusParam : UIParam, EventParam
{
    public Unit _u;

    public UIStatusParam(Unit u) => _u = u;

    public override UIType _uitype
    {
        get { return UIType.StatusPanel; }
    }
}

public class UISummonParam : UIParam, EventParam
{
    public List<Unit> _units;
    public bool _add;
    public UISummonParam(List<Unit> units, bool add)
    {
        _units = units;
        _add = add;
    }
    public override UIType _uitype
    {
        get { return UIType.SummonPanel; }
    }
}

public class UIStageSelectPopupParam : UIParam
{
    public SceneMgr.Scene nextScene;
    public StageData nextStageData;
    public UIStageSelectPopupParam(StageData stageData, SceneMgr.Scene scene)
    {
        nextStageData = stageData;
        nextScene = scene;
    }

    public override UIType _uitype
    {
        get { return UIType.StageSelectPopup; }
    }
}

