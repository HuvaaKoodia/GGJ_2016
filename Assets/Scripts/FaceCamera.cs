using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour 
{
	void Update () 
	{
		Vector3 lookVector = Vector3.ProjectOnPlane(Camera.main.transform.position - transform.position, Camera.main.transform.right);
		transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
	}
}
