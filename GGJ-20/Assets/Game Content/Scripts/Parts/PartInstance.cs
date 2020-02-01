using System;
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
    public BoxCollider2D col;

    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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

        Collider2D hit = Physics2D.OverlapBox(transform.position, col.size, transform.rotation.eulerAngles.z, mask);
        if (hit)
        {
            // Attach to something if inside?
            AssemblyZone assembly = hit.GetComponent<AssemblyZone>();
            transform.parent = assembly.transform;
            transform.position = assembly.attachPoint.position;
            transform.localRotation = Quaternion.identity;
            assembly.AttachPart(col);
        }
        else
        {
            // Destroy if outside of building zone
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 2;
            Destroy(gameObject, 4);
        }

        
    }
}
