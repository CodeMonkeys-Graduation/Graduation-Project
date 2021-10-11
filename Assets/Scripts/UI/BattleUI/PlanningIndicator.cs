using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningIndicatorUIParam : UIParam
{
    public Unit planningUnit;

    public override UIType _uitype => UIType.PlanningIndicator;

    public PlanningIndicatorUIParam(Unit unit)
    {
        planningUnit = unit;
    }
}


public class PlanningIndicator : PanelUIComponent
{
    [SerializeField]
    private Transform indicatorImage;

    private Unit planningTargetUnit;

    private void Update()
    {
        if(gameObject.activeInHierarchy && planningTargetUnit != null)
        {
            indicatorImage.transform.position = Camera.main.WorldToScreenPoint(planningTargetUnit.transform.position);
        }
    }

    public override void SetPanel(UIParam u = null)
    {
        PlanningIndicatorUIParam param = (PlanningIndicatorUIParam)u;

        planningTargetUnit = param.planningUnit;

        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
        planningTargetUnit = null;
    }
}
