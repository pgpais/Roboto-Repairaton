using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the actual living data relevant to the game of a piece, after it's been instasiated. 
/// </summary>
public class PartInstance : MonoBehaviour
{
    [Header("Part SO")]
    public Part part;

    [Header("Masking")]
    [SerializeField]
    private LayerMask mask = 0;

    [Header("In-Game Properties")]
    [ReadOnly]
    public int playerId;
    [ReadOnly]
    public ConveyorBelt belt;
    [ReadOnly]
    public bool beingGrabbed;

    // Private
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private bool inAssembly;
    private bool markedToDestory = false;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (transform.position.y < -16f && !markedToDestory)
        {
            markedToDestory = true;
            Destroy(gameObject);
            GameManager.Instance.partsDropped++;
        }
    }

    /// <summary>
    /// Called when a piece is grabbed from the player's magnet.
    /// </summary>
    public void OnGrab(Transform claw)
    {
        if(inAssembly)
        {
            return;
        }

        GetComponent<Animator>().SetBool("isHovered", false);

        transform.SetParent(claw);
        transform.localPosition = Vector3.zero;
        
        // This should only remove part from the belt's list?
        if(belt != null)
        {
            belt.RemoveConveyorPart(this);
        }
    }

    /// <summary>
    /// Called when a piece is released from the player's magnet.
    /// </summary>
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
    }

    /// <summary>
    /// Changes the sprites to fixes after the robot gets repaired.
    /// </summary>
    public void ChangeSpritesToFixed()
    {
        GetComponent<SpriteRenderer>().sprite = part.fixedSprite;

        // Used for lighting up the eyes if it's an head.
        if(transform.Find("Eyes") != null)
        {
            transform.Find("Eyes").GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }
}
