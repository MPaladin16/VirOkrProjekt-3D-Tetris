using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisPieceScript : MonoBehaviour
{
    //explosion details
    public int cubesPerAxis = 3;
    public float radius = 0.5f;
    public float force = 15f;

    // drag details
    private bool dragChanged = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //sudar s podom -> eksplozija
        if (collision.gameObject.layer == 3)
        {
            force *= collision.relativeVelocity.y;
            Invoke("Explode", 0);
        }
    }

    private void Explode()
    {
        foreach(Transform childCube in this.transform)
        {
            for(int x = 0; x < cubesPerAxis; x++) {
                for (int y = 0; y < cubesPerAxis; y++) {
                    for (int z = 0; z < cubesPerAxis; z++) {
                        CreateMiniCube(childCube, new Vector3(x, y, z));
                    }
                }
            }
        }

        Destroy(this.gameObject);
    }

    private void CreateMiniCube(Transform childCube, Vector3 pos)
    {
        GameObject miniCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer rd = miniCube.GetComponent<Renderer>();
        rd.material = childCube.GetComponent<Renderer>().material;

        //ako tetris dio padne na komadice
        miniCube.layer = 3;

        miniCube.transform.localScale = this.transform.localScale / cubesPerAxis;

        Vector3 firstMiniCubePos = transform.position - transform.localScale / 2 + miniCube.transform.localScale / 2;
        miniCube.transform.position = firstMiniCubePos + Vector3.Scale(pos, miniCube.transform.localScale);

        Rigidbody rb = miniCube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(force, transform.position, radius);

        Destroy(miniCube, 3.5f);
    }

    public void changeDrag()
    {
        if (!dragChanged)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.drag = 0;
            dragChanged = true;
        }
    }

}
