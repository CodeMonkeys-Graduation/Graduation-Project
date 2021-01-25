using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map
{
    protected HashSet<Cube> cubes = new HashSet<Cube>();
    public HashSet<Cube> Cubes { get => cubes; set => cubes = value; }

    public virtual void AddNode(Cube cube) => Cubes.Add(cube);
    public virtual void RemoveNode(Cube cube) => Cubes.Remove(cube);

}
