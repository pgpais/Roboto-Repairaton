using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zone where robots and where the validation of patterns happens.
/// </summary>
public class AssemblyZone : MonoBehaviour
{
    [Header("Transform")]
    public Transform legsTransform;
    public Transform bodyTransform;
    public Transform headTransform;

    [Header("Assembled Parts")]
    public PartInstance legsPart;
    public PartInstance bodyPart;
    public PartInstance headPart;

    private GameObject shadow;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        shadow = legsTransform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Attaches a part to the collider.
    /// </summary>
    public void AttachPart(PartInstance part)
    {
        Transform targetTransform = null;
        if(legsPart == null)
        {
            legsPart = part;
            targetTransform = legsTransform;
            shadow.SetActive(true);
        }
        else if (bodyPart == null)
        {
            bodyPart = part;
            targetTransform = bodyTransform;
        }
        else if (headPart == null)
        {
            headPart = part;
            targetTransform = headTransform;
        }

        ValidateAssembly();
        AttachPartToTransform(part, targetTransform);
    }

    public void AttachPartToTransform(PartInstance part, Transform point)
    {
        part.transform.parent = point;
        part.transform.position = point.position;
    }

    /// <summary>
    /// Validates the pieces that have been put into assembly.
    /// </summary>
    public void ValidateAssembly()
    {
        // Checks if the player has done any mistake.
        if (legsPart != null && legsPart.part.partType != PartType.Legs)
        {
            RemoveAll();
            return;
        }

        if (bodyPart != null && bodyPart.part.partType != PartType.Body)
        {
            RemoveAll();
            return;
        }

        // Validates if the robot is the robot order.
        if(headPart != null)
        {
            Pattern pattern = GameManager.Instance.targetPattern;
            if (legsPart.part != pattern.legsPart)
            {
                RemoveAll();
                return;
            }

            if (bodyPart.part != pattern.bodyPart)
            {
                RemoveAll();
                return;
            }

            if (headPart.part != pattern.headPart)
            {
                RemoveAll();
                return;
            }

            GameManager.Instance.ConfirmAssembly();
        }
    }

    /// <summary>
    /// Removes all parts from the assembly line.
    /// </summary>
    public void RemoveAll()
    {
        if (legsPart == null)
        {
            return;
        }

        shadow.SetActive(false);
        legsPart.ThrowPiece();
        legsPart = null;

        if (bodyPart == null)
        {
            return;
        }

        bodyPart.ThrowPiece();
        bodyPart = null;

        if (headPart == null)
        {
            return;
        }

        headPart.ThrowPiece();
        headPart = null;
    }
}
