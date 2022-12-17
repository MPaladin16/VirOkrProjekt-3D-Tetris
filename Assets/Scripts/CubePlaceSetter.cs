using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlaceSetter : MonoBehaviour
{
    // Start is called before the first frame update
    public void setAllChildrenCubePosition() {
        foreach (var c in GetComponentsInChildren<CubeScript>())
        {
            c.SetCubePositions();
        }
    }
}
