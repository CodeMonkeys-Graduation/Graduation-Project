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
    [SerializeField] Animator illustAnim;

    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI npcname;
    [SerializeField] TextMeshProUGUI context;

    void Awake()
    {
        unitDataContainer = UnitDataMgr.unitDataContainer;
    }

    public void SetTalk(int id, string context, int emotion)
    {
        dialogEffect.SetMsg(this.context, context);

        illustAnim.SetTrigger("isTalk");

        illust.sprite = unitDataContainer.GetSprite(id, emotion);
        npcname.text = unitDataContainer.GetName(id);
        
    }

}
