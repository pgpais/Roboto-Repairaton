using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A linear conveyor belt tracks parts from point A to point B.
/// </summary>
public class LinearConveyorBelt : ConveyorBelt
{
    [Header("Linear Anchors")]
    public Transform leftPoint;
    public Transform rightPoint;

    private Transform conveyorBelt;

    protected override void Start()
    {
        base.Start();
        conveyorBelt = transform.Find("Conveyor Belt");
    }

    /// <summary>
    /// Spawns the instance of a part so that the conveyor can move it on it's own accord.
    /// </summary>
    public override void SpawnPart(PartInstance part)
    {
        Transform targetSpawn = !invertedDirection ? leftPoint : rightPoint;
        if(!CheckSpawnClear(targetSpawn.position))
        {
            return;
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
        if (!invertedDirection)
        {
            part.transform.Translate(new Vector2(speed * Time.deltaTime, 0));
            if (Vector2.Distance(rightPoint.position, part.transform.position) <= 0.2f)
            {
                DestroyConveyorPart(part);
            }
        }
        else
        {
            part.transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
            if (Vector2.Distance(leftPoint.position, part.transform.position) <= 0.2f)
            {
                DestroyConveyorPart(part);
            }
        }
    }

    /// <summary>
    /// Animates the conveyor belt.
    /// </summary>
    public override void AnimateConveyor()
    {
        if (!invertedDirection)
        {
            conveyorBelt.Translate(new Vector2(speed * Time.deltaTime, 0));
            if(conveyorBelt.position.x >= 2.38f)
            {
                Vector2 pos = conveyorBelt.position;
                pos.x = 0;
                conveyorBelt.position = pos;
            }

            if(conveyorBelt.GetChild(0).GetComponent<SpriteRenderer>().flipX)
            {
                foreach(Transform childSprite in conveyorBelt)
                {
                    childSprite.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
        else
        {
            conveyorBelt.transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
            if (conveyorBelt.position.x <= -2.38f)
            {
                Vector2 pos = conveyorBelt.position;
                pos.x = 0;
                conveyorBelt.position = pos;
            }

            if (!conveyorBelt.GetChild(0).GetComponent<SpriteRenderer>().flipX)
            {
                foreach (Transform childSprite in conveyorBelt)
                {
                    childSprite.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
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
