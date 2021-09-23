using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStaticData", menuName = "GameDB/UnitStaticData", order = 0)]
public class UnitStaticData : GameStaticData<UnitStaticData>
{
    [System.Serializable]
    public class UnitStatDictionary : SerializableDictionaryBase<UnitType, UnitStat> { }

    public enum UnitType
    {
        SwordMan,
        Archer,
        Wizard,
        Commander,
        Slime,
        Skeleton,
        TurtleShell,
        Golem,
        Bat,
        EvilMaze,
    }

    public UnitStatDictionary unitStats;
}
