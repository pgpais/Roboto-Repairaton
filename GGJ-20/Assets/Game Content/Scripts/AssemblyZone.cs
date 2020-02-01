using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyZone : MonoBehaviour
{
    [SerializeField]
    private Transform attachPoint;

    public Transform AttachPoint => attachPoint;
    // Start is called before the first frame update
    void Start()
    {
        attachPoint = transform.Find("Assembly Attach Point");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
