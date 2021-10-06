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
    [SerializeField] private GameObject textUIPrefab;

    private Dictionary<GameObject, Transform> textInstancesAndTargets = new Dictionary<GameObject, Transform>();

    private void Update()
    {
        if(textInstancesAndTargets.Count <= 0)
        {
            UIMgr.Instance.SetUIComponent<DamageText>(null, false);
        }
    }

    public override void SetPanel(UIParam u = null)
    {
        DamageTextUIParam param = (DamageTextUIParam)u;

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(param.damagedTarget.position);
        var instanceObject = Instantiate(textUIPrefab, screenPoint, Quaternion.identity, transform);
        textInstancesAndTargets.Add(instanceObject, param.damagedTarget);
        instanceObject.transform.position = screenPoint + Vector3.up * 70f;
        instanceObject.GetComponentInChildren<TextMeshProUGUI>().text = param.damage.ToString();

        gameObject.SetActive(true);

        StartCoroutine(UnsetInstanceAfterSeconds(instanceObject.gameObject, 1f));
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator UnsetInstanceAfterSeconds(GameObject go, float seconds)
    {
        float secondsPassed = 0f;
        while(seconds > secondsPassed)
        {
            yield return null;

            Vector3 screenPoint = Camera.main.WorldToScreenPoint(textInstancesAndTargets[go].position);
            go.transform.position = screenPoint + Vector3.up * 70f;

            secondsPassed += Time.deltaTime;
        }

        textInstancesAndTargets.Remove(go);
        Destroy(go);
    }
}
