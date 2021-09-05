using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using ObserverPattern;

    public class UIAction : EventParam
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

    public class UIActionPoint : EventParam
{
        public int _point;

        public UIActionPoint(int point) => _point = point;
    }

    public class UIItem : EventParam
{
        public Dictionary<Item, int> _itemCounter;
        public Action<Item> _onClickItemSlot;
        public UIItem(Dictionary<Item, int> itemCounter, Action<Item> onClickItemSlot)
        {
            _itemCounter = itemCounter;
            _onClickItemSlot = onClickItemSlot;
        }
    }

    public class UIPopup : EventParam
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
    }

    public class UIStatus : EventParam
{
        public Unit _u;

        public UIStatus(Unit u) => _u = u;
    }

    public class UISummon : EventParam
{
        public List<Unit> _units;
        public bool _add;

        public UISummon(List<Unit> units, bool add)
        {
            _units = units;
            _add = add;
        }
    }

    public class UITurn : EventParam
{
        
    }

