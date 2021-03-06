﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A circular conveyor belt tracks parts around a semi-circle.
/// </summary>
public class CircularConveyorBelt : ConveyorBelt
{
    [Header("Circular Anchors")]
    public float radius;
    public bool inverted;

    private SpriteRenderer sushiBelt;
    private Dictionary<GameObject, float> angleDictionary = new Dictionary<GameObject, float>();

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        sushiBelt = transform.Find("Sushi Belt").GetComponent<SpriteRenderer>();

        if(inverted)
        {
            sushiBelt.flipX = true;
        }
    }

    /// <summary>
    /// Spawns the instance of a part so that the conveyor can move it on it's own accord.
    /// </summary>
    public override void SpawnPart(PartInstance part)
    {
        Vector2 targetStartingPoint = transform.position;
        targetStartingPoint = new Vector2(!inverted ? transform.position.x - radius : transform.position.x + radius, transform.position.y);

        if (!CheckSpawnClear(targetStartingPoint))
        {
            return;
        }

        PartInstance partInstance = Instantiate(part, targetStartingPoint, Quaternion.identity);
        partInstance.transform.SetParent(spawnedPartsTransform);

        partsOnBelt.Add(partInstance);
        angleDictionary.Add(partInstance.gameObject, 0);

        if(!inverted)
        {
            angleDictionary[partInstance.gameObject] = !invertedDirection ? -1.2f : 4.4f;
        }
        else
        {
            angleDictionary[partInstance.gameObject] = !invertedDirection ? 1.2f : -4.4f;
        }
    }

    /// <summary>
    /// Animates the conveyor belt.
    /// </summary>
    public override void AnimateConveyor()
    {
        if(!invertedDirection)
        {
            if(sushiBelt.flipY)
            {
                sushiBelt.flipY = false;
            }

            sushiBelt.transform.Rotate(0, 0, (!inverted ? -speed * 60 : speed * 60) * Time.deltaTime);

        }
        else
        {
            if (!sushiBelt.flipY)
            {
                sushiBelt.flipY = true;
            }

            sushiBelt.transform.Rotate(0, 0, (!inverted ? speed * 60 : -speed * 60) * Time.deltaTime);
        }
    }


    /// <summary>
    /// Moves a part along the belt.
    /// </summary>
    public override void MovePart(PartInstance part)
    {
        Vector2 offset = Vector2.zero;

        if (invertedDirection)
        {
            if (!inverted)
            {
                angleDictionary[part.gameObject] -= speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x - radius, transform.position.y), part.transform.position) <= 0.1f)
                {
                    DestroyConveyorPart(part);
                }
            }
            else
            {
                angleDictionary[part.gameObject] += speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x + radius, transform.position.y), part.transform.position) <= 0.1f)
                {
                    DestroyConveyorPart(part);
                }
            }
        }
        else
        {
            if (!inverted)
            {
                angleDictionary[part.gameObject] += speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x - radius, transform.position.y), part.transform.position) <= 0.1f)
                {
                    DestroyConveyorPart(part);
                }
            }
            else
            {
                angleDictionary[part.gameObject] -= speed * Time.deltaTime;

                offset = new Vector2(Mathf.Sin(angleDictionary[part.gameObject]), Mathf.Cos(angleDictionary[part.gameObject])) * radius;
                part.gameObject.transform.position = (Vector2)transform.position + offset;
                if (Vector2.Distance(new Vector2(transform.position.x + radius, transform.position.y), part.transform.position) <= 0.2f)
                {
                    DestroyConveyorPart(part);
                }
            }
        }
    }

    /// <summary>
    /// Removes a part from the conveyor belt.
    /// </summary>
    public override void RemoveConveyorPart(PartInstance part)
    {
        partsOnBelt.Remove(part);
        angleDictionary.Remove(part.gameObject);
    }

    /// <summary>
    /// Destroys a part from the conveyor belt. In the circular conveyor belt, also removes it from angle calculations.
    /// </summary>
    public override void DestroyConveyorPart(PartInstance part)
    {
        base.DestroyConveyorPart(part);
    }

    /// <summary>
    /// Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawSphere(new Vector2(!inverted ? transform.position.x - radius : transform.position.x + radius, transform.position.y), 0.2f);
    }


}
