using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSettingBtn : UIComponent
{
    SelectSetting settingPopup = null;

    private void Start()
    {
        settingPopup = FindObjectOfType<SelectSetting>();
        settingPopup.UnsetPanel();
    }

    public void OnClickSetting()
    {
        if (settingPopup.gameObject.activeSelf)
        {
            settingPopup.UnsetPanel();
        }
        else
        {
            settingPopup.SetPanel();
        }
    }

}
