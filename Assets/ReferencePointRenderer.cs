using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePointRenderer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        foreach(Transform point in transform)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(point.position, 1.2f);
        }
    }
}
