using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class GenerateTetris : MonoBehaviour
{
    [Tooltip("Spawn tetris piece every x seconds")]
    [SerializeField] float seconds;

    //ako zelimo vise spawnera koji rade periodicki
    [Tooltip("Start spawning pieces after startOffset seconds")]
    [SerializeField] float startOffset;

    public GameObject[] tetrisPieces;

    //index zadnjeg stvorenog dijela i broj ponavljanja
    //svaki se moze pojaviti max 2 puta za redom
    (int index, int reps) lastSpawned;

    // Start is called before the first frame update
    void Start()
    {
        //initial
        lastSpawned.index = -1;
        lastSpawned.reps = -1;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyRedundantObjects();
    }

    void SpawnTertris()
    { 
        int index = UnityEngine.Random.Range(0,tetrisPieces.Length);

        if(lastSpawned.index == index && lastSpawned.reps >= 2)
        {
            do
            {
                index = UnityEngine.Random.Range(0, tetrisPieces.Length);
            } while (index == lastSpawned.index);

            lastSpawned.index = index;
            lastSpawned.reps = 1;
        }

        else if(lastSpawned.index == index)
        {
            lastSpawned.reps++;
        }

        else
        {
            lastSpawned.index = index;
            lastSpawned.reps = 1;
        }

        Instantiate(tetrisPieces[index], this.gameObject.transform.position, this.gameObject.transform.rotation);
    }

    private void DestroyRedundantObjects()
    {
        GameObject go = GameObject.Find("Cube");
        Destroy(go);
    }

    public void startSpawning()
    {
        InvokeRepeating("SpawnTertris", startOffset, seconds);
    }

}