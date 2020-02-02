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
    
    //TODO: create variable to make sure only one part is selectable?

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        arm.ColliderDetected(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Part"))
            other.GetComponent<Animator>().SetBool("isHovered", true);
    }

    /// <summary>
    /// OnTriggerStay2D is called once per frame for every Collider2D other that is touching the trigger (2D physics only).
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Part"))
            arm.ColliderDetected(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Part"))
            other.GetComponent<Animator>().SetBool("isHovered", false);
    }
}
