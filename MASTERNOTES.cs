using UnityEngine;
using System.Collections;
//Dummy script to hold notes for updating the master file
public class MASTERNOTES : MonoBehaviour
{

    //Changed tags (maybe name as well?) on MoveTo object, Player object container
    //Attached PlayerMove and PlayerController scripts to Player group and MoveTo GO respectively
    //Set moveLength to 0.5 on MoveTo GO 


    //GLITCHES
    //Start game, rotate left twice quickly (rot and queue rot). Rotates despite the fact it shouldn't. Wtf.


    /*VERSION 0.02!!!
    V0.01 mostly works. I started adding coroutines/rewriting some stuff/adding some new features before upgrading the version number so it's not an overall working thingamajig.
    In V0.02 I will be:
    -Making all movement coroutine-based, including collisions
        -Movements should include intelligent input-queueing and be able to make uninterruptible moves due to collisions
            -Input queueing: While a move is happening, player might press (or continue to hold) an input buttom. I would like a double-tap to equate to two moves, and a hold to be just as fast as mashing the move button (it gets to a certain point in the move and checks if the same button is still held down, queues the next move)
            -Will probably need to write a separate function just to handle input translation
    -Adding pointers since I just learned how to use them (don't need to GetComponent all the time, nor do I need to make copies of GO's, etc.)
        -NEVER MIND! Can't use pointers in Unity since they're used natively!
        -USEFUL NOTE: If I want to pass the actual object instead of a pointer, use "ref" as in SomeFunc(ref GameObject someObject)
    -TRYING to implement collisions for doors (each door will have a "safe spot" it pushes the player back to if a collision occurs. Might be generous and try to let them stay on the "other side" if a collision happens when the player is clearly through the door, but also might not. Basic approach:
    _______i
          X|
          X|
          X|
          X|
          ___ is wall, | is door, door opens to the right, X's denote the "safe spots" that that door will move the player back to in the even of a collision. So if the door closes on the player, door will bounce back, player slides to the nearest X (have to make sure the "rightmost" block goes there) and door closes behind player
    -RotateNearest() - looks at the current rotation of the player, starts and (uninterruptable) rotation toward the nearest 90* position
    -Put in a list of valid positions for the doors, move +/- in that list (%number of positions [might I need to set a flag when it wraps around?])
    -Door open/close behavior needs to actually be finished as opposed to test code where it is now
    -Playing around with global class instance variables ala #2: http://www.gamasutra.com/blogs/JohnWarner/20130910/194559/The_top_5_things_Ive_learned_as_a_Unity_developer.php
        CONSIDER SINGLETONS FOR SCRIPTS ATTACHED TO MORE THAN ONE OBJECT/INSTANTIABLE OBJECTS, IF NEED BE
    -Making use of delegates (#3 from above)
    -Incorprate get/set?


    ASK MIKE:
    -Prefabs
    -Hinge: Why is the hinge scaled instead of the door?




    void OTHERNOTES()
    {
    //Make otherGameObject parent of this GO:
    //transform.parent = otherGameObject.transform
    }
    //*/
}
