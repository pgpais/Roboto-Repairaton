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
    public PoolStandalone partPool;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        List<Part> parts = GameManager.Instance.reference.parts;
        foreach (Part part in parts)
        {
            PoolVariable variable = new PoolVariable(part, part.poolChance);
            partPool.poolTable.AddVariable(variable);
        }
    }

    /// <summary>
    /// Courotine that handles spawning parts.
    /// </summary>
    public IEnumerator IESpawnParts()
    {
        yield return new WaitForSeconds(1f);
    }
}
