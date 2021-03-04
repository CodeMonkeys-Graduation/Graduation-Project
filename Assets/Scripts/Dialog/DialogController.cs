using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogController : MonoBehaviour
{
    UnitDataContainer unitDataContainer;

    [Header("Set in Editor")]
    [SerializeField] DialogEffect dialogEffect;
    [SerializeField] DialogPopup dialogPopup;
    [SerializeField] Animator illustAnim;

    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI npcname;
    [SerializeField] TextMeshProUGUI context;

    void Awake()
    {
        unitDataContainer = UnitDataMgr.unitDataContainer;
    }

    public void SetTalk(TalkData talkData, SelectionData selectionData)
    {
        illustAnim.SetTrigger("isTalk");

        illust.sprite = unitDataContainer.GetSprite(talkData.id, talkData.emotion);
        npcname.text = unitDataContainer.GetName(talkData.id);
        dialogEffect.SetMsg(this.context, talkData.context);
        
        if (talkData.hasSelection && !dialogEffect.isTyping) dialogPopup.SetPopup(selectionData);

    }

}
