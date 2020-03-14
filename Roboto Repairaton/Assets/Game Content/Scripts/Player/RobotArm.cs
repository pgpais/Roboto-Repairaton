using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Serialization;

/// <summary>
/// Represents a player as a magnet, allowing for controls and similiar.
/// </summary>
public class RobotArm : MonoBehaviour
{
    [Header("Rewired Info")]
    public int playerId = 0; //set at Awake

    [Header("Particles")]
    public ParticleSystem sparks;
    
    [Header("Input")]
    public float armExtendSpeed = 5;
    public float armRotationSpeed = 5;
    public Vector2 armClampedSize = new Vector2(1, 14);

    [Header("Grab Options")] 
    public LayerMask mask;

    [Header("Chaos")] 
    public bool controlsFrozen;
    public bool controlsInverted;
    public Color invertedColor = Color.green;
    public Color frozenColor = Color.cyan;

    [Header("Camera Shake")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    
    private Transform clawBase;
    private Transform arm;
    private Transform claw;
    private Transform grabArea;
    private Transform grabPoint;
    
    private Player player;
    private Animator clawAnimator;
    private SpriteRenderer armRender;
    private List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
    private BoxCollider2D armCollider;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private GameObject invertedStatus;
    private GameObject frozenStatus;

    private HingeJoint2D hinge;

    // Input vars
    private float verticalInput;
    private float horizontalInput;

    private PartInstance grabbablePiece;
    private PartInstance grabbedPart;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        clawBase = transform.Find("ClawBase");
        arm = clawBase.Find("Arm");
        claw = arm.Find("Claw");
        clawAnimator = claw.Find("ClawHead/ClawMagnet").GetComponentInChildren<Animator>();
        grabArea = claw.Find("ClawHead/ClawMagnet/Grab Area");
        grabPoint = grabArea.transform.GetChild(0);
        invertedStatus = claw.Find("ClawHead/Inverted Status").gameObject;
        frozenStatus = claw.Find("ClawHead/Frozen Status").gameObject;

        allSprites.Add(clawBase.GetComponent<SpriteRenderer>());
        allSprites.Add(arm.GetComponent<SpriteRenderer>());
        allSprites.Add(claw.Find("ClawHead").GetComponent<SpriteRenderer>());
        allSprites.Add(claw.Find("ClawHead/ClawMagnet").GetComponent<SpriteRenderer>());

        audioSource = GetComponent<AudioSource>();
        armRender = arm.GetComponent<SpriteRenderer>();
        armCollider = arm.GetComponent<BoxCollider2D>();
        rb = GetComponentInChildren<Rigidbody2D>();
        hinge = GetComponentInChildren<HingeJoint2D>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.GameOver)
        {
            return;
        }

        GetMovementInput();
        GetGrabInput();
    }

    /// <summary>
    /// Gets input from the player to perform the magnet's actions.
    /// </summary>
    void GetMovementInput()
    {
        // Sets variables to zero and returns if the inputs are frozen.
        if(controlsFrozen)
        {
            horizontalInput = 0;
            verticalInput = 0;
            return;
        }

        // Extend and shrink arm.
        verticalInput = player.GetAxis("Vertical");
        Vector2 size = armRender.size;
        size.y += ((controlsInverted ? -verticalInput : verticalInput) * armExtendSpeed * Time.deltaTime);
        size.y = Mathf.Clamp(size.y, armClampedSize.x, armClampedSize.y);
        armRender.size = size;

        Vector2 clawPosition = claw.transform.localPosition;
        clawPosition.y = size.y - 1.7f;
        claw.transform.localPosition = clawPosition;

        // Rotate Arms.
        horizontalInput = player.GetAxis("Horizontal");

        // Update Collider depending on size.
        Vector2 spriteSize = armRender.size;
        spriteSize.x *= 0.3f;
        armCollider.size = spriteSize;
        armCollider.offset = new Vector2(0, spriteSize.y / 2);
    }

    /// <summary>
    /// Handles the grabbing input for a piece that's being held.
    /// </summary>
    public void GetGrabInput()
    {
        // If a piece is being grabbed, stop grabbing.
        if (grabbedPart != null)
        {
            if (player.GetButtonUp("Grab"))
            {
                OnReleasePiece();
                grabbedPart = null;
            }
        }
        else if (grabbablePiece != null)
        {
            if (player.GetButtonDown("Grab"))
            {
                OnGrabPiece();
            }
        }
    }

    /// <summary>
    /// Called when the piece is grabbed.
    /// </summary>
    private void OnGrabPiece()
    {
        audioSource.Play();
        clawAnimator.SetBool("Attracting", true);

        grabbedPart = grabbablePiece;
        if (grabbedPart.beingGrabbed)
        {
            grabbedPart.lastRobotArm.grabbedPart = null;
            grabbedPart.lastRobotArm.OnReleasePiece();
        }

        grabbedPart.OnGrab(this, grabPoint);
        grabbablePiece = null;
    }

    /// <summary>
    /// Called when the piece is released, either by input, or by being taken by another player.
    /// </summary>
    public void OnReleasePiece()
    {
        clawAnimator.SetBool("Attracting", false);

        if(grabbedPart != null)
        {
            audioSource.Play();
            grabbedPart.OnRelease();
        }
    }

    /// <summary>
    /// Marks a piece as grabbable.
    /// </summary>
    public void MarkPartAsGrabbable(PartInstance part)
    {
        grabbablePiece = part;
    }

    /// <summary>
    /// Removes a piece from being a grabbable. 
    /// </summary>
    public void RemovePartAsGrabbable(PartInstance part)
    {
        // Fail-Safe to assure that the piece being removed is effectively the one the player thinks they're grabbing.
        if(grabbablePiece == part)
        {
            grabbablePiece = null;
        }
    }

    /// <summary>
    /// Spawns collision particles and does camera shake whenever two arms hit each other.
    /// </summary>
    public void SpawnCollisionParticles(Vector3 spawnPos)
    {
        Instantiate(sparks, spawnPos, Quaternion.identity).Play();
        FindObjectOfType<CameraController>().ShakeCamera(shakeDuration, shakeMagnitude);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        DoRotation();
    }

    /// <summary>
    /// Handles the rotations of the arms depending on physics.
    /// </summary>
    private void DoRotation()
    {
        float rot = rb.rotation;
        var jointMotor2D = hinge.motor;

        jointMotor2D.motorSpeed = (controlsInverted ? -horizontalInput : horizontalInput) * armRotationSpeed;
        hinge.motor = jointMotor2D;
    }

    /// <summary>
    /// Changes the sprite colours.
    /// </summary>
    public void ChangeSpriteColors(Color target)
    {
        foreach(SpriteRenderer renderer in allSprites)
        {
            renderer.material.SetColor("_Tint", target);
        }
    }

    /// <summary>
    /// Inverts the controls of the arms as an effect.
    /// </summary>
    public void InvertControls(bool isInverted)
    {
        controlsInverted = isInverted;
        invertedStatus.SetActive(isInverted);
    }

    /// <summary>
    /// Freezes the controls of the arms as an effect.
    /// </summary>
    public void FreezeControls(bool isFrozen)
    {
        controlsFrozen = isFrozen;
        frozenStatus.SetActive(isFrozen);
    }
}
