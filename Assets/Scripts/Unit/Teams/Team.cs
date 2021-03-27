using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (order = 1, fileName = "New Team", menuName = "New Team")]
public class Team : ScriptableObject
{
    public enum Controller { Player, AI }
    [SerializeField] public Controller controller;
    [SerializeField] public new string name;
    [SerializeField] public Sprite teamTurnSlotFrame;
    [SerializeField] public List<Team> enemyTeams;
}
