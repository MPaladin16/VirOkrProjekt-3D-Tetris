using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<ColliderScript> ColliderScriptList;

    public static Action onRowCleared;

    private List<List<bool>> markerList;
    private int ind1 = 0;
    private int ind2 = 0;
    private int ind3 = 0;

    private void OnEnable()
    {
        CubePlaceSetter.onShapeAdded += UpdateStatus;
        GameManager.onGameStoppedClearBox += ClearBox;
    }

    private void OnDisable()
    {
        CubePlaceSetter.onShapeAdded -= UpdateStatus;
        GameManager.onGameStoppedClearBox -= ClearBox;
    }

    private void Start()
    {
        markerList = new List<List<bool>>();
        for (int i = 0; i < 10; i++) {
            List<bool> list = new List<bool>();
            for (int j = 0; j < 16; j++) {
                list.Add(false);
            }
            markerList.Add(list);
        }
    }

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
        for (ind1 = 0; ind1 < 10; ind1++) {
            bool rowFull = true;
            
            for (ind2 = 0; ind2 < 16; ind2++) {
                if (!markerList[ind1][ind2]) {
                    rowFull = false;
                    break;
                }
            }

            if (rowFull) {
                for (int i = 0; i < 16; i++) {
                    ColliderScriptList[ind1 * 16 + i].SetEmpty();
                }

                // tu se dodaje bod na screen za unisten cijeli red
                onRowCleared?.Invoke();
            }
        }
    }

    private void ClearBox()
    {
        ind1 = 0;
        ind2 = 0;
        foreach (ColliderScript marker in ColliderScriptList)
        {
            markerList[ind1][ind2] = false;
            ColliderScriptList[ind1 * 16 + ind2].SetEmpty();
            ind2++;
            if (ind2 == 16)
            {
                ind1++;
                ind2 = 0;
            }
           
        }
    }

    public static void PlaceCubeInNetwork(ColliderScript colliderCube, CubeScript tetrisCube, GameObject tetrisShape) {
        GameObject clone = Instantiate(tetrisCube.gameObject, colliderCube.gameObject.transform.position, colliderCube.gameObject.transform.rotation);
        clone.transform.localScale = tetrisShape.transform.localScale;
        colliderCube.SetFull(clone);
    }

    // TODO: Falling blocks (maybe add an id to CubeScript, and sort
    // shapes here with a map/dictionary?), clearing rows
}