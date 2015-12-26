using UnityEngine;
using System.Collections;
//Sends a notification to MoveTo.PlayerController when a player object collides with a wall
//Changes a Bool for reaction of collision and also increments a counter in case we want to put consequences for colliding that care about how many times it's happened

    //MIGHT ADD: Wall/cube change color when they collide and then fade back to original color

//NEEDS TO BE MORE ROBUST
//Probably should change from "move back when collision occurs" to actually understanding nature of collision and pushing the block in the opposite direction (think what would happen when a moving door collides with the player when they're static)

public class WallCollision_V0_01 : MonoBehaviour
{

    private GameObject MoveToGO;//grab Player GO to send messages when collisions happen


    // Use this for initialization
    void Start()
    {
        MoveToGO = GameObject.FindWithTag("MoveTo");//Grab MoveTo GO
    }


    void Update()
    {

    }

    void LateUpdate()
    {

    }

    void OnTriggerEnter(Collider other)//If a collision occurs, let MoveToGO know. Also throws a bunch of messages at the start of the scene due to walls colliding with one another
    {
        MoveToGO.GetComponent<PlayerController>().collision = true;
        MoveToGO.GetComponent<PlayerController>().collisionCounter += 1;
    }
}
