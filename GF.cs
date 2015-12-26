using UnityEngine;
using System.Collections;
/*V0.02
    -Quiet so far....

//*/
//Grant Functions
//Should all be static functions so I can just call stuff with GF.Whatever()
public class GF : MonoBehaviour
{
    


    //ClockWise returns +/- 1 depending on whether the shortest rotation from 'a' to 'b' about axis (0/1/2 x/y/z [CHECK THAT THIS IS TRUE]) is CW or not, 0 if equal

        //Could be made a little faster but why?

    public static int ClockWise(Quaternion a, Quaternion b, int axis)
    {
        float rot = SignedAngle(a, b, axis);
        if (rot == 0) return 0;//Special case
        return (int)Mathf.Sign(rot);
    }

    //SignedAngle returns the shortest angle and direction (+ = cw, - = ccw) to rotate a into b along 'axis' (0/1/2 x/y/z)
    //Returns 0 if a == b
    //Returns 180 if they are 180 degrees apart
    public static float SignedAngle(Quaternion a, Quaternion b, int axis)
    {
        float ai = a.eulerAngles[axis];
        float bi = b.eulerAngles[axis];
        if (ai == bi) return 0;//Special case



        float lesser = Mathf.Min(ai, bi);
        float greater = Mathf.Max(ai, bi);

        if (greater - lesser <= 180)
        {
           // if (greater == bi)
            {
                return bi - ai;
            }
            /*/else
            {
                //return ai - bi;
            }//*/
        }
        else//NOT TESTED
        {
           // if (greater == bi)
            {
                return ((bi + ai) % 360);
            }
            /*/else
            {
               // return ((ai + bi) % 360);
            }//*/
        }

    }

    //public static
}
