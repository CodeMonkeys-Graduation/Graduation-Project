using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RotaryHeart.Lib.SerializableDictionary;

public class DialogController : SingletonBehaviour<DialogController>
{
    [Header("Set in Editor")]
    [SerializeField] public Animator dialogAnimator; // 알 수 있는 정보가 너무 한정되어있음...
    [SerializeField] public DialogEffect dialogEffect;
    [SerializeField] public DialogPopup dialogPopup;

    [SerializeField] Animator illustAnim;
    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI npcname;
    [SerializeField] TextMeshProUGUI context;

    [System.Serializable] public class AnimatorDictionary : SerializableDictionaryBase<SceneMgr.Scene, RuntimeAnimatorController> { }
    [SerializeField] AnimatorDictionary dictionary = new AnimatorDictionary();

    public void Init(SceneMgr.Scene scene)
    {
        DialogDataMgr.InitDialogData();
        dialogAnimator.runtimeAnimatorController = dictionary[scene];
        dialogPopup.UnsetPopup();
    }

    public void SetTalk(TalkData talkData)
    {
        illustAnim.SetTrigger("isTalk");

        int emotion = DialogUnitDataMgr.dialogUnitDataContainer.GetEmotion(talkData.emotion);

        illust.sprite = DialogUnitDataMgr.dialogUnitDataContainer.GetSprite(talkData.name, emotion);
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

    public void Play()
    {
        gameObject.SetActive(true);
    }

    public void Pause()
    {
        gameObject.SetActive(false);
    }
}
