using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Serialization;

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

    [Header("CHAOS")] 
    public bool stretchInverted;
    public bool rotationInverted;

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
    private BoxCollider2D armCollider;
    private Rigidbody2D rb;
    private CameraController cameraController;
    private AudioSource audioSource;
    private GameObject invertedStatus;

    private HingeJoint2D hinge; // TODO: this might not work, if something breaks, prolly this
    
    // Input vars TODO: are these necessary? Can the input work on FixedUpdate?
    private float h, v;
    private PartInstance grabbedPart;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        clawBase = transform.Find("ClawBase");
        arm = clawBase.Find("Arm");
        claw = arm.Find("Claw");
        clawAnimator = claw.Find("ClawHead/ClawMagnet").GetComponentInChildren<Animator>();
        grabArea = claw.Find("ClawHead/ClawMagnet/Grab Area");
        grabPoint = grabArea.transform.GetChild(0);
        invertedStatus = claw.Find("ClawHead/Inverted Status").gameObject;

        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        audioSource = GetComponent<AudioSource>();
        armRender = arm.GetComponent<SpriteRenderer>();
        armCollider = arm.GetComponent<BoxCollider2D>();
        rb = GetComponentInChildren<Rigidbody2D>();
        hinge = GetComponentInChildren<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GameState != GameState.Running)
        {
            return;
        }

        GetInput();
    }

    void GetInput()
    {
        // Extend and shrink arm
        v = player.GetAxis("Vertical");
        Vector2 size = armRender.size;
        size.y = size.y + ((stretchInverted? -v : v) * armExtendSpeed * Time.deltaTime);
        size.y = Mathf.Clamp(size.y, armClampedSize.x, armClampedSize.y);
        armRender.size = size;

        Vector2 clawPosition = claw.transform.localPosition;
        clawPosition.y = size.y - 1.7f;
        claw.transform.localPosition = clawPosition;

        // Rotate Arms
        h = player.GetAxis("Horizontal");

        // Update Collider
        Vector2 spriteSize = armRender.size;
        spriteSize.x *= 0.3f;
        armCollider.size = spriteSize;
        armCollider.offset = new Vector2(0, spriteSize.y / 2);

        if (grabbedPart != null)
        {
            if (player.GetButtonUp("Grab"))
            {
                // Stop Grabbing
                // TODO: Play POP Audio
                audioSource.Play();
                clawAnimator.SetBool("Attracting", false);
                grabbedPart.OnRelease();
                grabbedPart = null;
            }
        }
    }

    /// <summary>
    /// Allows the player to pick stuff up using the Grab Area Collider.
    /// </summary>
    public void ColliderDetected(GameObject other)
    {
        if(grabbedPart != null)
        {
            return;
        }

        // Grab part
        if (player.GetButtonDown("Grab"))
        {
            // Try to Grab 
            {
                if (other.tag.Equals("Part"))
                {
                    // TODO: Play POP Audio
                    audioSource.Play();
                    clawAnimator.SetBool("Attracting", true);
                    grabbedPart = other.GetComponent<PartInstance>();
                    grabbedPart.OnGrab(grabPoint);
                    grabbedPart.playerId = playerId;
                }
            }
        }
    }

    public void SpawnCollisionParticles(Vector3 spawnPos)
    {
        Instantiate(sparks, spawnPos, Quaternion.identity).Play();
        cameraController.ShakeCamera(shakeDuration, shakeMagnitude);
    }


    private void FixedUpdate()
    {
        DoRotation();
    }

    private void DoRotation()
    {
        float rot = rb.rotation;
        var jointMotor2D = hinge.motor;
        jointMotor2D.motorSpeed = (rotationInverted ? -h : h) * armRotationSpeed;
        hinge.motor = jointMotor2D;
        //rot += (rotationInverted? -h : h) * armRotationSpeed * Time.deltaTime;
        //rb.rotation = rot;
    }

    public void InvertControls(bool isInverted)
    {
        invertedStatus.SetActive(isInverted);
    }
}
