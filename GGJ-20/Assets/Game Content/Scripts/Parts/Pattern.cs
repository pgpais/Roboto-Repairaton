using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pattern symbolizes an 'order' of how a robot should be assembled. The player must complete assembles as they are received.
/// </summary>
[CreateAssetMenu(fileName = "New Part Pattern", menuName = "Robots/Pattern")]
public class Pattern : ScriptableObject
{
    [Header("Pattern Parts")]
    [ValidateInput("ValidateHead", "This part is not a head part or is not assigned!", InfoMessageType.Warning)]
    public Part headPart;
    [ValidateInput("ValidateBody", "This part is not a body part or is not assigned!", InfoMessageType.Warning)]
    public Part bodyPart;
    [ValidateInput("ValidateLegs", "This part is not a leg part or is not assigned!", InfoMessageType.Warning)]
    public Part legsPart;

    #region Validations
#if UNITY_EDITOR
    public static bool ValidateHead(Part headPart)
    {
        if (headPart == null)
            return false;

        return headPart.partType == PartType.Head;
    }
    public static bool ValidateBody(Part bodyPart)
    {
        if (bodyPart == null)
            return false;

        return bodyPart.partType == PartType.Body;
    }
    public static bool ValidateLegs(Part legsPart)
    {
        if (legsPart == null)
            return false;

        return legsPart.partType == PartType.Legs;
    }
#endif
    #endregion
}
