using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Conveyor Belt which is then extended by the different types (linear and sushibelt) to carry items.
/// </summary>
public abstract class ConveyorBelt : MonoBehaviour
{
    [Header("Properties")]
    public LayerMask spawningMask;
    public float speed = 10f;
    public bool stopped;
    public bool invertedDirection;
    public float spawningTime;

    protected List<PartInstance> partsOnBelt = new List<PartInstance>();
    protected Transform spawnedPartsTransform;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    protected virtual void Start()
    {
        spawningTime = GameManager.Instance.startingSpawnTime;
        spawnedPartsTransform = transform.Find("Spawned Parts");
    }

    /// <summary>
    /// Starts a belt into spawning parts. Can be used to give a small delay so we can show something when starting the game.
    /// </summary>
    public void StartSpawnParts()
    {
        StartCoroutine(IESpawnParts());
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (partsOnBelt.Count == 0)
        {
            return;
        }

        for (int i = 0; i < partsOnBelt.Count; i++)
        {
            PartInstance part = partsOnBelt[i];
            if(part == null)
            {
                continue;
            }

            if (part.beingGrabbed)
            {
                continue;
            }

            MovePart(part);
        }

        AnimateConveyor();
    }

    /// <summary>
    /// Moves a part along the belt.
    /// </summary>
    public abstract void MovePart(PartInstance part);

    /// <summary>
    /// Animates the conveyor belt.
    /// </summary>
    public abstract void AnimateConveyor();

    /// <summary>
    /// Spawns parts by requesting them to the Game Manager.
    /// </summary>
    /// <returns></returns>
    public IEnumerator IESpawnParts()
    {
        while(true)
        {
            if(stopped == true)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(spawningTime);
            Part part = GameManager.Instance.RequestPartToSpawn();

            part.instance.belt = this; // Links the part to the belt (so it knows where to trigger the remove from)
            SpawnPart(part.instance);
        }
    }

    /// <summary>
    /// Spawns the instance of a part so that the conveyor can move it on it's own accord.
    /// </summary>
    public abstract void SpawnPart(PartInstance part);

    /// <summary>
    /// Checks if the spawn point is clear.
    /// </summary>
    public bool CheckSpawnClear(Vector2 target)
    {
        Collider2D[] pointsAtSpawn = Physics2D.OverlapPointAll(target, spawningMask);
        if (pointsAtSpawn.Length == 0)
        {
            return true;
        }

        foreach (Collider2D point in pointsAtSpawn)
        {
            if (point.transform.parent == spawnedPartsTransform)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Tracks a part as being part of the conveyor belt.
    /// </summary>
    public virtual void TrackPartInstance(PartInstance partInstance)
    {
        partsOnBelt.Add(partInstance);
    }

    /// <summary>
    /// Removes a part from the conveyor belt.
    /// </summary>
    public virtual void RemoveConveyorPart(PartInstance part)
    {
        partsOnBelt.Remove(part);
    }

    /// <summary>
    /// Destroys a part from the conveyor belt.
    /// </summary>
    public virtual void DestroyConveyorPart(PartInstance part)
    {
        RemoveConveyorPart(part);
        Destroy(part.gameObject);
    }
}
