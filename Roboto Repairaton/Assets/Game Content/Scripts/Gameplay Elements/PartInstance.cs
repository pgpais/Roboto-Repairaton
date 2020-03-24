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
    public RobotArm lastRobotArm;
    [ReadOnly]
    public ConveyorBelt belt;
    [ReadOnly]
    public bool beingGrabbed;

    // Private
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private bool inAssembly;
    private bool insideAssemblyZone;
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
            return;
        }

        if(!beingGrabbed)
        {
            return;
        }

        // Checks what the piece is currently in bounds off.
        Collider2D hit = Physics2D.OverlapBox(transform.position, col.size, transform.rotation.eulerAngles.z, mask);
        if (hit != null && hit.gameObject == GameManager.Instance.AssemblyZone.gameObject)
        {
            insideAssemblyZone = true;
            GameManager.Instance.AssemblyZone.ToggleCircleGlow(true);
        }
        else if (insideAssemblyZone)
        {
            insideAssemblyZone = false;
            GameManager.Instance.AssemblyZone.ToggleCircleGlow(false);
        }
    }

    /// <summary>
    /// Called when a piece is grabbed from the player's magnet.
    /// </summary>
    public void OnGrab(RobotArm arm, Transform claw)
    {
        if(inAssembly || markedToDestory)
        {
            return;
        }

        beingGrabbed = true;
        GetComponent<Animator>().SetBool("isHovered", false);

        // Marks the body as being kinematic.
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        // Sets the parent to the claw.
        lastRobotArm = arm;
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
        beingGrabbed = false;
        transform.parent = null;

        GameManager.Instance.AssemblyZone.ToggleCircleGlow(false);

        // Depending on where the piece is, decide on how to drop it.
        if (insideAssemblyZone)
        {
            GameManager.Instance.AssemblyZone.AttachPart(this);
            transform.localRotation = Quaternion.identity;

            inAssembly = true;
            col.enabled = false;
        }
        else
        {
            // Makes it dynamic if outside of the assembly zone.
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 2;
        }    
    }

    /// <summary>
    /// Throws the piece into oblivion.
    /// </summary>
    public void ThrowPiece()
    {
        inAssembly = false;
        beingGrabbed = false;
        markedToDestory = true;

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
