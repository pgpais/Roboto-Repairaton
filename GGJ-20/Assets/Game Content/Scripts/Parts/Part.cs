using AssetIcons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Robot Part that is used for assembling a robot.
/// </summary>
[CreateAssetMenu(fileName = "New Part", menuName = "Robot/Part")]
public class Part : ScriptableObject
{
    [Header("Definition")]
    [AssetIcon]
    public Sprite sprite;
    public GameObject instance;
    public PartType partType;
}

public enum PartType
{
    Legs,
    Body,
    Head
}
