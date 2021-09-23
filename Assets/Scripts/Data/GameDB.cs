using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDB", menuName = "GameDB/GameDB", order = 0)]
public class GameDB : ScriptableObject
{
    [SerializeField] private List<IGameStaticData> gameDatas;

    public void Init()
    {
        gameDatas.ForEach(data => data.MakeStatic());
    }
}
