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

    private Transform arm;
    private Transform claw;
    private Transform clawGear;
    
    private Player player;
    private SpriteRenderer armRender;
    private BoxCollider2D armCollider;
    
    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        arm = transform.Find("Arm");
        claw = arm.Find("Claw");
        clawGear = claw.Find("ClawHead/ClawGear");

        armRender = arm.GetComponent<SpriteRenderer>();
        armCollider = arm.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        // Extend and shrink arm
        float v = player.GetAxis("Vertical");
        Vector2 size = armRender.size;
        size.x = size.x + (v * armExtendSpeed * Time.deltaTime);
        size.x = Mathf.Clamp(size.x, armClampedSize.x, armClampedSize.y);
        armRender.size = size;

        Vector2 clawPosition = claw.transform.localPosition;
        clawPosition.x = size.x + 0.6f;
        claw.transform.localPosition = clawPosition;

        // Rotate Arms
        float h = player.GetAxis("Horizontal");
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z += h * armRotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);

        clawGear.Rotate(new Vector3(0, 0, h * armRotationSpeed * Time.deltaTime));

        // Update Collider
        Vector2 spriteSize = armRender.size;
        armCollider.size = spriteSize;
        armCollider.offset = new Vector2(spriteSize.x / 2, 0);

        // Grab part
    }
}
