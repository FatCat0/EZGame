using UnityEngine;
using System.Collections;

public class NOTRELATEDPlayerController : MonoBehaviour 
{
	public float zRotation = 0F;
	public float currentRotate;
	public float tilt;  // speed of left and right tilt
	public float tiltAcc;  // acceleration of tilt
	public float rightTilt;
	public float leftTilt;
    public Quaternion rotAngle;
	
	void Update() 
	{
		
		

		if (Input.GetKey(KeyCode.RightArrow) ||  Input.GetKey(KeyCode.D))
		{
            zRotation += tilt * Time.deltaTime;
			transform.eulerAngles = new Vector3(0, 0, -zRotation);
		}
		
		if (zRotation > rightTilt)
		{
			transform.eulerAngles = new Vector3(0, 0, -rightTilt);
			zRotation = rightTilt;
		}
		
		if (Input.GetKey(KeyCode.LeftArrow) ||  Input.GetKey(KeyCode.A))
		{
			zRotation += tilt * Time.deltaTime;
			transform.eulerAngles = new Vector3(0, 0, zRotation);		
		}
		
		if (zRotation < leftTilt  && zRotation > 40)
		{
			transform.eulerAngles = new Vector3(0, 0, leftTilt);
			zRotation = leftTilt;
		}
	}
}
