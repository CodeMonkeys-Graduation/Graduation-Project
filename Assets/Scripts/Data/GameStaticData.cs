using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class IGameStaticData : ScriptableObject
{
    public abstract void MakeStatic();
}

public class GameStaticData<T> : IGameStaticData where T : GameStaticData<T>
{
    public static T Instance;
    public override void MakeStatic()
    {
        Instance = (T)this;
    }
}
