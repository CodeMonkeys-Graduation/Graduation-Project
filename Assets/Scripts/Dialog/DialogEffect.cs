using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogEffect : MonoBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] GameObject EndCursor;
    [SerializeField] int charPerSeconds;

    [Header("Set In Runtime")]
    [HideInInspector] TextMeshProUGUI context;

    [HideInInspector] string targetMsg;
    [HideInInspector] float interval;
    [HideInInspector] int msgIndex;
    [HideInInspector] public bool isTyping = false;
    [HideInInspector] public bool isEnded = false;

    public void SetMsg(TextMeshProUGUI context, string msg)
    {
        this.context = context;

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
    
}
