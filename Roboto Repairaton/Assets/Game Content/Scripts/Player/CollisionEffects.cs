using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the collision effects for when arms hit each other.
/// </summary>
public class CollisionEffects : MonoBehaviour
{
    public RobotArm arm;

    // Private.
    private AudioSource audioSource;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only).
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 position = collision.contacts[0].point;
        audioSource.Play();

        arm.SpawnCollisionParticles(position);
    }
}
