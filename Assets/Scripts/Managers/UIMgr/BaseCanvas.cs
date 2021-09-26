using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
public class BaseCanvas : MonoBehaviour
{
    [System.Serializable] 
    public class UIDictionary : SerializableDictionaryBase<UIType, UIComponent> { }
    public UIDictionary _prefab_dictionary = new UIDictionary();
    
    protected UIDictionary _dictionary = new UIDictionary();
    protected Dictionary<Type, UIType> TypeToEnumConverter = new Dictionary<Type, UIType>(); 

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
        UIType ut = (UIType)Enum.Parse(typeof(UIType), typeof(T).ToString()); // 이렇게 변환해도 되긴 하는데, 이러면 Type명과 enum이 같아야 함

        if (!_dictionary[ut].gameObject.activeSelf && evenInactive) return null;

        Debug.Log((T)_dictionary[ut]);
;
        return (T)_dictionary[ut];
    }

    public void TurnOnUIComponent(UIType ut)
    {
        _dictionary[ut].gameObject.SetActive(true);
    }
    public void TurnOffUIComponent(UIType ut)
    {
        _dictionary[ut].gameObject.SetActive(false);
    }
    public void TurnOnUIs(List<UIType> l_ui)
    {
        foreach (var v in _dictionary)
        {
            if (l_ui.Contains(v.Key))
            {
                v.Value.gameObject.SetActive(true);
            }
        }
    }
    public void TurnOffUIs(List<UIType> l_ui)
    {
        foreach (var v in _dictionary)
        {
            if (l_ui.Contains(v.Key))
            {
                v.Value.gameObject.SetActive(false);
            }
        }
    }
    public void TurnOnAllUI()
    {
        foreach(var v in _dictionary)
        {
            v.Value.gameObject.SetActive(true);
        }
    }
    public void TurnOffAllUI()
    {
        foreach (var v in _dictionary)
        {
            v.Value.gameObject.SetActive(false);
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
