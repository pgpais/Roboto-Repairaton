using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the collisions of the check area of the robot arm.
/// </summary>
public class RobotArmGrabArea : MonoBehaviour
{
    public RobotArm arm;

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        arm.ColliderDetected(other.gameObject);
    }

    /// <summary>
    /// OnTriggerStay2D is called once per frame for every Collider2D other that is touching the trigger (2D physics only).
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        arm.ColliderDetected(collision.gameObject);
    }
}
