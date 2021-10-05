using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextUIParam : UIParam
{
    public override UIType _uitype => UIType.DamageText;

    public DamageTextUIParam(Transform target, int dmgAmount)
    {
        damage = dmgAmount;
        damagedTarget = target;
    }

    public Transform damagedTarget;

    public int damage;
}



public class DamageText : PanelUIComponent
{
    [SerializeField] private TextMeshProUGUI text;

    private Transform target;

    private void Update()
    {
        if(gameObject.activeInHierarchy && target != null)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.position);
            transform.position = screenPoint + Vector3.up * 70f;
        }
    }

    public override void SetPanel(UIParam u = null)
    {
        DamageTextUIParam param = (DamageTextUIParam)u;

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(param.damagedTarget.position);
        transform.position = screenPoint + Vector3.up * 70f;
        target = param.damagedTarget;
        text.text = param.damage.ToString();

        gameObject.SetActive(true);

        StartCoroutine(UnsetPanelAfterSeconds(1f));
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator UnsetPanelAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        UIMgr.Instance.SetUIComponent<DamageText>(null, false);
    }
}
