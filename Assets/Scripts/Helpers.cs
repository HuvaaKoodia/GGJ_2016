using UnityEngine;
using System.Collections;

public class Helpers
{
	public static Vector3 RandomPlanePosition(Vector3 center, float radius)
	{
		var circle = Random.insideUnitCircle * radius;
		return center + Vector3.right * circle.x + Vector3.forward * circle.y;
	}

	public static Vector3 RandomPlanePosition(Vector3 center, float minRadius, float maxRadius)
	{
		var circle = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
		return center + Vector3.right * circle.x + Vector3.forward * circle.y;
	}
}
