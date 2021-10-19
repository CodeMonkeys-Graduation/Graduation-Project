using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    private HashSet<Cube> cubes = new HashSet<Cube>();
    public HashSet<Cube> Cubes { get => cubes; private set => cubes = value; }

    public Map(List<Cube> cubes, Material material)
    {
        this.Cubes.Clear();

        foreach (var cube in cubes)
        {
            MeshRenderer[] cubeMeshRenders = cube.gameObject.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer meshRenderer in cubeMeshRenders)
            {
                meshRenderer.material = material;
            }

            this.Cubes.Add(cube);
        }
    }

    public virtual void AddNode(Cube cube) => Cubes.Add(cube);
    public virtual void RemoveNode(Cube cube) => Cubes.Remove(cube);

}
