using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selection : MonoBehaviour
{
    protected static List<Selectable> currentlySelected = new List<Selectable>();

    protected void Select(List<Selectable> targets)
    {
        foreach (var target in targets)
        {
            target.Select();
        }
        currentlySelected = targets;
    }

    protected void Select(Selectable target)
    {
        target.Select();
        currentlySelected = new List<Selectable> { target };
    }

    protected void ClearSelection()
    {
        for (int i = 0; i < currentlySelected.Count; i++)
        {
            currentlySelected[i].Deselect();
        }
        currentlySelected.Clear();
    }


}
