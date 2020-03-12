﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zone where robots and where the validation of patterns happens.
/// </summary>
public class AssemblyZone : MonoBehaviour
{
    [Header("Transform")]
    public Transform legsTransform;
    public Transform bodyTransform;
    public Transform headTransform;

    [Header("Assembled Parts")]
    public PartInstance assembledLegs;
    public PartInstance assembledBody;
    public PartInstance assembledHead;

    public AudioSource kaching;
    public AudioSource weld;

    private ParticleSystem particles;
    private BoxCollider2D bc;
    private GameObject legsShadow;
    private Animator assemblyAnimator;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        particles = transform.Find("Robot Repair Particles").GetComponent<ParticleSystem>();

        assemblyAnimator = transform.parent.GetComponent<Animator>();
        legsShadow = legsTransform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Attaches a part to the collider.
    /// </summary>
    public void AttachPart(PartInstance part)
    {
        Transform targetTransform = null;
        if(assembledLegs == null)
        {
            assembledLegs = part;
            targetTransform = legsTransform;
            legsShadow.SetActive(true);
        }
        else if (assembledBody == null)
        {
            assembledBody = part;
            targetTransform = bodyTransform;
        }
        else if (assembledHead == null)
        {
            assembledHead = part;
            targetTransform = headTransform;
        }

        ValidateAssembly();
        AttachPartToTransform(part, targetTransform);
    }

    /// <summary>
    /// Attachs a part to a transform.
    /// </summary>
    public void AttachPartToTransform(PartInstance part, Transform point)
    {
        part.transform.parent = point;
        part.transform.position = point.position;
    }

    /// <summary>
    /// Validates the pieces that have been put into assembly.
    /// </summary>
    public void ValidateAssembly()
    {
        // Checks if the player has done any mistake.
        Pattern pattern = GameManager.Instance.targetPattern;
        if (assembledLegs != null)
        {
            if (assembledLegs.part != pattern.legsPart)
            {
                ThrowAll();
                return;
            }
        }
        else
        {
            return;
        }
        GameManager.Instance.Canvas.CheckmarkLegsPart();

        if (assembledBody != null)
        {
            if (assembledBody.part != pattern.bodyPart)
            {
                ThrowAll();
                return;
            }
        }
        else
        {
            return;
        }
        GameManager.Instance.Canvas.CheckmarkBodyPart();

        if (assembledHead != null)
        {
            if (assembledHead.part != pattern.headPart)
            {
                ThrowAll();
                return;
            }
        }
        else
        {
            return;
        }
        GameManager.Instance.Canvas.CheckmarkHeadPart();

        StartCoroutine(ValidAssembly());
    }

    /// <summary>
    /// Confirms a sucessfull assembly and gives assembly a small delay before submitting. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator ValidAssembly()
    {
        bc.enabled = false;
        assemblyAnimator.SetTrigger("Accept");
        yield return new WaitForSeconds(0.1f);
        particles.Play();
        weld.Play();
        yield return new WaitForSeconds(0.1f);

        // Failsafe for when the player finishes the bot as time ends.
        if (assembledHead == null || assembledBody == null || assembledLegs == null)
        {
            bc.enabled = true;
            yield break;
        }

        assembledHead.ChangeSpritesToFixed();
        assembledBody.ChangeSpritesToFixed();
        assembledLegs.ChangeSpritesToFixed();

        // Scores
        switch(assembledHead.playerId)
        {
            case 0:
                GameManager.Instance.player1Contribution++;
                break;
            case 1:
                GameManager.Instance.player2Contribution++;
                break;
        }

        switch (assembledBody.playerId)
        {
            case 0:
                GameManager.Instance.player1Contribution++;
                break;
            case 1:
                GameManager.Instance.player2Contribution++;
                break;
        }

        switch (assembledLegs.playerId)
        {
            case 0:
                GameManager.Instance.player1Contribution++;
                break;
            case 1:
                GameManager.Instance.player2Contribution++;
                break;
        }

        GameManager.Instance.ConfirmAssembly();
        yield return new WaitForSeconds(1.5f);

        Destroy(assembledLegs.gameObject);
        assembledLegs = null;
        legsShadow.SetActive(false);

        Destroy(assembledBody.gameObject);
        assembledBody = null;

        Destroy(assembledHead.gameObject);
        assembledHead = null;

        bc.enabled = true;
    }

    /// <summary>
    /// Throws away all parts from the assembly line.
    /// </summary>
    public void ThrowAll()
    {
        GameManager.Instance.FailAssembly();

        assemblyAnimator.SetTrigger("Refuse");
        if (assembledLegs == null)
        {
            return;
        }

        legsShadow.SetActive(false);
        assembledLegs.ThrowPiece();
        assembledLegs = null;

        if (assembledBody == null)
        {
            return;
        }

        assembledBody.ThrowPiece();
        assembledBody = null;

        if (assembledHead == null)
        {
            return;
        }

        assembledHead.ThrowPiece();
        assembledHead = null;
    }
}
