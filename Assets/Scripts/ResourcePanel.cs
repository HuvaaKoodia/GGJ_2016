using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Constants;

public class ResourcePanel : MonoBehaviour 
{
	public List<Text> Names, Values;

	public void Init(ResourceList list)
	{
		for (int i = 0; i < (int)ResourceID._Amount; i++)
		{
			Names[i].text = ((ResourceID) i).ToString();
			Values[i].text = "" + list.GetResource((ResourceID) i);
		}
	}
}
