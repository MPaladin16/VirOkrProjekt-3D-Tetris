using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TetrisPieceScript : MonoBehaviour
{
    [Header("Explosion details")]
    public int cubesPerAxis = 3;
    public float radius = 0.5f;
    public float force = 15f;

    private Quaternion _startRotation;

    // drag details
    private bool dragChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        _startRotation = transform.rotation;
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

            GameObject gameManager = GameObject.Find("GameManager");
            gameManager.GetComponent<GameManager>().DroppedOrDespawned();
        }
    }

    public void Explode()
    {
        foreach (Transform childCube in this.transform)
        {
            for (int x = 0; x < cubesPerAxis; x++)
            {
                for (int y = 0; y < cubesPerAxis; y++)
                {
                    for (int z = 0; z < cubesPerAxis; z++)
                    {
                        try
                        {
                            CreateMiniCube(childCube, new Vector3(x, y, z));
                        }
                        catch (Exception e)
                        {
                        }
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

        Vector3 firstMiniCubePos = transform.localPosition - transform.localScale / 2 + miniCube.transform.localScale / 2;
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
