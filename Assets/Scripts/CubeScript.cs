using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeScript : MonoBehaviour
{
    private GameObject _collidedObject;
    public void SetCubePositions()
    {
        //pivot mora bit sredina da ovo radi
        this.gameObject.transform.position = _collidedObject.transform.position;
        this.gameObject.transform.rotation = _collidedObject.transform.rotation;
        _collidedObject.GetComponent<ColliderScript>().SetFull(this.gameObject);
    }
    public void OnTriggerEnter(Collider other)

    {
        //bitno da i ruka i kocka imaju collidere tako da se trigger zove sam s placeholderima koji su triggeri
        _collidedObject = other.gameObject;
    }

}