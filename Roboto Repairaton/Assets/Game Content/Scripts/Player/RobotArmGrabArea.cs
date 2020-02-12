using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

/// <summary>
/// Handles the collisions of the check area of the robot arm.
/// </summary>
public class RobotArmGrabArea : MonoBehaviour
{
    public RobotArm arm;

    /// <summary>
    /// OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PartInstance part = collision.GetComponent<PartInstance>();
        if (part != null)
        {
            arm.MarkPartAsGrabbable(part);
            part.GetComponent<Animator>().SetBool("isHovered", true);
        }
    }

    /// <summary>
    /// OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only).
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        PartInstance part = collision.GetComponent<PartInstance>();
        if (part != null)
        {
            arm.RemovePartAsGrabbable(part);
            part.GetComponent<Animator>().SetBool("isHovered", false);
        }
    }
}
