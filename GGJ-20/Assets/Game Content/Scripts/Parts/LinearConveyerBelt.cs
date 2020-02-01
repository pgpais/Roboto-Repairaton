using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A linear conveyer belt tracks parts from point A to point B.
/// </summary>
public class LinearConveyerBelt : ConveyerBelt
{
    [Header("Linear Anchors")]
    public Transform leftPoint;
    public Transform rightPoint;

    /// <summary>
    /// Spawns the instance of a part so that the conveyer can move it on it's own accord.
    /// </summary>
    public override void SpawnPart(PartInstance part)
    {
        Transform targetSpawn = null;
        switch (direction)
        {
            case -1:
                targetSpawn = rightPoint;
                break;
            case 0:
                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    targetSpawn = rightPoint;
                }
                else
                {
                    targetSpawn = leftPoint;
                }
                break;
            case 1:
                targetSpawn = leftPoint;
                break;
        }

        PartInstance partInstance = Instantiate(part, targetSpawn.position, Quaternion.identity);
        partInstance.transform.SetParent(spawnedPartsTransform);

        partsOnBelt.Add(partInstance);
    }

    /// <summary>
    /// Moves a part along the belt.
    /// </summary>
    public override void MovePart(PartInstance part)
    {
        switch(direction)
        {
            case -1:
                part.transform.Translate((leftPoint.position - part.transform.position).normalized * speed * Time.deltaTime);
                if(Vector2.Distance(leftPoint.position, part.transform.position) <= 0.1f)
                {
                    DestroyConveyorPart(part);
                }
                break;
            case 0:
                break;
            case 1:
                part.transform.Translate((rightPoint.position - part.transform.position).normalized * speed * Time.deltaTime);
                if (Vector2.Distance(rightPoint.position, part.transform.position) <= 0.1f)
                {
                    DestroyConveyorPart(part);
                }
                break;
        }
    }

    /// <summary>
    /// Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(leftPoint.transform.position, 0.2f);
        Gizmos.DrawLine(leftPoint.transform.position, rightPoint.transform.position);
        Gizmos.DrawSphere(rightPoint.transform.position, 0.2f);
    }
}
