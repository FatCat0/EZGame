using UnityEngine;
using System.Collections;
//Actually moves the player

    //NOTES: When doing rotation pivot change 

//Attached to the Player game object group
public class PlayerMover : MonoBehaviour
{
    public float moveSpeed;//How quickly moves happen(~inverse number of seconds a move lasts)
    public float timeMoveStart; //used to keep track of move animation
    public float timeRotStart; //used to keep track of rotation animation
    public float rotSpeed;//How quickly rotations happen(same)
    public float rotPastDeg;//How far beyond the target rotation the animation aims



    //QUEUEpublic Quaternion rotFinal;//where the rotation will end up when animation is done/current rotation if not rotating
    //QUEUEpublic Quaternion rotPast;//target rotation of Slerp. This is a little bit "past" the target rotation (rotFinal) so that Slerp more smoothly moves toward the target position vs. slowing down a lot toward the end


    private GameObject moveToGO;//Access the MoveTo object
    private GameObject prevTransGO, pastTransGO;//prev has transform parameters for undoing moves (previous), past (i.e. beyond) has them for targeting moves (in PlayerMove, Lerps point a little bit beyond the goal position so the animation happens a bit more smoothly)

    private PlayerController playerController;//So I don't have to use GetComponent a bazillion times (even though I already did)

    void Start()
    {
        moveToGO = GameObject.FindWithTag("MoveTo");
        prevTransGO = GameObject.FindWithTag("MovePrev");//Allows script to modify MovePrev GO
        pastTransGO = GameObject.FindWithTag("MovePast");//Allows script to modify MovePast GO
        playerController = moveToGO.GetComponent<PlayerController>();
    }

    void Update()
    {
        //Update player position this frame if need be
        if (playerController.moving)//REWRITE AS COROUTINE
        {
            if (Vector3.Distance(transform.position, pastTransGO.transform.position) > Vector3.Distance(pastTransGO.transform.position, moveToGO.transform.position))//If the current PlayerGO position hasn't yet passed the MoveTo (AKA goal) position
            {
                //transform Player a bit closer to MoveTo (not quite linearly since it is aimed past MoveTo)
                float newX = Mathf.Lerp(transform.position.x, pastTransGO.transform.position.x, moveSpeed * (Time.time - timeMoveStart));
                float newY = 0f;//Mathf.Lerp(transform.position.y, pastTransGO.transform.position.y, moveSpeed * (Time.time - timeMoveStart));UNNECESSARY UNLESS GOING 3D
                float newZ = Mathf.Lerp(transform.position.z, pastTransGO.transform.position.z, moveSpeed * (Time.time - timeMoveStart));
                transform.position = new Vector3(newX, newY, newZ);
            }

            else 
            {
                transform.position = moveToGO.transform.position;//Make sure the player is on top of MoveTo at end of move 
                playerController.moving = false;
                playerController.unmoving = false;
                //Might do additional garbage cleanup here (playerController).
            }
        }

        //Update player rotation this frame if need be
        if (playerController.rotating)
        {
            if (Quaternion.Angle(transform.rotation, pastTransGO.transform.rotation) > Quaternion.Angle(moveToGO.transform.rotation, pastTransGO.transform.rotation))//If current rotation has not yet surpassed MoveTo (AKA goal) rotation
            {
                RotateStep();//Rotate a little closer toward it
            }
            else
            {
                transform.rotation = moveToGO.transform.rotation;//Make sure the rotation ends exactly where we want it
                playerController.rotating = false;
                playerController.unrotating = false;
                //Might do additional garbage cleanup here (playerController)
            }

        }

        
    }
    
    public static void PlayerRotate(Quaternion targetRot)
    {
            
    }
    IEnumerator Rotater(Quaternion targetRot, float rotSpeed)
    {
        Quaternion rotPast = targetRot * Quaternion.Euler(0, rotPastDeg * GF.ClockWise(transform.rotation, targetRot, 1), 0);
        float timeRotStart = Time.time;
        while (Quaternion.Angle(transform.rotation, rotPast) > Quaternion.Angle(targetRot, rotPast))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, (Time.time - timeRotStart) * rotSpeed);
            yield return null;
        }
        transform.rotation = targetRot;
    }

    void RotateStep()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, pastTransGO.transform.rotation, (Time.time - timeRotStart) * rotSpeed);
    }

    /*int Compare(float a, float b)
    {
        if (a > b)
        {
            return 1;
        }
        else if (a < b)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }*/

    /* public void Test()
     {
         transform.rotation = Quaternion.Slerp(transform.rotation, rotFinal, Time.deltaTime * rotSpeed);
         //playerController().test = 0;
     }*/
}
