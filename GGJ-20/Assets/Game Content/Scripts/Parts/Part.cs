using AssetIcons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Robot Part that is used for assembling a robot.
/// </summary>
[CreateAssetMenu(fileName = "New Part", menuName = "Robots/Part")]
public class Part : ScriptableObject
{
    [Header("Definition")]
    [AssetIcon]
    public Sprite sprite;
    public PartInstance instance;
    public PartType partType;

    [Header("Probability")]
    public int poolChance;
}

public enum PartType
{
    Legs,
    Body,
    Head
}
