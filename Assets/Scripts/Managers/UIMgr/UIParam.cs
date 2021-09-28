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

public class UIAction : UIParam, EventParam
{
    UIParam p;
    public List<Unit.ActionSlot> _actionSlots;
    public int _actionPointRemain;
    public Dictionary<ActionType, UnityAction> _btnEvents;
    public UIAction(List<Unit.ActionSlot> actionSlots, int actionPointRemain, Dictionary<ActionType, UnityAction> btnEvents)
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

public class UIActionPoint : UIParam, EventParam
{
    public int _point;
    public UIActionPoint(int point) => _point = point;
    public override UIType _uitype
    {
        get { return UIType.ActionPointPanel; }
    }
}

public class UIItem : UIParam, EventParam
{
    public Dictionary<Item, int> _itemCounter;
    public Action<Item> _onClickItemSlot;
    public UIItem(Dictionary<Item, int> itemCounter, Action<Item> onClickItemSlot)
    {
        _itemCounter = itemCounter;
        _onClickItemSlot = onClickItemSlot;
    }
    public override UIType _uitype
    {
        get { return UIType.ItemPanel; }
    }
}

public class UIPopup : UIParam, EventParam
{
    public string _content;
    public Vector3 _pos;
    public UnityAction _yes;
    public UnityAction _no;

    public UIPopup(string content, Vector3 pos, UnityAction yes, UnityAction no)
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

public class UIStatus : UIParam, EventParam
{
    public Unit _u;

    public UIStatus(Unit u) => _u = u;

    public override UIType _uitype
    {
        get { return UIType.StatusPanel; }
    }
}

public class UISummon : UIParam, EventParam
{
    public List<Unit> _units;
    public bool _add;
    public UISummon(List<Unit> units, bool add)
    {
        _units = units;
        _add = add;
    }
    public override UIType _uitype
    {
        get { return UIType.SummonPanel; }
    }
}


