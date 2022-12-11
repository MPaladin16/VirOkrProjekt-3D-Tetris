using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<ColliderScript> ColliderScriptList;

    private List<List<bool>> markerList;
    private int ind1 = 0;
    private int ind2 = 0;
    private int ind3 = 0;
    public void UpdateStatus()
    {
        ind1 = 0;
        ind2 = 0;
        foreach (ColliderScript marker in ColliderScriptList)
        {
            if (marker.GetFull())
            {
                markerList[ind1][ind2] = true;
            }
            else
            {
                markerList[ind1][ind2] = false;
            }
            ind2++;
            if (ind2 == 16)
            {
                ind1++;
                ind2 = 0;
            }
        }
        ClearRows();
    }

    public void ClearRows()
    {
        ind1 = 0;
        ind2 = 0;
        ind3 = 0;
        int rowFull = 0;
        if (markerList[ind1][ind2])
        {
            rowFull++;
        }
        else
        {
            rowFull = 0;
        }
        ind2++;
        if (rowFull == 16)
        {
            while (rowFull > 0)
            {
                ColliderScriptList[ind1 * 16 + ind3].SetEmpty();
                ind3++;
                rowFull--;
            }
            ind3 = 0;
        }

        if (ind2 == 16)
        {
            ind1++;
            ind2 = 0;
            rowFull = 0;
        }
    }
}