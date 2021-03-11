using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr : MonoBehaviour
{
    private static EventMgr instance;
    public static EventMgr Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<EventMgr>();

            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }

    //--- Pathfind ---//
    [SerializeField] public Event onPathfindRequesterCountZero;
    [SerializeField] public Event onPathUpdatingStart;

    //--- Unit ---//
    [SerializeField] public Event onUnitAttackExit;
    [SerializeField] public Event onUnitDead;
    [SerializeField] public Event onUnitIdleEnter;
    [SerializeField] public Event onUnitItemExit;
    [SerializeField] public Event onUnitRunEnter;
    [SerializeField] public Event onUnitRunExit;
    [SerializeField] public Event onUnitSkillExit;

    //--- Turn ---//

}
