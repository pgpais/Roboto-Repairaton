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

    [SerializeField]
    private LayerMask mask;

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

        Collider2D hit = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().size, transform.rotation.eulerAngles.z, mask);
        if (hit)
        {
            Debug.Log(hit.name);
            // Attach to something if inside?
            transform.parent = hit.GetComponent<AssemblyZone>().AttachPoint;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            // Destroy if outside of building zone  
            Destroy(gameObject);
        }

        
    }
}
