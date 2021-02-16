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
        //cube = apUnit.cube;
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

public class APCube : INavable
{
    public Cube owner;
    public APUnit unit;

    private List<INavable> neighbors = new List<INavable>();
    private Transform platform;
    public List<INavable> Neighbors { get => neighbors; set => neighbors = value; }
    public Transform Platform { get => null; set => platform = null; }


    public APCube(Cube cube)
    {
        owner = cube;
    }

    public APCube(APCube apCube)
    {
        owner = apCube.owner;
        //unit = apCube.unit;
        //neighbors = new List<INavable>(apCube.Neighbors);
    }

    public bool IsAccupied() => unit != null;
}

public class APGameState
{
    public List<APCube> _cubes;
    public List<APUnit> _units;
    public APUnit self { get => _units.Find(u => u.isSelf); }
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
            foreach(var neighbor in cube.Neighbors)
            {
                APCube apNeighbor = APFind(neighbor);
                APCube apCube = APFind(cube);

                apCube.Neighbors.Add(apNeighbor);
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

        // set position
        foreach(var unit in units)
        {
            APUnit _apUnit = _units.Find(_u => _u.owner == unit.owner);
            APCube apCube = cubes.Find(c => c.unit != null && c.unit.owner == _apUnit.owner);
            APCube _apCube = _cubes.Find(_c => _c.owner == apCube.owner);

            SetPos(_apUnit, _apCube);
        }

        // set neighbors
        foreach(var cubeOwner in cubes.Select(c => c.owner))
        {
            APCube _apCube = _cubes.Find(_c => _c.owner == cubeOwner);

            List<APCube> _apNeighbor = _cubes.Where(c => cubeOwner.Neighbors.Contains(c.owner)).ToList();
            _apCube.Neighbors.AddRange(_apNeighbor);
        }
    }

    public APGameState Clone() => new APGameState(_units, _cubes);

    public APCube APFind(INavable cube) => _cubes.Find(c => c.owner == cube);
    public APUnit APFind(Unit unit) => _units.Find(u => u.owner == unit);

    private void SetPos(APUnit unit, APCube cube)
    {
        unit.cube = cube;
        cube.unit = unit;
    }
}
