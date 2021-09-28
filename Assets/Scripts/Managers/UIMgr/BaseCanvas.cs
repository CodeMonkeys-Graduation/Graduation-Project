using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using ObserverPattern;

public class BaseCanvas : MonoBehaviour
{
    [System.Serializable] 
    public class UIDictionary : SerializableDictionaryBase<UIType, UIComponent> { }
    public UIDictionary _prefab_dictionary = new UIDictionary();
    
    protected UIDictionary _dictionary = new UIDictionary();
    protected void Awake()
    {
        foreach (var u in _prefab_dictionary)
        {
            UIComponent ui = Instantiate(u.Value, transform);
            _dictionary.Add(ui._uitype, ui);
        }
    }

    public T GetUIComponent<T>(bool evenInactive = false) where T : UIComponent
    {
        UIType ut = UIMgr.TypeToUITypeConverter(typeof(T));

        if (!_dictionary[ut].gameObject.activeSelf && evenInactive) return null;

        return (T)_dictionary[ut];
    }

    public void TurnOnUIComponent(UIType ut, UIParam param = null)
    {
        _dictionary[ut].SetPanel(param);
    }

    public void TurnOffUIComponent(UIType ut)
    {
        _dictionary[ut].gameObject.SetActive(false);
    }

    public void TurnOffUIComponents(List<UIType> l_ui)
    {
        foreach (var v in _dictionary)
        {
            if (l_ui.Contains(v.Key))
            {
                v.Value.UnsetPanel();
            }
        }
    }

    public void TurnOffAllUI()
    {
        foreach (var v in _dictionary)
        {
            v.Value.UnsetPanel();
        }
    }
    public void TurnOnCanvas()
    {
        gameObject.SetActive(true);
    }

    public void TurnOffCanvas()
    {
        gameObject.SetActive(false);
    }
}
