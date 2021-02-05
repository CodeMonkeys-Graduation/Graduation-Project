using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (order = 2, fileName = "New Team", menuName = "Team Object")]
public class Team : ScriptableObject
{
    public enum Controller { Player, AI }
    [SerializeField] public Controller controller;
    [SerializeField] public string name;

    [SerializeField] public List<Team> enemyTeams;
}

public class TeamMgr : MonoBehaviour
{
    [SerializeField] public List<Team> teams;
}
