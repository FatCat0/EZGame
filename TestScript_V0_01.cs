using UnityEngine;
using System.Collections;
//Used for whatever
public class TestScript_V0_01 : MonoBehaviour
{
    public GameObject otherCube;

    // Use this for initialization
    void Start()
    {
        otherCube = GameObject.FindWithTag("otherGuy");
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion difference = Quaternion.FromToRotation(transform.forward, otherCube.transform.forward);
        print(difference);
    }
}
