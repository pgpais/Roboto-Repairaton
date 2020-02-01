using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A circular conveyer belt tracks parts around a semi-circle.
/// </summary>
public class CircularConveyerBelt : MonoBehaviour
{
    [Header("Circular Anchors")]
    public float radius;
    public int semiCircle = 1;

    /// <summary>
    /// Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
