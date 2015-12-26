using UnityEngine;
using System.Collections;
using System.Collections.Generic;//For Dictionary



/*V0.02
    -Added list of states
    -Gave up on multiple states for now, let's get it working for binary open/closed
    -Added initialState variable (what orientation does the door start in?)
    -Also gave up on this

    //*/
//Attached to each Hinge GO
//Handles telling an individual door what lights, if any, to turn on/off and when (Lights.cs keeps HingeScript informed, HingeScript lights things up and sets appropriate conditions for when to start checking to turn them off again, then keeps an eye out for when to turn them off [in reality, will probably just do something like "turn on, if that means a movement do that, THEN check once if the light needs off, then just wait around until UpdateLights is called again])
//Handles door opening/closing

//Hinge GO should be located s.t. they are convenient pivot points for the doors [this will be in line with the edge of the door, probably at the corner as denoted by . below (but imagine it at the bottom of the | and right up against the _ AKA without the limitations of text drawing):
/*
| |
|_|.
| |
| |
| |
| |
|_|
| |
This is a door that will open CCW which will mean no clipping on the hinge side, though a bit on the other side (who cares....maybe even who cares about any of this beyond Hinge being in line with the crease of the door)

    EVENTUALLY! TODO: Want an actual hinge so we don't have to worry about the side connected to another wall

*/

//Hinge GO location will also probably determine the location of [Way to finish your thoughts! Lights maybe?]


public class HingeScript : MonoBehaviour
{
    //V0.02 STARTS HERE

        //abandoned for now, possibly forever, in lieu of binary open/close
    //public float[] states;//List of available states, all relative to global axes. 0 = 0 deg, 1 = 90, 2 = 180, 3 = 270, 0.5 = 45, etc. Should be stored in ascending order (TODO: MAKE IT REORDER THE LIST IN START?)

    public Transform redLight, greenLight, blueLight, yellowLight;//Attach appropriate prefabs of light GO's here
    public float lightSpacing;//How far apart is each light from its neighbor?
    public float firstLightOffset;//How far from the hinge plane is the first light?

    //V0.02 ENDS HERE
    public int cw;//Determines if the door opens CW (1) or CCW (-1). Use this over bool so I can multiply rotations by the variable

    public float openSpeed, closeSpeed;//Determines how quickly the door opens, closes (if one only specifies openSpeed [AKA closeSpeed == 0], both are the same)

    //binary open/close
    //public int currentState, initialState;//, numStates;//How many/current rotation state(s) a given door has. Usually 1 (0 = closed, 1 = open), might be more in later levels V0.02: Meaning changed, see states[] above. No longer need numStates. initialState is self-explanatory

    //TODO: Make a list of rotation points to avoid POSSIBLE stupid rounding errors

    public float rotAmount;//How many degrees the door rotates when activated. Generally 90
    public float rotPastDeg;//How many degrees it targets to rotate beyond the next position for animation reasons

    public Lights.hasLights[] doorLights;//Set in inspector, contains an entry for each light a door has attached to it and its associated timer

    Dictionary<string, Lights.GLight> lightsDict = new Dictionary<string, Lights.GLight>();//initialize the dictionary of lights for this door
                                                                                           //This is the active dictionary that actually changes during gameplay, vs. the "static" one that's set in the inspector (doorLights)

    public bool test;//FOR TESTING


    /*
    public bool redLight, greenLight, blueLight, yellowLight;//Determines which lights a hinge has (should always be at least one, unless we do something more complex than door <- switch eventually)
    public bool redLightOn, greenLightOn, blueLightOn, yellowLightOn;//Same name as variables in Lights.cs, however won't always be in the same state. Lights will stay on while a door is moving, and thus "true" here, though the player might have already left a switch
    //DONE: MAKE A DICTIONARY/ARRAY, ALSO MAKE PRIVATE
    //On the note of dictionary, might have Start() add the appropriate lights (with colors as their string labels)
    //Dictionary structure: < "Name as string", has/doesn't, on/off, timervalue> (e.g. <"Green", true, false, -1> would mean hinge has a green light, currently off, and when it is switched it stays open until triggered again (-1). 0 means closes immediately, larger values are number of seconds it stays open until it closes automatically. Need some thought about the visuals (do the lights blink on/off to signify "I'm a-closin' soon!"? If so, based off of how long the thing is open or always starts x seconds before it closes?)
    */

    void Start()
    {
        //Dictionary<string, Lights.GLight> lightsDict = new Dictionary<string, Lights.GLight>();//initialize the dictionary of lights for this door
        //This is the active dictionary that actually changes during gameplay, vs. the "static" one that's set in the inspector
    }

    // Use this for initialization, called by 
    public void Initialize()
    {
        //Dictionary<string, Lights.GLight> lightsDict = new Dictionary<string, Lights.GLight>();//initialize the dictionary of lights for this door
        //This is the active dictionary that actually changes during gameplay, vs. the "static" one that's set in the inspector

        foreach (var item in Lights.MasterLightsTable())//Populates lights dictionary 
        {
            lightsDict.Add(item.color, new Lights.GLight(false, 0f));
        }

        foreach (var item in doorLights)//Let the door know which lights it actually has, and what their timers are
        {
            lightsDict[item.color].has = true;
            lightsDict[item.color].timer = item.timer;
        }
        PlaceLights();//Add light GO's to the scene, where they belong both in space and in the hierarchy
        if (closeSpeed == 0) closeSpeed = openSpeed;




        //FOR TESTING
        //test = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaceLights()//When the scene is first generated, creates the GO's for the lights as children GO's, places them appropriately by the door
                      //TODO: Incorporate prefabs
                      //WORK IN PROGESS
    {
        /*Debug.Log(transform.tag);
        Debug.Log("Position: " + transform.position);
        Debug.Log("X: " + 2.0f * Mathf.Cos(transform.eulerAngles.y + Mathf.PI / 2.0f));
        Debug.Log("Z: " + 2.0f * Mathf.Sin(transform.eulerAngles.y + Mathf.PI / 2.0f));
        Debug.Log("OffX: " + firstLightOffset * Mathf.Cos(transform.eulerAngles.y));
        Debug.Log("OffZ: " + firstLightOffset * Mathf.Sin(transform.eulerAngles.y));
        //*/
        Vector3 lightOffset = new Vector3(2.0f * Mathf.Cos(transform.eulerAngles.y + Mathf.PI / 2.0f), 0f, 2.0f * Mathf.Sin(transform.eulerAngles.y + Mathf.PI / 2.0f)) + new Vector3(firstLightOffset * Mathf.Cos(transform.eulerAngles.y), 0f, firstLightOffset * Mathf.Sin(transform.eulerAngles.y));//Offset from the hinge to the first light. First part is magnitude and direction of the offset from the hinge to the line of lights, second part adds an offset so the light is not directly across from the hinge

        //THIS WILL BE REPLACED WITH A PREFAB INSTANTIATION
        GameObject tempLight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //tempLight.AddComponent<Rigidbody>();
        tempLight.transform.position = transform.position + lightOffset;
        tempLight.transform.parent = transform.GetChild(0);
        
                                                                                                                                                                                                                                                                                                        //Vector3 hingeLocation = transform.position;



    }

    public void UpdateLights()//Checks current status of all lights and updates their on/offness appropriately.
                              //Should yield if opening or timers still running, then continue with the check once door is free again
                              //At the end of this function, call Open- or CloseSesame() depending on state of lights
    {
        bool open = true;
        foreach (var item in lightsDict)
        {
            print("Item key, value: " + item.Key + ", " + item.Value);
            if (item.Value.has)
            {
                print("Item has: " + item.Key + ", " + item.Value);

                if (!Lights.LightLit(item.Key))
                {
                    print("Light not lit: " + item.Key + ", " + item.Value);

                    open = false;
                }
            }
        }

        if (open)
        {
            //StartTimer();
            OpenSesame();
        }
        else
        {
            CloseSesame();
        }
    }

    void OpenSesame()
    {
        StartCoroutine(SwingDoor((transform.rotation * Quaternion.Euler(0, Mathf.RoundToInt(rotAmount * cw), 0)), openSpeed));
    }

    void CloseSesame()
    {
        StartCoroutine(SwingDoor((transform.rotation * Quaternion.Euler(0, Mathf.RoundToInt(rotAmount * -cw), 0)), closeSpeed));
    }

    IEnumerator SwingDoor(Quaternion targetRot, float rotSpeed)
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

}
