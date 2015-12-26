using UnityEngine;
using System.Collections;


//CURRENTLY (11/25/15) NOT IN USE
//This script will be on a lot of objects
//Determines (via public bools) which other objects the script-holder has information from

public class Inherits_V0_01 : MonoBehaviour {
    public bool RPos, GPos, BPos, YPos, PlayerPos;  //Determines what information an object should inherit from other objects
    private GameObject Rgo, Ggo, Bgo, Ygo, Playergo;                                                //Public so easily changeable
   // private Vector3 RVec, GVec, YVec, PlayerVec;    //Variables to hold said info
    
	// Use this for initialization
	void Start () {
        if (RPos)
        {
            Rgo = GameObject.FindWithTag("RTag");
        }

        if (GPos)
        {
            Ggo = GameObject.FindWithTag("GTag");
        }

        if (BPos)
        {
            Bgo = GameObject.FindWithTag("BTag");
        }

        if (YPos)
        {
            Ygo = GameObject.FindWithTag("YTag");
        }

        if (PlayerPos)
        {
            Playergo = GameObject.FindWithTag("Player");
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
}
