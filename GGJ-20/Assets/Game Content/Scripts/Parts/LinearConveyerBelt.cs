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

    private Transform conveyerBelt;

    protected override void Start()
    {
        base.Start();
        conveyerBelt = transform.Find("Conveyer Belt");
    }

    /// <summary>
    /// Spawns the instance of a part so that the conveyer can move it on it's own accord.
    /// </summary>
    public override void SpawnPart(PartInstance part)
    {
        Transform targetSpawn = null;
        if (!invertedDirection)
        {
            targetSpawn = leftPoint;
        }
        else
        {
            targetSpawn = rightPoint;
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
                DestroyConveyerPart(part);
            }
        }
        else
        {
            part.transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
            if (Vector2.Distance(leftPoint.position, part.transform.position) <= 0.2f)
            {
                DestroyConveyerPart(part);
            }
        }
    }

    /// <summary>
    /// Animates the conveyer belt.
    /// </summary>
    public override void AnimateConveyer()
    {
        if (!invertedDirection)
        {
            conveyerBelt.Translate(new Vector2(speed * Time.deltaTime, 0));
            if(conveyerBelt.position.x >= 2.38f)
            {
                Vector2 pos = conveyerBelt.position;
                pos.x = 0;
                conveyerBelt.position = pos;
            }

            if(conveyerBelt.GetChild(0).GetComponent<SpriteRenderer>().flipX)
            {
                foreach(Transform childSprite in conveyerBelt)
                {
                    childSprite.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }
        else
        {
            conveyerBelt.transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
            if (conveyerBelt.position.x <= -2.38f)
            {
                Vector2 pos = conveyerBelt.position;
                pos.x = 0;
                conveyerBelt.position = pos;
            }

            if (!conveyerBelt.GetChild(0).GetComponent<SpriteRenderer>().flipX)
            {
                foreach (Transform childSprite in conveyerBelt)
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
