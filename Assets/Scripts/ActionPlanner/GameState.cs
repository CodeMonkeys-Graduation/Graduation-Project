using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState
{
    public Unit owner;
    public CubeState cube;
    public int health;
    public Team team;
    public int actionPoint;
    public bool isSelf;

}

public class CubeState 
{
    public UnitState unit;
    public List<CubeState> neighbors;
    //public List<PFPath<T>> paths;
}

public class GameState
{
    public List<CubeState> cubes;
    public List<UnitState> units;
    public GameState(Unit self, List<Unit> units, List<Cube> cubes)
    {
        this.cubes = new List<CubeState>();
        this.units = new List<UnitState>();

        //units.ForEach((u) => this.units.Add(new UnitState(u, u == self ? true : false)));
        //cubes.ForEach((c) => this.cubes.Add(new CubeState(c)));
    }



}
