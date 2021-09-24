using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogController : SingletonBehaviour<DialogController>
{
    [Header("Set in Editor")]
    [SerializeField] public Animator dialogAnimator;
    [SerializeField] public DialogEffect dialogEffect;
    [SerializeField] public DialogPopup dialogPopup;

    [SerializeField] Animator illustAnim;
    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI npcname;
    [SerializeField] TextMeshProUGUI context;

    [SerializeField] public int stageProgress;

    private void Start()
    {
        DialogDataMgr.InitDialogData();
        dialogAnimator.runtimeAnimatorController  = Resources.Load("DialogSystem-" + stageProgress) as RuntimeAnimatorController;
        InitDialogUI();
    }

    void InitDialogUI()
    {
        dialogPopup.UnsetPopup();
    }

    public void SetTalk(TalkData talkData)
    {
        illustAnim.SetTrigger("isTalk");

        int emotion = UnitDataMgr.unitDataContainer.GetEmotion(talkData.emotion);

        illust.sprite = UnitDataMgr.unitDataContainer.GetSprite(talkData.name, emotion);
        npcname.text = talkData.name;
        dialogEffect.SetMsg(this.context, talkData.dialogue);
    }

    public void SetSelection(SelectionData selectionData)
    {
        dialogPopup.SetPopup(selectionData);
    }

    public void OnClickNext()
    {
        if (dialogPopup.gameObject.activeSelf) return;

        if (dialogEffect.isTyping)
        {
            dialogEffect.SetMsg(this.context, "");
        }
        else if(dialogEffect.isEnded)
        {
            dialogAnimator.SetBool("isTalkEnd", true);
        }
        
    }

}
