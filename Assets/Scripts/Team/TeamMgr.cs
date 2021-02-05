using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (order = 2, fileName = "New Team", menuName = "Team Object")]
public class Team : ScriptableObject
{
    public enum Controller { Player, AI }
    public Controller controller;
    public string name;
}

public class TeamMgr : MonoBehaviour
{
    [SerializeField] public List<Team> teams;
}
