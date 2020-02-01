using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhalesAndGames.Pools;

/// <summary>
/// Handles the spawning of parts, defined by different conveyer belts.
/// </summary>
public class PartSpawner : MonoBehaviour
{
    [Header("Pool Parts")]
    public PoolTable partPool;

    [Header("Conveyers")]
    public ConveyerBelt[] conveyerBelts;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        List<Part> parts = GameManager.Instance.reference.parts;
        if (parts.Count == 0)
        {
            Debug.LogError("No Parts Exist in the Reference!");
            return;
        }

        foreach (Part part in parts)
        {
            PoolVariable variable = new PoolVariable(part, part.poolChance);
            partPool.AddVariable(variable);
        }

        StartCoroutine(IESpawnParts());
    }

    /// <summary>
    /// Courotine that handles spawning parts.
    /// </summary>
    public IEnumerator IESpawnParts()
    {
        if(partPool.poolVariables.Count == 0)
        {
            Debug.LogError("No Parts Exist in the Part Pool! Please double check!");
            yield break;
        }

        while(true)
        {
            yield return new WaitForSeconds(1f);
            foreach(ConveyerBelt belt in conveyerBelts)
            {
                Part pickedPart = Pool.Fetch<Part>(partPool);
                belt.SpawnPart(pickedPart.instance);
            }
        }
    }
}
