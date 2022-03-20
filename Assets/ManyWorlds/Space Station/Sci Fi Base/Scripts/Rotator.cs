using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
	{

	public Vector3 RotationXYZ  = Vector3.zero;
	public float Speed = 0.5f;


	// Update is called once per frame
	void FixedUpdate ()
	{
		gameObject.transform.Rotate (RotationXYZ, Speed);
	}
}
