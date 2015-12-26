using UnityEngine;
using System.Collections;

//Attached to individual cubes that make up the Player GO Group
//When a cube is clicked, tells PivotController.cs to change the relative pivot point of the Player GO Group 

public class ClickPivot_V0_01 : MonoBehaviour
{
    public PivotController pivotController;

    private Vector3 clickPivot;

    void Start()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");//allows access to Player GO group
        if (playerGO != null)
        {
            pivotController = playerGO.GetComponent<PivotController>();
        }
        else
        {
            Debug.Log("Cannot find 'PivotController' script");
        }
    }

    void OnMouseDown()
    {
        //clickPivot = transform.localPosition + transform.parent.localPosition;//Adds local position of the shape relative to the Shapes GO group, plus the local position of Shapes relative to Player GO group (since the cubes are 2-deep in the hierarchy w.r.t. Player GO)
        clickPivot = transform.position - transform.parent.parent.position;//Finds the absolute location change of the Player GO due to moving the pivot 
        clickPivot = new Vector3(Mathf.Round(clickPivot.x), Mathf.Round(clickPivot.y), Mathf.Round(clickPivot.z));//makes sure shit stays on the grid positions, no rounding errors accumulatin' in MAH HOUSE

        pivotController.UpdateNewPivot(clickPivot);//tell pivotController to move various shit around
    }
}
