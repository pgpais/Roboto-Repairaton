using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Conveyer Belt which is then extended by the different types (linear and sushibelt) to carry items.
/// </summary>
public abstract class ConveyerBelt : MonoBehaviour
{
    [Header("Properties")]
    public float speed = 10f;
    public int direction = 1;
    public float spawningTime;

    protected List<PartInstance> partsOnBelt = new List<PartInstance>();
    protected Transform spawnedPartsTransform;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        spawningTime = GameManager.Instance.startingSpawnTime;
        spawnedPartsTransform = transform.Find("Spawned Parts");

        StartCoroutine(IESpawnParts());
    }

    /// <summary>
    /// Spawns parts by requesting them to the Game Manager.
    /// </summary>
    /// <returns></returns>
    public IEnumerator IESpawnParts()
    {
        while(true)
        {
            if(direction == 0)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(spawningTime);
            Part part = GameManager.Instance.RequestPartToSpawn();

            SpawnPart(part.instance);
        }
    }

    /// <summary>
    /// Spawns the instance of a part so that the conveyer can move it on it's own accord.
    /// </summary>
    public abstract void SpawnPart(PartInstance part);

    /// <summary>
    /// Tracks a part as being part of the conveyor belt.
    /// </summary>
    public virtual void TrackPartInstance(PartInstance partInstance)
    {
        partsOnBelt.Add(partInstance);
    }

    /// <summary>
    /// Destroys a part from the conveyer belt.
    /// </summary>
    public void DestroyConveyorPart(PartInstance partInstance)
    {
        partsOnBelt.Remove(partInstance);
        Destroy(partInstance.gameObject);
    }
}
