using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadCanvas : BaseCanvas
{
    CanvasGroup cg;

    private new void Awake()
    {
        base.Awake();
        cg = GetComponent<CanvasGroup>();
    }

    public new void TurnOnCanvas()
    {
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        cg.alpha = 1f;

        float waitTime = 0.5f;

        while (waitTime > 0)
        {
            yield return 0;
            waitTime -= Time.deltaTime;
        }

        float timeElapsed = 0f;
        float fadeTime = 1.5f;

        while (timeElapsed < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeTime);
            yield return 0;
            timeElapsed += Time.deltaTime;
        }

        cg.alpha = 0f;
        Destroy(gameObject);
    }

}
