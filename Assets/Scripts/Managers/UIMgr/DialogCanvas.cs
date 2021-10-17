using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;

public class DialogCanvas : BaseCanvas
{
    [Header("Set in Editor")]
    [HideInInspector] public Animator dialogAnimator; // 알 수 있는 정보가 너무 한정되어있음...

    [System.Serializable] public class AnimatorDictionary : SerializableDictionaryBase<SceneMgr.Scene, RuntimeAnimatorController> { }
    [SerializeField] AnimatorDictionary dictionary = new AnimatorDictionary();

    public void Start()
    {
        DialogDataMgr.InitDialogData();

        dialogAnimator = GetComponent<Animator>();
        dialogAnimator.runtimeAnimatorController = dictionary[SceneMgr.Instance._currScene];

        _dictionary[UIType.DialogPopup].UnsetPanel();

        TurnOffCanvas();
    }

    public void SetTalk(TalkData talkData)
    {
        int emotion = DialogUnitDataMgr.dialogUnitDataContainer.GetEmotion(talkData.emotion);

        DialogEffect df = (DialogEffect)_dictionary[UIType.DialogEffect];
        df.SetMsg(talkData.dialogue, talkData.name, emotion);
    }

    public void SetSelection(SelectionData selectionData)
    {
        DialogPopup dp = (DialogPopup)_dictionary[UIType.DialogPopup];
        dp.SetPopup(selectionData);
    }

    public void OnClickNext()
    {
        DialogEffect df = (DialogEffect)_dictionary[UIType.DialogEffect];
        DialogPopup dp = (DialogPopup)_dictionary[UIType.DialogPopup];

        if (dp.gameObject.activeSelf) return;

        if (df.isTyping)
        {
            df.SetMsg("", "", 0);
        }
        else if(df.isEnded)
        {
            dialogAnimator.SetBool("isTalkEnd", true);
        }
    }

    public void Play()
    {
        TurnOnCanvas();
    }

    public void Pause()
    {
        TurnOffCanvas();
    }
}
