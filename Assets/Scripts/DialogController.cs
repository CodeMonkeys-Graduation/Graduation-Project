using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogController : MonoBehaviour
{
    [HideInInspector] UnitDataContainer unitDataContainer;

    [Header("Set in Editor")]
    [SerializeField] Animator illustAnim;

    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI npcname;
    [SerializeField] TextMeshProUGUI context;

    public void SetTalk(int id, string context, int emotion)
    {
        illustAnim.SetTrigger("isTalk");

        illust.sprite = unitDataContainer.GetSprite(id, emotion);
        npcname.text = unitDataContainer.GetName(id);
        this.context.text = context;
    }

}
