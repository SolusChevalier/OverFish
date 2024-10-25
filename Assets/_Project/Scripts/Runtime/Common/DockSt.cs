using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DockSt
{
    public string Name;
    public int PlayerNumber;
    public Collider Collider;
    public Bounds Bounds;

    public DockSt(string name, int playerNumber, Collider collider)
    {
        Name = name;
        PlayerNumber = playerNumber;
        Collider = collider;
        Bounds = collider.bounds;
    }

    public bool IsDocked(Vector3 position)
    {
        return Bounds.Contains(position);
    }
}