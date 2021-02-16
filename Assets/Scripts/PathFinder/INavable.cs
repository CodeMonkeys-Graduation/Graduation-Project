using System.Collections.Generic;
using UnityEngine;

public interface INavable
{
    List<INavable> Neighbors { get; set; }
    Transform Platform { get; set; }

    bool IsAccupied();
}
