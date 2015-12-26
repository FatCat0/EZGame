using UnityEngine;
using System.Collections;
//Receives and processes input related to moving the player (translation, not rotation)
//Pretty sure this prioritizes horizontal movement over vertical (i.e. if the player is trying to move both at once horizontal will result)
//Certain that horizontal movement prioritized over vertical in the queueing. Going to stay this way so long as only one extra input can be queued. I think that handling 2 queued inputs might be enough in the long run (and disallow overwrite; you pressed it you meant it)

//Also receives information about collisions from Player GO's and handles them in LateUpdate

//Attached to the MoveTo object

    //GLITCH: If one tries to rotate (move?) while unrotating, it is queued up but not executed when the unrotate is complete. Not terrible, but then when another move is input the queued move comes into play too
    //Will probably let this slide until I rewrite as coroutines as there are other movement glitches evoked from collisions (0 degree rotation position can fall off a few degrees from being exactly 0 [really weird since this persists despite 90, 180, 270 being fine...], can still manage to break through walls) - NOTE!!! I might have been misinterpreting the Inspector screen. When a float is small it is represented as x.xxxxx(some number large enough to hide)E-y. 0* was probably just a small number.

public class PlayerController : MonoBehaviour
{
    public bool moving;//Is a movement occuring?
    public bool rotating;//Is rotation occurring?
    public bool collision;//Did a collision occur?
    public bool unmoving;//Is an undo move occurring? Might be necessary for preventing abuse of undo move
    public bool unrotating;//Similar
    public bool changePivot;//Is a pivot point change queued up?

    public float moveWindow;//How far into the move animation before the queued movement comes into effect. If set equal to moveTime, whole move executes before moving to the next move
    public float closeEnoughMove;//How close is close enough (to the current move position before we allow the queued move to start)? [SHOULD BE BETWEEN 0-1!!!]
    public float movePastScale;//Determines have far beyond the move amount the pastTrans object is moved(1.xx = xx% past)

    public float rotLength;//Similar
    public float rotWindow;
    public float closeEnoughRot;//
    public float rotPastScale;//How far beyond rotFinal rotPast is set(1.xx = xx% of 90 degrees past the target rotation)

    public int moveHorizontal, moveVertical;//Make holders for movement input
    public int nextMoveHorizontal, nextMoveVertical;//queued move holder

    public int rotate;//Holder for rotate input -1, 0, 1 [CCW, not, CW]
    private int nextRotate;//queued

    public int collisionCounter;//How many collisions happened?\\this frame?

    public int test;//These are used for testing various things at various times while coding, always temporarily
    public int testCount;

    private GameObject prevTransGO, pastTransGO;//prev has transform parameters for undoing moves (previous), past (i.e. beyond) has them for targeting moves (in PlayerMover, Lerps point a little bit beyond the goal position so the animation happens a bit more smoothly)
    //REPLACE THESE WITH A PAIR OF QUATERNIONS AND VECTOR3'S EVENTUALLY NOW THAT YOU KNOW YOU WERE BEING AN IDIOT


    private GameObject playerGO;//allows access to Player GO group
    void Start()
    {
        playerGO = GameObject.FindWithTag("Player");//Allows script to modify MoveTime for player
        prevTransGO = GameObject.FindWithTag("MovePrev");//Allows script to modify MovePrev GO
        pastTransGO = GameObject.FindWithTag("MovePast");//Allows script to modify MovePast GO

        transform.position = playerGO.transform.position;//Start out where the player starts

        //Initiate positions/rotations of tracking GO's
        prevTransGO.transform.position = transform.position;
        prevTransGO.transform.eulerAngles = transform.eulerAngles;
        pastTransGO.transform.position = transform.position;
        pastTransGO.transform.eulerAngles = transform.eulerAngles;

        collisionCounter = 0;//No collisions to start
        collision = false;
        moveHorizontal = 0;//Not moving at start
        moveVertical = 0;
        rotate = 0;

        changePivot = false;
        testCount = 0;
    }

    void Update()
    {

        ProcessInput();//Handles any keypresses this frame


        if (!unrotating && !unmoving)
        {
            if (!changePivot)
            {
                if (!moving)//Move from a stop
                {
                    if (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical) > 0) //if a player has initiated a move
                    {
                        Move();
                    }
                }
                else if (Vector3.Distance(transform.position, playerGO.transform.position) < closeEnoughMove)//Move queued up and far enough into current move, not currently handling a collision
                {
                    if (Mathf.Abs(nextMoveHorizontal) + Mathf.Abs(nextMoveVertical) > 0)
                    {
                        RudeMove();//Interrupts current move to start the next (how rude!)
                    }
                    else if (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical) > 0) //if a player is still holding the move key from before
                    {
                        Move();
                    }
                }

                if (!rotating)//Rotate from a stop
                {
                    if (rotate != 0)
                    {
                        Rotate();
                    }
                }
                else if (Quaternion.Angle(playerGO.transform.rotation, transform.rotation) < closeEnoughRot)//Rotate queued up and far enough into current rotation, not currently handling a collision
                {
                    if (nextRotate != 0)
                    {
                        RudeRotate();//interrupt current rotation to start next one IN PRACTICE, SAME REGARDLESS OF HOLDING OR TAPPING THE KEY
                    }
                    else if (rotate != 0)
                    {
                        Rotate();
                    }
                }
            }
            else
            {
                ChangePivot();//adjusts the pivot point
                changePivot = false;
            }
        }

        //*
        if (test == 1)
        {
            
        }
        //*/
    }

    void LateUpdate()//Handles collisions; receives input from walls if collisions have occured, then reverses any current move or rotation (reverses BOTH if both are occurring)
    {
        if (collision)
        {
            //Probably play a *boof* sound effect
            if (moving)
            {
                UnMove();
            }
            if (rotating)
            {
                UnRotate();
            }
            collision = false;//Reset for next frame
        }
    }

    void Move() //Processes a new move incl. if player is just holding a key down
    {
        //Move the "MoveTo" object accordingly, make a note that there's a move in progress
        //and set moveTime (how long a move will take to execute) in player GO
        prevTransGO.transform.position = transform.position;//Save current (soon to be previous) position
        pastTransGO.transform.position = transform.position;//prep pastTrans position

        transform.Translate(moveHorizontal * 1.0f, 0.0f, moveVertical * 1.0f, Space.World);//Move moveTo object
        pastTransGO.transform.Translate(moveHorizontal * 1.0f * movePastScale, 0.0f, moveVertical * 1.0f * movePastScale, Space.World);//Move pastTrans appropriately past
        moving = true;
        playerGO.GetComponent<PlayerMover>().timeMoveStart = Time.time;//Start counter for movement Lerp

    }

    void RudeMove() //interrupts current move with queued move, then clears queue
    {
        prevTransGO.transform.position = transform.position;//Save current (soon to be previous) position
        pastTransGO.transform.position = transform.position;//prep pastTrans position

        transform.Translate(nextMoveHorizontal * 1.0f, 0.0f, nextMoveVertical * 1.0f, Space.World);//Move moveTo object
        pastTransGO.transform.Translate(nextMoveHorizontal * 1.0f * movePastScale, 0.0f, nextMoveVertical * 1.0f * movePastScale, Space.World);//Move pastTrans appropriately past
        playerGO.GetComponent<PlayerMover>().timeMoveStart = Time.time;
        nextMoveHorizontal = 0;//clear queue
        nextMoveVertical = 0;

    }

    void UnMove()//Moves to the previously occupied spot similar to RudeMove but with a shorter moveLength
    {
        unmoving = true;//used to prevent funny business
        playerGO.GetComponent<PlayerMover>().timeMoveStart = Time.time;//Consider shortening for faster bounce back, or otherwise modifying playerMove if un
        Vector3 unMoveDirection = (prevTransGO.transform.position - transform.position).normalized;//What direction is the move to previous spot?
        transform.position = prevTransGO.transform.position;//UnMove
        pastTransGO.transform.position = transform.position;//prep pastTrans position


        pastTransGO.transform.Translate(unMoveDirection * (1.01f), Space.World);//move pastTrans a bit past the new current position in the same direction as the current position was UnMoved
        nextMoveHorizontal = 0;//clear queue
        nextMoveVertical = 0;
    }

    void Rotate()//Starts rotation to a new position +- 90 degrees from current one
    {
        rotating = true;//Rotating now
        playerGO.GetComponent<PlayerMover>().timeRotStart = Time.time;//Starting when?

        prevTransGO.transform.eulerAngles = transform.rotation.eulerAngles;//Store current rotation in case needs undone

        /*WORKSpastTransGO.transform.eulerAngles = transform.rotation.eulerAngles;//Set "past" rotation
        pastTransGO.transform.Rotate(0, rotate * 90.0f * rotPastScale, 0, Space.World);
        transform.Rotate(0, rotate * 90.0f, 0, Space.World);//Set new EQ rotation*/
        pastTransGO.transform.rotation = transform.rotation * Quaternion.Euler(0, rotate * 90.0f * rotPastScale, 0);
        transform.rotation *= Quaternion.Euler(0, rotate * 90.0f, 0);

    }

    void RudeRotate()
    {
        playerGO.GetComponent<PlayerMover>().timeRotStart = Time.time;//Starting when?


        prevTransGO.transform.eulerAngles = transform.rotation.eulerAngles;//Store current rotation in case needs undone
        pastTransGO.transform.rotation = transform.rotation * Quaternion.Euler(0, nextRotate * 90.0f * rotPastScale, 0);
        transform.rotation *= Quaternion.Euler(0, nextRotate * 90.0f, 0);

        nextRotate = 0;//Clear the queue
    }

    void UnRotate()//Returns the player to the previous kosher rotation. During this unrotate, no other moves or rotations are allowed to happen (except a simultaneous UnMove)
    {
        unrotating = true;//Well, it is!
        Quaternion difference = Quaternion.FromToRotation(transform.forward, prevTransGO.transform.forward);//what does the UnRotate do to the MoveTo GO (used for pastTransGO; if MoveTo GO rotates CCW 90 deg, pastTransGO must rotate CCW 90*alittlemore degrees so logic in PlayerMover still stands)

        playerGO.GetComponent<PlayerMover>().timeRotStart = Time.time;//Starting when? Consider adding -0.xf if undo is too slow

        pastTransGO.transform.rotation = transform.rotation * Quaternion.Euler(difference.eulerAngles * 1.01f);//That little extra nudge for pastTransGO
        transform.rotation = prevTransGO.transform.rotation;//UnRotate MoveTo GO

    }

    void ChangePivot()//If the player has clicked one of the constituent shapes, the changePivot flag is set to true. Next frame when Update is called, this function runs which A) Adjusts the pivot point of the Player GO group [container for all of the cubes that make up the Player), B) Adjusts the positions of the cubes within the Player GO group so that their position in the world doesn't change, C) moves MoveTo and other GO's to the same end, D) in practice, due to logic in Update, this all occurs in lieu of any moves or rotations that frame (so that one can't stack them up and do naughty things)
                      //POTENTIAL ISSUE: If shifting the Player group creates a collision, will this register as one? By the end of this function call the collision will not still be happening, but it technically "happened" momentarily. Will have to test. Might have to also use changePivot as an override for collisions
    {

    }



    void ProcessInput()//Checks for keypresses/lifts, adjusts variables accordingly
                       //REWRITE AS ROUTINE CALLS, INCLUDING QUEUE
    {
        //Movement input
        //key pressed
        if (Input.GetKeyDown("a"))
        {
            moveHorizontal = -1;
            moveVertical = 0;//overwrite whether vertical key is held down or not
            if (moving)//queue up next move
            {
                nextMoveHorizontal = -1;
                nextMoveVertical = 0;
            }
        }
        else if (Input.GetKeyDown("s"))
        {
            moveVertical = -1;
            moveHorizontal = 0;
            if (moving)
            {
                nextMoveVertical = -1;
                nextMoveHorizontal = 0;
            }
        }
        else if (Input.GetKeyDown("d"))
        {
            moveHorizontal = 1;
            moveVertical = 0;
            if (moving)
            {
                nextMoveHorizontal = 1;
                nextMoveVertical = 0;
            }
        }
        else if (Input.GetKeyDown("w"))
        {
            moveVertical = 1;
            moveHorizontal = 0;
            if (moving)
            {
                nextMoveVertical = 1;
                nextMoveHorizontal = 0;
            }
        }

        //Key lifted NOTE LACK OF ELSE
        if (Input.GetKeyUp("a"))
        {
            moveHorizontal = 0;
        }
        if (Input.GetKeyUp("s"))
        {
            moveVertical = 0;
        }
        if (Input.GetKeyUp("d"))
        {
            moveHorizontal = 0;
        }
        if (Input.GetKeyUp("w"))
        {
            moveVertical = 0;
        }

        //Rotation input
        //Key pressed
        if (Input.GetKeyDown("q"))
        {
            rotate = -1;
            if (rotating)
            {
                nextRotate = -1;
            }
        }
        else if (Input.GetKeyDown("e"))
        {
            rotate = 1;
            if (rotating)
            {
                nextRotate = 1;
            }
        }

        //key lifted NOTE  LACK OF ELSE
        if (Input.GetKeyUp("q"))
        {
            rotate = 0;
        }
        if (Input.GetKeyUp("e"))
        {
            rotate = 0;
        }




        if (Input.GetKeyDown("t"))//t is for test
        {
            test = 1;

        }

        if (Input.GetKeyUp("t"))
        {
            test = 0;
        }
    }
}
