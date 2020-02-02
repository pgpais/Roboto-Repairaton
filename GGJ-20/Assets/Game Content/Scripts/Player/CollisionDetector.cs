using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public RobotArm arm;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 position = other.contacts[0].point;
        arm.SpawnCollisionParticles(position);
    }
}
