using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

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

    private Transform clawBase;
    private Transform arm;
    private Transform claw;
    private Transform clawGear;
    
    private Player player;
    private SpriteRenderer armRender;
    private BoxCollider2D armCollider;
    private BoxCollider2D clawCollider;
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
        clawGear = claw.Find("ClawHead/ClawGear");

        armRender = arm.GetComponent<SpriteRenderer>();
        armCollider = arm.GetComponent<BoxCollider2D>();
        clawCollider = claw.GetComponent<BoxCollider2D>();
        rb = clawBase.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        // Extend and shrink arm
        v = player.GetAxis("Vertical");
        Vector2 size = armRender.size;
        size.x = size.x + (v * armExtendSpeed * Time.deltaTime);
        size.x = Mathf.Clamp(size.x, armClampedSize.x, armClampedSize.y);
        armRender.size = size;

        Vector2 clawPosition = claw.transform.localPosition;
        clawPosition.x = size.x + 0.6f;
        claw.transform.localPosition = clawPosition;

        // Rotate Arms
        h = player.GetAxis("Horizontal");
        /*Vector3 rot = transform.rotation.eulerAngles;
        rot.z += h * armRotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);

        clawGear.Rotate(new Vector3(0, 0, h * armRotationSpeed * Time.deltaTime));*/

        // Update Collider
        Vector2 spriteSize = armRender.size;
        armCollider.size = spriteSize;
        armCollider.offset = new Vector2(spriteSize.x / 2, 0);

        // Grab part
        if (player.GetButtonDown("Grab"))
        {
            // Try to Grab
            Collider2D hit = Physics2D.OverlapBox(claw.position, clawCollider.size, claw.rotation.eulerAngles.z, mask);
            if (hit)
            {
                if(hit.tag.Equals("Part"))
                {
                    grabbedPart = hit.GetComponent<PartInstance>();
                    grabbedPart.OnGrab(claw);
                    
                    isGrabbing = true;
                }
            }
        }

        if (isGrabbing)
        {
            if (player.GetButtonUp("Grab"))
            {
                // Stop Grabbing
                grabbedPart.OnRelease();
                grabbedPart = null;
                isGrabbing = false;
            }
        }
    }

    private void FixedUpdate()
    {
        DoRotation();
        //DoStretch();
        //DoGrab();
    }

    private void DoGrab()
    {
        throw new NotImplementedException();
    }

    private void DoStretch()
    {
        throw new NotImplementedException();
    }

    private void DoRotation()
    {
        float rot = rb.rotation;
        rot += h * armRotationSpeed * Time.deltaTime;
        rb.rotation = rot;
    }
}
