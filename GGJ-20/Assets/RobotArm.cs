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

    [Header("References")] 
    public Transform claw;
    
    private Player player;
    private SpriteRenderer ren;
    
    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponentInChildren<SpriteRenderer>();
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
        Vector2 size = ren.size;
        size.x = size.x + (v * armExtendSpeed * Time.deltaTime);
        ren.size = size;
        claw.Translate(new Vector3(v*armExtendSpeed*Time.deltaTime, 0, 0));
        
        // Rotate arms
        float h = player.GetAxis("Horizontal");
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z += h * armRotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);
        
        // Grab part
    }
}
