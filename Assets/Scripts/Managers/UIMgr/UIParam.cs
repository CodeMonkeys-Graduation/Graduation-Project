using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

    public interface UIParam
    {

    }

    public class UIAction : UIParam
    {
        public List<Unit.ActionSlot> _actionSlots;
        public int _actionPointRemain;
        public Dictionary<ActionType, UnityAction> _btnEvents;

        public UIAction(List<Unit.ActionSlot> actionSlots, int actionPointRemain, Dictionary<ActionType, UnityAction> btnEvents)
        {
            _actionSlots = actionSlots;
            _actionPointRemain = actionPointRemain;
            _btnEvents = btnEvents;
        }
    }

    public class UIActionPoint : UIParam
    {
        public int _point;

        public UIActionPoint(int point) => _point = point;
    }

    public class UIItem : UIParam
    {
        public Dictionary<Item, int> _itemCounter;
        public Action<Item> _onClickItemSlot;
        public UIItem(Dictionary<Item, int> itemCounter, Action<Item> onClickItemSlot)
        {
            _itemCounter = itemCounter;
            _onClickItemSlot = onClickItemSlot;
        }
    }

    public class UIPopup : UIParam
    {
        public string _content;
        public Vector2 _pos;
        public UnityAction _yes;
        public UnityAction _no;

        public UIPopup(string content, Vector2 pos, UnityAction yes, UnityAction no)
        {
            _content = content;
            _pos = pos;
            _yes = yes;
            _no = no;
        }
    }

    public class UIStatus : UIParam
    {
        public Unit _u;

        public UIStatus(Unit u) => _u = u;
    }

    public class UISummon : UIParam
    {
        public Unit _unit;
        public bool _add;

        public UISummon(Unit unit, bool add)
        {
            _unit = unit;
            _add = add;
        }
    }

    public class UITurn : UIParam
    {
        
    }

