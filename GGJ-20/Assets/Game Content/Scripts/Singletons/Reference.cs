using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores references to Scriptable Objects that are loaded to memory on game start.
/// </summary>
[CreateAssetMenu(fileName = "New Reference", menuName = "Robots/Reference")]
public class Reference : ScriptableObject
{
    [Header("Parts")]
    public List<Part> parts;

    [Header("Patterns")]
    public List<Pattern> patterns;
}
