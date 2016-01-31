using UnityEngine;
using System.Collections;

public class FlipAround : MonoBehaviour 
{
	IEnumerator Start()
	{

		yield return new WaitForSeconds(Random.Range(0f, 5f));


		while(true)
		{
			transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

			yield return new WaitForSeconds(Random.Range(5f, 10f));
		}
	}
}
