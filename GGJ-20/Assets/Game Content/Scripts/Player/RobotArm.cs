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
    
    [Header("Input")]
    public float armExtendSpeed = 5;
    public float armRotationSpeed = 5;
    public Vector2 armClampedSize = new Vector2(1, 14);

    [Header("Grab Options")] 
    public LayerMask mask;

    [Header("CHAOS")] 
    public bool stretchInverted;
    public bool rotationInverted;
    
    private Transform clawBase;
    private Transform arm;
    private Transform claw;
    private Transform grabArea;
    
    private Player player;
    private Animator clawAnimator;
    private SpriteRenderer armRender;
    private BoxCollider2D armCollider;
    private BoxCollider2D grabCollider;
    private Rigidbody2D rb;
    
    // Input vars TODO: are these necessary? Can the input work on FixedUpdate?
    private float h, v;
    private bool isGrabbing;
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

        armRender = arm.GetComponent<SpriteRenderer>();
        armCollider = arm.GetComponent<BoxCollider2D>();
        grabCollider = grabArea.GetComponent<BoxCollider2D>();
        rb = clawBase.GetComponent<Rigidbody2D>();
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

        // Grab part
        if (player.GetButtonDown("Grab"))
        {
            // Try to Grab 
            Collider2D hit = Physics2D.OverlapBox((Vector2)grabArea.transform.position + new Vector2(grabCollider.size.y / 2, 0), new Vector2(grabCollider.size.y, grabCollider.size.x), grabArea.rotation.eulerAngles.z, mask);
            if (hit)
            {
                if(hit.tag.Equals("Part"))
                {
                    clawAnimator.SetBool("Attracting", true);
                    grabbedPart = hit.GetComponent<PartInstance>();
                    grabbedPart.OnGrab(grabArea);
                    
                    isGrabbing = true;
                }
            }
        }

        if (isGrabbing)
        {
            if (player.GetButtonUp("Grab"))
            {
                // Stop Grabbing
                clawAnimator.SetBool("Attracting", false);
                grabbedPart.OnRelease();
                grabbedPart = null;
                isGrabbing = false;
            }
        }
    }

    private void FixedUpdate()
    {
        DoRotation();
    }

    private void DoRotation()
    {
        float rot = rb.rotation;
        rot += (rotationInverted? -h : h) * armRotationSpeed * Time.deltaTime;
        rb.rotation = rot;
    }

    /// <summary>
    /// Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Transform clawBase = transform.Find("ClawBase");
        Transform arm = clawBase.Find("Arm");
        Transform claw = arm.Find("Claw");
        Transform grabArea = claw.Find("ClawHead/ClawMagnet/Grab Area");
        BoxCollider2D grabCollider = grabArea.GetComponent<BoxCollider2D>();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((Vector2)grabArea.transform.position + new Vector2(grabCollider.size.y / 2, 0), new Vector2(grabCollider.size.y, grabCollider.size.x));
    }
}
