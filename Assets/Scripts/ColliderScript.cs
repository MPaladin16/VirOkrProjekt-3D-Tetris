using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColliderScript : MonoBehaviour
{
    [SerializeField] bool full;

    private GameObject _parentCube;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetFull(GameObject go)
    {
        full = true;
        _parentCube = go;
    }
    public void SetEmpty()
    {
        full = false;
        Destroy(_parentCube);
    }

    public bool GetFull()
    {
        return full;
    }
}