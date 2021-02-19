using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class APUnit
{
    public Unit owner;

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

}

public class APCube : INavable
{
    public APGameState gameState;
    public Cube owner;

    private List<INavable> neighbors = new List<INavable>();
    private Transform platform = null;
    public List<INavable> Neighbors { get => neighbors; set => neighbors = value; }
    public Transform Platform { get => null; set => platform = null; }


    public APCube(APGameState gameState, Cube cube)
    {
        this.gameState = gameState;
        owner = cube;
    }

    public APCube(APGameState gameState, APCube apCube)
    {
        this.gameState = gameState;
        owner = apCube.owner;
    }

    public bool IsAccupied() => gameState.unitPos.TryGetValue(this, out _);
}

public class APGameState
{
    public List<APCube> _cubes = new List<APCube>();
    public List<APUnit> _units = new List<APUnit>();
    public Dictionary<APCube, APUnit> unitPos = new Dictionary<APCube, APUnit>();
    public APUnit self { get => _units.Find(u => u.isSelf); }
    public APGameState(Unit self, List<Unit> units, List<Cube> cubes)
    {
        // init APUnit List
        foreach(var unit in units)
            _units.Add(new APUnit(unit, unit == self ? true : false));

        // init APCube List
        foreach (var cube in cubes)
            _cubes.Add(new APCube(this, cube));

        // set neighbors
        foreach(var cube in cubes)
        {
            foreach(var neighbor in cube.Neighbors)
            {
                APCube apNeighbor = _cubes.Find(c => c.owner == neighbor);
                APCube apCube = _cubes.Find(c => c.owner == cube);

                apCube.Neighbors.Add(apNeighbor);
            }
        }

        // set position
        foreach(var unit in units)
        {
            Cube cube = unit.GetCube;
            APCube apCube = _cubes.Find(c => c.owner == cube);
            APUnit apUnit = _units.Find(u => u.owner == unit);

            unitPos.Add(apCube, apUnit);
        }
    }

    public APGameState(APGameState gameState, List<APUnit> units, List<APCube> cubes)
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
            _cubes.Add(new APCube(this, cube));
        }

        // set position
        foreach(var unit in units)
        {
            APUnit _apUnit = _units.Find(_u => _u.owner == unit.owner);
            APCube apCube = cubes.Find(c => gameState.unitPos.TryGetValue(c, out _) && gameState.unitPos[c].owner == _apUnit.owner);
            APCube _apCube = _cubes.Find(_c => _c.owner == apCube.owner);

            unitPos.Add(_apCube, _apUnit);
        }

        // set neighbors
        foreach(var cubeOwner in cubes.Select(c => c.owner))
        {
            APCube _apCube = _cubes.Find(_c => _c.owner == cubeOwner);

            List<APCube> _apNeighbor = _cubes.Where(c => cubeOwner.Neighbors.Contains(c.owner)).ToList();
            _apCube.Neighbors.AddRange(_apNeighbor);
        }
    }

    public APGameState Clone() => new APGameState(this, _units, _cubes);

    public APCube APFind(INavable cube) => _cubes.Find(c => c.owner == cube);
    public APUnit APFind(Unit unit) => _units.Find(u => u.owner == unit);

    public void MoveTo(APCube cube)
    {
        APCube prevAPCube = this.unitPos.FirstOrDefault(p => p.Value == self).Key;
        this.unitPos.Add(cube, self);
        this.unitPos.Remove(prevAPCube);
    }
}
