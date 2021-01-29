using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Map
{
    private HashSet<Cube> cubes = new HashSet<Cube>();
    public HashSet<Cube> Cubes { get => cubes; private set => cubes = value; }

    public Map(List<Cube> cubes)
    {
        this.Cubes.Clear();

        foreach (var cube in cubes)
            this.Cubes.Add(cube);
    }

    public virtual void AddNode(Cube cube) => Cubes.Add(cube);
    public virtual void RemoveNode(Cube cube) => Cubes.Remove(cube);

}
