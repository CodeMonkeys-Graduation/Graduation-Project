using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState
{
    public int health;
    public Cube position;
    public Team team;
    public int actionPoint;
    public bool isSelf;

    public UnitState(Unit unit, bool isSelf)
    {
        this.isSelf = isSelf;
        this.health = unit.Health;
        this.position = unit.GetCube;
        this.team = unit.team;
        this.actionPoint = unit.actionPointsRemain;
    }
}

public class CubeState 
{
    public Unit unitOnCube;
    public List<Cube> neighbors;
    public List<PFPath> paths;
    public CubeState(Cube cube)
    {
        this.unitOnCube = cube.GetUnit();
        this.neighbors = new List<Cube>(cube.neighbors);
        this.paths = new List<PFPath>(cube.paths);
    }
}

public class GameState
{
    public List<CubeState> cubes;
    public List<UnitState> units;
    public GameState(Unit self, List<Unit> units, List<Cube> cubes)
    {
        this.cubes = new List<CubeState>();
        this.units = new List<UnitState>();

        units.ForEach((u) => this.units.Add(new UnitState(u, u == self ? true : false)));
        cubes.ForEach((c) => this.cubes.Add(new CubeState(c)));
    }



}
