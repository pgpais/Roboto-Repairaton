using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zone where robots and where the validation of patterns happens.
/// </summary>
public class AssemblyZone : MonoBehaviour
{
    public Transform[] attachPoints = new Transform[3];
    protected List<PartInstance> partsOnAssembly = new List<PartInstance>();

    /// <summary>
    /// Attaches a part to the collider.
    /// </summary>
    public void AttachPart(PartInstance part)
    {
        partsOnAssembly.Add(part);
        ValidateAssembly();

        Transform attachPoint = attachPoints[partsOnAssembly.Count];
        part.transform.parent = attachPoint;
        part.transform.position = attachPoint.position;
    }

    /// <summary>
    /// Removes a part from assembly.
    /// </summary>
    public void RemovePart(PartInstance part)
    {
        partsOnAssembly.Remove(part);
    }

    /// <summary>
    /// Validates the pieces that have been put into assembly.
    /// </summary>
    public void ValidateAssembly()
    {
        // Checks if the player has done any mistake.
        if (partsOnAssembly.Count == 1 && partsOnAssembly[0].part.partType != PartType.Legs)
        {
            RemoveAll();
        }

        if (partsOnAssembly.Count == 2 && partsOnAssembly[1].part.partType != PartType.Body)
        {
            RemoveAll();
        }

        foreach (PartInstance part in partsOnAssembly)
        {
            if(partsOnAssembly.Find(x => x != part && x.part.partType == part.part.partType))
            {
                RemoveAll();
            }
        }
    }

    /// <summary>
    /// Removes all parts from the assembly line.
    /// </summary>
    public void RemoveAll()
    {
        foreach(PartInstance partInstance in partsOnAssembly)
        {
            partInstance.ThrowPiece();
        }

        partsOnAssembly.Clear();
    }
}
