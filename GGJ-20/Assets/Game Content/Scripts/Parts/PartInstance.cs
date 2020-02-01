using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stpres the instance of a part.
/// </summary>
public class PartInstance : MonoBehaviour
{
    [Header("Part")]
    public Part part;

    [ReadOnly]
    public ConveyerBelt belt;
    [ReadOnly]
    public bool beingGrabbed;
    public BoxCollider2D col;

    private Rigidbody2D rb;
    [SerializeField]
    private LayerMask mask;
    private bool inAssembly;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnGrab(Transform claw)
    {
        if(inAssembly)
        {
            return;
        }
        
        transform.SetParent(claw);
        transform.localPosition = Vector3.zero;
        
        // This should only remove part from the belt's list?
        if(belt != null)
        {
            belt.RemoveConveyerPart(this);
        }
    }

    public void OnRelease()
    {
        transform.parent = null;

        Collider2D hit = Physics2D.OverlapBox(transform.position, col.size, transform.rotation.eulerAngles.z, mask);
        if (hit)
        {
            // Attach to something if inside?
            AssemblyZone assembly = hit.GetComponent<AssemblyZone>();
            transform.localRotation = Quaternion.identity;
            assembly.AttachPart(this);

            inAssembly = true;
            col.enabled = false;
        }
        else
        {
            // Destroy if outside of building zone.
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 2;
            Destroy(gameObject, 4);
        }    
    }

    /// <summary>
    /// Throws the piece into oblivion.
    /// </summary>
    public void ThrowPiece()
    {
        inAssembly = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2;
        rb.AddForce(new Vector2(Random.Range(-400, 400), Random.Range(200, 400)));
        Destroy(gameObject, 4);
    }
}
