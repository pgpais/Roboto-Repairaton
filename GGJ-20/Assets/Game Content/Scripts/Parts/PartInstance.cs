using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stpres the instance of a part.
/// </summary>
public class PartInstance : MonoBehaviour
{
    public ConveyerBelt belt;
    
    public bool beingGrabbed;

    public void OnGrab(Transform claw)
    {
        transform.parent = claw;
        transform.localPosition = Vector3.zero;
        
        // This should only remove part from the belt's list?
        belt.RemoveConveyerPart(this);
    }

    public void OnRelease()
    {
        transform.parent = null;
        
        // Destroy if outside of building zone
        
        // Attach to something if inside?
    }
}
