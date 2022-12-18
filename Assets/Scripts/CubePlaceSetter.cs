using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlaceSetter : MonoBehaviour
{
    List<ColliderScript> activeColliders = new List<ColliderScript>();

    [SerializeField] public bool[] shapeRow1 = new bool[2];
    [SerializeField] public bool[] shapeRow2 = new bool[2];
    [SerializeField] public bool[] shapeRow3 = new bool[2];
    [SerializeField] public bool[] shapeRow4 = new bool[2];

    private int boolCount;
    private List<ColliderScript> lastShape = new List<ColliderScript>();

    void Start() {
        int boolCount = 0;
        foreach (bool b in shapeRow1) {
            if (b) {
                boolCount++;
            }
        }
        foreach (bool b in shapeRow2) {
            if (b) {
                boolCount++;
            }
        }
        foreach (bool b in shapeRow3) {
            if (b) {
                boolCount++;
            }
        }
        foreach (bool b in shapeRow4) {
            if (b) {
                boolCount++;
            }
        }
    }

    void Update() {
        foreach (ColliderScript cube in lastShape) {
            cube.GetOutlineRenderer().enabled = false;
        }

        lastShape = correctShape();
        if (lastShape.Count == 4) {
            foreach (ColliderScript cube in lastShape) {
                cube.GetOutlineRenderer().enabled = true;
            }
        }
    }

    private List<ColliderScript> correctShape() {
        List<ColliderScript> shape = new List<ColliderScript>();

        // this is commented if the current idea won't work very well
        // List<string> shapeBool = new List<string>();
        // // 2 = number of columns for shape
        // for (int i = 0; i < 2; i++) {
        //     if (shapeRow1[i]) {
        //         shapeBool.Add("0" + i);
        //     }
        //     if (shapeRow2[i]) {
        //         shapeBool.Add("1" + i);
        //     }
        //     if (shapeRow3[i]) {
        //         shapeBool.Add("2" + i);
        //     }
        //     if (shapeRow4[i]) {
        //         shapeBool.Add("3" + i);
        //     }
        // }

        // // TODO: Create list with 6 functions for each direction

        // List<ColliderScript> activeCollidersCopy = new List<ColliderScript>(activeColliders);
        // foreach (ColliderScript middleCollider in activeCollidersCopy) {
        //     shape = new List<ColliderScript>();
        //     List<string> shapeBoolCopy = new List<string>(shapeBool);

        //     foreach (ColliderScript otherCollider in activeCollidersCopy) {
        //         if (middleCollider == otherCollider) {
        //             continue;
        //         }

        //         // TODO: check all bools to see if they are in the correct position
        //         if (middleCollider.GetI)
        //     }
        // }

        if (activeColliders.Count == 4) {
            List<ColliderScript> activeCollidersCopy = new List<ColliderScript>(activeColliders);
            bool touching = false;
            // this only works because each tetris piece has 4 cubes
            bool touchingTwo = false;

            foreach (ColliderScript collider1 in activeCollidersCopy) {
                touching = false;

                foreach (ColliderScript collider2 in activeCollidersCopy) {
                    if (collider1 == collider2) {
                        continue;
                    }

                    int differenceRow = Mathf.Abs(collider1.GetRowIndex() - collider2.GetRowIndex());
                    int differenceI = Mathf.Abs(collider1.GetI() - collider2.GetI());
                    int differenceJ = Mathf.Abs(collider1.GetJ() - collider2.GetJ());
                    if ((differenceI == 1 || differenceJ == 1 || differenceRow == 1) &&
                        differenceI <= 1 && differenceJ <= 1 && differenceRow <= 1 &&
                        !(differenceI == 1 && differenceJ == 1) &&
                        !(differenceI == 1 && differenceRow == 1) &&
                        !(differenceJ == 1 && differenceRow == 1)) {
                            if (touching) {
                                touchingTwo = true;
                                break;
                            }

                            touching = true;
                            // TODO: Could add collider2 as well
                            shape.Add(collider1);
                    }
                }
                if (!touching) {
                    break;
                }
            }

            if (!touching || !touchingTwo) {
                return new List<ColliderScript>();
            }
        }

        return shape;
    }

    public void setAllChildrenCubePosition() {
        foreach (var c in GetComponentsInChildren<CubeScript>())
        {
            c.SetCubePositions();
        }
    }

    public void PutChildrenInNetwork() {
        if (lastShape.Count == 4) {
            CubeScript[] tetrisCubes = GetComponentsInChildren<CubeScript>();

            for (int i = 0; i < 4; i++) {
                ColliderScript colliderCube = lastShape[i];
                CubeScript tetrisCube = tetrisCubes[i];
                RowManager.PlaceCubeInNetwork(colliderCube, tetrisCube, this.gameObject);
            }
            
            foreach (ColliderScript cube in lastShape) {
                cube.GetOutlineRenderer().enabled = false;
            }
            Destroy(this.gameObject);
        }
    }
    
    // i think the on trigger enter and exit functions don't behave well,
    // so it sometimes bugs out the placement of tetris shapes in the network
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "BoxHitCollider") {
            ColliderScript collider = other.gameObject.GetComponent<ColliderScript>();
            if (!collider.GetFull() && !activeColliders.Contains(collider)) {
                activeColliders.Add(collider);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "BoxHitCollider") {
            ColliderScript collider = other.gameObject.GetComponent<ColliderScript>();
            if (activeColliders.Contains(collider)) {
                activeColliders.Remove(collider);
            }
        }
    }
}
