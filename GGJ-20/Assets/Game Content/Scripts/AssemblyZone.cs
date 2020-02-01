using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zone where robots and where the validation of patterns happens.
/// </summary>
public class AssemblyZone : MonoBehaviour
{
    [SerializeField]
    public Transform attachPoint = null;


    // Start is called before the first frame update
    void Start()
    {
        attachPoint = transform.Find("Assembly Attach Point");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttachPart(BoxCollider2D partCol)
    {
        attachPoint.Translate(0, partCol.size.y, 0);
    }
}
