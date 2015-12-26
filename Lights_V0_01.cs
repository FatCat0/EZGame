using UnityEngine;
using System.Collections;
using System.Collections.Generic;//For Dictionary


//This class is in flux right now. Current thoughts:
//Attached to doors GO (the GO containing the set of door hinges, which contain the actual doors themselves [HIERARCHY IMPORTANT])
//Trying to make the functions of this that other objects will use all static so that no other objects have to inherit this script, they can just make Lights.Whatever() calls
//AS A RESULT, I use a lot of static objects, sometimes even need to generate extra copies of things in static form. Not sure if I always need to or if this will eventually bite me in the butt, and I KNOW I abuse the spirit of static objects by making static references to the Doors GO and its attached Lights.cs script, but whatever works, right?
//Whenever something happens that triggers lights, either on or off, this script is called. It then passes that information down to all doors so they know that something has happened to which they might want to respond

//NOT SURE: Do I want the doors themselves or their hinges to be the ones that know the light info? Probably the hinges, since those are what will be rotating anyway.

public class Lights_V0_01 : MonoBehaviour
{

    //These things are static so that they can be used in static functions
    //public static bool redLightOn, greenLightOn, blueLightOn, yellowLightOn;//True if appropriate switch on, false if not
                                                                            //MAKE A DICTIONARY/ARRAY



    public static Lights lightsScript;//Similar to lightsGO

    public static GameObject lightsGO;//This is the GO to which Lights.cs is applied, but a static version. Used for static references. Read note in Start()

    public hasLights[] masterLightsTable;//This can be modified in the Inspector. Add all of the lights we want to be available for the scene there. Right now RGBY
    public static hasLights[] masterLightsTableStatic;//Static copy of the masterlightstable

    public Dictionary<string, GLight> lightsDict;

    public int test;

   /* PleaseWork(lightsGO);//
    public static Lights PleaseWork(GameObject lightsGO)
    {
        return lightsGO.GetComponent<Lights>();
    }//*/

    void Start()
    {
        masterLightsTableStatic = masterLightsTable;
        lightsGO = transform.gameObject;//THIS MAKES IT VERY IMPORTANT THAT Lights.cs ONLY BE APPLIED TO THE LIGHTS GO (I think...things become ambiguous in the UpdateLights function otherwise)
        lightsScript = lightsGO.GetComponent<Lights>();//Similarly important to above

        lightsDict = new Dictionary<string, GLight>();//GenLightsDict();// Generates a base lights dictionary

        foreach (var item in lightsScript.masterLightsTable)//Populates lights dictionary 
        {
            lightsDict.Add(item.color, new GLight(true, 0f));
        }
        foreach (var item in lightsDict.Values)//Sets all of the "has" values to true (since Lights tracks all lights)
        {
            item.has = true;
        }

        foreach (Transform child in transform)//Manually "Start()"'s each hinge's HingeScript.cs
            //I do it this way so I know that necessary references in Lights.cs are already good to go before Hinges start trying to do shit (and because Unity kept yelling at me...)
        {
            child.GetComponent<HingeScript>().Initialize();
        }

        UpdateLights();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetOn(string color)//Sets the appropriate light position to "on" when a switch is hit
    {

        lightsScript.lightsDict[color].on = true;
        /*Removed in favor of dictionary lookup
        switch (color)
        {
            case "red":
                redLightOn = true;
                break;
            case "green":
                greenLightOn = true;
                break;
            case "blue":
                blueLightOn = true;
                break;
            case "yellow":
                yellowLightOn = true;
                break;
        }
        //*/
        UpdateLights();//Makes sure all doors are current on the status of lights
    }

    public static void SetOff(string color)//Similar
    {
        lightsScript.lightsDict[color].on = false;
        /*foreach (var item in lightsScript.lightsDict)
        {
            print(item.Key);
        }
        /*Removed in favor of dictionary lookup
        switch (color)
        {
            case "red":
                redLightOn = false;
                break;
            case "green":
                greenLightOn = false;
                break;
            case "blue":
                blueLightOn = false;
                break;
            case "yellow":
                yellowLightOn = false;
                break;
        }
        //*/
        UpdateLights();
    }

    public static bool LightLit(string color)
    {
        return lightsScript.lightsDict[color].on;
    }

    public static void UpdateLights()//Makes sure doors all know what's up with the light situation
                                     //Not sure if this is optimal, but basically this function just calls all of the individual hinges to check the light status. This has the benefit of being able to just call Lights.UpdateLights() which then takes care of an arbitrary number of hinges
    {
        foreach (Transform child in lightsGO.transform)
        {
            child.GetComponent<HingeScript>().UpdateLights();
        }

    }

    public class GLight//Class used in the dictionaries in this and HingeScript. Dictionary looks like <String color, Light light>
    {
        public bool has;//True if the associated color is one of the lights for this door
        public bool on;//True when the light is on (NOTE: Slightly differenct context between use in Lights.cs and HingeScript.cs. In the former, this is true whenever a light of a particular color is currently being triggered. In the latter, true when the light is lit, which is likely to be out of sync with triggering)
        public float timer;//Determines how long a light stays activated once it is fully on. For Lights.cs, this should always be 0. For HingeScript.cs, it's a little different. FOR NOW, effectively 0 unless all appropriate lights are turned on, at which point the timer is started. Might even just ignore this value for Lights.cs ala "effectively 0" part of HingeScript.cs

        public GLight(bool initHas, float initTimer)
        {
            on = false;
            has = initHas;
            timer = initTimer;
        }


    }


    public static hasLights[] MasterLightsTable()
    {
        return masterLightsTableStatic;
    }
    /*
    public static Dictionary<string, GLight> GenLightsDict()//Generates and returns a blank dictionary of available lights, blank -> all with "has==false", "timer==0"
    {
        Dictionary<string, GLight> newLightsDict = new Dictionary<string, GLight>();
        //*
        foreach (var item in masterLightsTableStatic)
        {
            newLightsDict.Add(item.color, new GLight(item.has, 0f));
        }
        

        /*newLightsDict.Add("red", new GLight(false, 0f));
        newLightsDict.Add("green", new GLight(false, 0f));
        newLightsDict.Add("blue", new GLight(false, 0f));
        newLightsDict.Add("yellow", new GLight(false, 0f));
        /
        return newLightsDict;
    }
    */

    //Makes a list of lights which is accessible in the inspector (i.e. we can add red light, blue light, whatever to this list from the inspector, and inside the scripts this list is fully accessible)
    [System.Serializable]
    public class hasLights
    {
        public string color;
        //public bool has;
        public float timer;
    }




}
