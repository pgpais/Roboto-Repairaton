using System.Collections;
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
    public PartInstance legsPart;
    public PartInstance bodyPart;
    public PartInstance headPart;

    private GameObject legsShadow;
    private Animator assemblyAnimator;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        assemblyAnimator = transform.parent.GetComponent<Animator>();
        legsShadow = legsTransform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Attaches a part to the collider.
    /// </summary>
    public void AttachPart(PartInstance part)
    {
        Transform targetTransform = null;
        if(legsPart == null)
        {
            legsPart = part;
            targetTransform = legsTransform;
            legsShadow.SetActive(true);
        }
        else if (bodyPart == null)
        {
            bodyPart = part;
            targetTransform = bodyTransform;
        }
        else if (headPart == null)
        {
            headPart = part;
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
        if (legsPart != null && legsPart.part.partType != PartType.Legs)
        {
            ThrowAll();
            return;
        }

        if (bodyPart != null && bodyPart.part.partType != PartType.Body)
        {
            ThrowAll();
            return;
        }

        // Validates if the robot is the robot order.
        if(headPart != null)
        {
            Pattern pattern = GameManager.Instance.targetPattern;
            if (legsPart.part != pattern.legsPart)
            {
                ThrowAll();
                return;
            }

            if (bodyPart.part != pattern.bodyPart)
            {
                ThrowAll();
                return;
            }

            if (headPart.part != pattern.headPart)
            {
                ThrowAll();
                return;
            }

            StartCoroutine(ConfirmAssembly());
        }
    }

    /// <summary>
    /// Confirms a sucessfull assembly and gives assembly a small delay before submitting. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator ConfirmAssembly()
    {
        assemblyAnimator.SetTrigger("Accept");
        GameManager.Instance.ConfirmAssembly();
        yield return new WaitForSeconds(0.6f);

        Destroy(legsPart.gameObject);
        legsPart = null;
        legsShadow.SetActive(false);

        Destroy(bodyPart.gameObject);
        bodyPart = null;

        Destroy(headPart.gameObject);
        headPart = null;
    }

    /// <summary>
    /// Throws away all parts from the assembly line.
    /// </summary>
    public void ThrowAll()
    {
        assemblyAnimator.SetTrigger("Refuse");
        if (legsPart == null)
        {
            return;
        }

        legsShadow.SetActive(false);
        legsPart.ThrowPiece();
        legsPart = null;

        if (bodyPart == null)
        {
            return;
        }

        bodyPart.ThrowPiece();
        bodyPart = null;

        if (headPart == null)
        {
            return;
        }

        headPart.ThrowPiece();
        headPart = null;
    }
}
