using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogController : MonoBehaviour
{
    UnitDataContainer unitDataContainer;

    [Header("Set in Editor")]
    [SerializeField] public Animator dialogAnimator;
    [SerializeField] public DialogEffect dialogEffect;
    [SerializeField] public DialogPopup dialogPopup;

    [SerializeField] Animator illustAnim;
    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI npcname;
    [SerializeField] TextMeshProUGUI context;
    [SerializeField] int stageProgress;

    void Awake()
    {
        dialogAnimator.runtimeAnimatorController  = Resources.Load("DialogSystem-" + stageProgress) as RuntimeAnimatorController;
        unitDataContainer = UnitDataMgr.unitDataContainer;
        InitDialogUI();
    }

    void InitDialogUI()
    {
        dialogPopup.UnsetPopup();
    }

    public void SetTalk(TalkData talkData)
    {
        illustAnim.SetTrigger("isTalk");

        illust.sprite = unitDataContainer.GetSprite(talkData.id, talkData.emotion);
        npcname.text = unitDataContainer.GetName(talkData.id);
        dialogEffect.SetMsg(this.context, talkData.context);
    }

    public void SetSelection(SelectionData selectionData)
    {
        dialogPopup.SetPopup(dialogAnimator, selectionData);
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
            dialogAnimator.SetTrigger("Next");
        }
        // 마지막 대화라면 씬 이동
    }

    public void OnClickSkip()
    {
        // anystate를 이용해 종료
        // 마지막 대화라면 씬 이동
    }

}
