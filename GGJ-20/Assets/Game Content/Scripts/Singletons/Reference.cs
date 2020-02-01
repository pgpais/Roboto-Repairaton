using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reference", menuName = "Robots/Reference")]
public class Reference : ScriptableObject
{
    [Header("Parts")]
    public List<Part> parts;

    [Header("Patterns")]
    public List<Pattern> patterns;
}
