using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogEffect : PanelUIComponent
{
    [Header("Set In Editor")]
    [SerializeField] GameObject EndCursor;
    [SerializeField] Animator illustAnim;
    [SerializeField] TextMeshProUGUI context;
    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI npcname;
    [SerializeField] int charPerSeconds;

    [Header("Set In Runtime")]
    [HideInInspector] string targetMsg;
    [HideInInspector] float interval;
    [HideInInspector] int msgIndex;
    [HideInInspector] public bool isTyping = false;
    [HideInInspector] public bool isEnded = false;

    public void SetMsg(string msg, string name, int emotion)
    {
        if (name != "")
        {
            illust.sprite = DialogUnitDataMgr.dialogUnitDataContainer.GetSprite(name, emotion);
            npcname.text = name;
        }

        if (isTyping)
        {
            context.text = targetMsg;
            CancelInvoke(); //재귀함수 정지
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart()
    {
        illustAnim.SetTrigger("isTalk");

        context.text = "";
        msgIndex = 0;

        EndCursor.SetActive(false);

        interval = 1.0f / charPerSeconds;

        isEnded = false;
        isTyping = true;
        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        if (context.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        context.text += targetMsg[msgIndex++];

        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isTyping = false;
        isEnded = true;
        EndCursor.SetActive(true);
    }

    public override void SetPanel(UIParam u = null)
    {
        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
