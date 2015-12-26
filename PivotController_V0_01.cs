using UnityEngine;
using System.Collections;

//ClickPivot tells this script when and how to update the Player GO and the constituent cubes to create a new pivot point (without net movement of the cubes in the world space)

//Attached to Player GO

public class PivotController_V0_01 : MonoBehaviour
{
    public GameObject moversGO;


    Vector3 newPivot;
    public Vector3 oldPivot;

    // Use this for initialization
    void Start()
    {
        moversGO = GameObject.FindGameObjectWithTag("Movers");
    }

    // Update is called once per frame
    void Update()
    {


    }



    public void UpdateNewPivot(Vector3 clickPivot)
    {
        transform.Translate(clickPivot, Space.World);//Move Player GO to accomodate new pivot point

        transform.Find("Shapes").Translate(-clickPivot, Space.World);//Move shape holder GO (Shapes) so that the player's cubes don't move around when the pivot is changed

        moversGO.transform.Translate(clickPivot, Space.World);//Move the move GO container so that the movers stay lined up with the player's new position
        /*
        foreach (Transform child in transform.Find("Shapes"))//Move the consituent shapes appropriately inside of Player GO s.t. they are still in the same place on the screen and now relatively shifted so that the new pivot point affects how they rotate
        {
            child.Translate(-clickPivot);
        }
		//*/
       

    }


}
