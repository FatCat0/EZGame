using UnityEngine;
using System.Collections;

public class PH_PlayerController : MonoBehaviour 
{
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;


	void Update () 
	{
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		rotation *= Time.deltaTime;
		transform.Rotate(0, rotation, 0);
	}
}
