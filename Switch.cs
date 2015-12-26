using UnityEngine;
using System.Collections;
//Recognizes when a player shape collides with the switch to which this script is applied

    //Applied to all switches, some parameters (INSERT HERE) different to distinguish color and objects that will be 

    //MIGHT ADD: secret messages [a specific switch might be the trigger for a later section of the level to activate or be set to a specific state]

public class Switch : MonoBehaviour {
    public int switchHitCount;//How many times has this switch been activated?
    //public bool switchHit;//Is the switch currently depressed? [If so, poor switch]

    public string color;//What color is this switch? Format Red, Green, etc. Might bear rethinking. Maybe reference the name of the shader, or use shader name on cube to give itself a tag and set color by name of the shader on the switch

    //public GameObject doorsGO;//This GO should be the "doors" container GO which is the top hierarchy level doors GO in the scene. Need to come up with the specifics of said hierarchy before final draft. When the correct color block is over this switch, it will tell doorsGO to light (or attempt to light) all of the appropriate lights on all of the doors

    //MIGHT be able to not store this GO; just find by tag and reference it in Start to get lights

    public GameObject secretGO;//Game Object (hidden in the scene) containing another script with instructions of how to handle the secret switching event. Example: secretGO might be connected to the last 3 doors in a maze and when this switch is activated it ensures that the last 3 doors are in the right position to start the end of the puzzle. This switch just pokes secretGO and tells it "Hey, you might have to do some things"

    public Lights lights;//Lights is a script on the doorsGO that accepts input about when different switches are pressed and lets the appropriate doors know this has happened

	// Use this for initialization
	void Start () {
        switchHitCount = 0;
        //switchHit = false;
        //doorsGO = GameObject.FindGameObjectWithTag("Doors");
        //lights = doorsGO.GetComponent<Lights>();
    }

    void OnTriggerEnter(Collider other)//When a block runs into a switch
    {
        if (other.gameObject.CompareTag(color))//If the colliding block is the same color as the switch
        {
            //THIS MIGHT CHANGE IN THE END, BUT FOR NOW A SWITCH OF COLOR color ATTEMPTS TO LIGHT ALL LIGHTS OF THE SAME COLOR
            Lights.SetOn(color);
        }
    }

    void OnTriggerExit(Collider other)//When a block leaves a switch MIGHT HAVE TO BE RETHOUGHT IF WE HAVE MANY BLOCKS OF THE SAME COLOR ADJACENT TO ONE ANOTHER, OR MULTIPLE SWITCHES OF A SINGLE COLOR AVAILABLE FOR TOUCHING
    {
        /*    void OnTriggerStay(Collider other) {
        if (other.GetComponent<OtherScript>())
            other.GetComponent<OtherScript>().DoSomething();
        
    }
    */
        if (other.gameObject.CompareTag(color))//If the colliding block is the same color as the switch
        {
            //THIS MIGHT CHANGE IN THE END, BUT FOR NOW A SWITCH OF COLOR color ATTEMPTS TO TURN OFF ALL LIGHTS OF THE SAME COLOR
            Lights.SetOff(color);
        }
    }
}
