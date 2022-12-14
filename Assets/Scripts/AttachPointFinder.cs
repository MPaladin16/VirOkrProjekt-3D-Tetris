using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPointFinder : MonoBehaviour
{
    public static Action<Vector3> onNewAttachPoint;

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 6;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log(hit.transform.position);
            onNewAttachPoint?.Invoke(hit.transform.position);
        }
    }
}
