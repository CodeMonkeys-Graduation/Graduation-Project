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
        gameObject.SetActive(true);
        cg.alpha = 1f;
    }

    public new void TurnOffCanvas()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
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
