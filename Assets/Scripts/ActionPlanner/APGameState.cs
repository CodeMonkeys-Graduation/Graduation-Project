using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class APUnit
{
    public Unit owner;
    public APCube cube;

    public Team team;
    public int health;
    public int actionPoint;
    public bool isSelf;

    public APUnit(Unit unit, bool isSelf)
    {
        owner = unit;
        team = unit.team;
        health = unit.Health;
        actionPoint = unit.actionPointsRemain;
        this.isSelf = isSelf;
    }

    public APUnit(APUnit apUnit)
    {
        owner = apUnit.owner;
        team = apUnit.team;
        health = apUnit.health;
        actionPoint = apUnit.actionPoint;
        isSelf = apUnit.isSelf;
    }

    public void MoveTo(APCube cube)
    {
        this.cube.unit = null;
        cube.unit = this;
    }
}

public class APCube : Navable<APUnit>
{
    public Cube owner;
    public APUnit unit;

    public APCube(Cube cube)
    {
        owner = cube;
    }

    public APCube(APCube apCube)
    {
        owner = apCube.owner;
    }

    public override APUnit WhoAccupied() => unit;
}

public class APGameState
{
    public List<APCube> _cubes;
    public List<APUnit> _units;
    public APGameState(Unit self, List<Unit> units, List<Cube> cubes)
    {
        _cubes = new List<APCube>();
        _units = new List<APUnit>();

        // init APUnit List
        foreach(var unit in units)
            _units.Add(new APUnit(unit, unit == self ? true : false));

        // init APCube List
        foreach (var cube in cubes)
            _cubes.Add(new APCube(cube));

        // set neighbors
        foreach(var cube in cubes)
        {
            foreach(var neighbor in cube.neighbors)
            {
                APCube apNeighbor = APFind(neighbor as Cube);
                APCube apCube = APFind(cube);

                apCube.neighbors.Add(apNeighbor);
            }
        }

        // set position
        foreach(var unit in units)
        {
            Cube cube = unit.GetCube;
            APCube apCube = APFind(cube);
            APUnit apUnit = APFind(unit);

            SetPos(apUnit, apCube);
        }
    }

    public APGameState(List<APUnit> units, List<APCube> cubes)
    {
        _cubes = new List<APCube>();
        _units = new List<APUnit>();

        // init basic members variables
        foreach(var unit in units)
        {
            _units.Add(new APUnit(unit));
        }
        foreach (var cube in cubes)
        {
            _cubes.Add(new APCube(cube));
        }

        // set neighbors
        foreach (var cube in cubes)
        {
            foreach (var neighbor in cube.neighbors)
            {
                APCube apCube = APFind(cube.owner);
                APCube apNeighbor = _cubes.Find(_c => _c.owner == (neighbor as APCube).owner);
                apCube.neighbors.Add(apNeighbor);
            }
        }

        // set position
        foreach (var unit in units)
        {
            Cube cube = unit.cube.owner;
            APCube apCube = APFind(cube);
            APUnit apUnit = _units.Find(_u => _u.owner == unit.owner);

            SetPos(apUnit, apCube);
        }

    }

    public APGameState Clone() => new APGameState(_units, _cubes);

    public APCube APFind(Cube cube) => _cubes.Find(c => c.owner == cube);
    public APUnit APFind(Unit unit) => _units.Find(u => u.owner == unit);

    private void SetPos(APUnit unit, APCube cube)
    {
        unit.cube = cube;
        cube.unit = unit;
    }
}
