using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Constants;

public class ResourcePanel : MonoBehaviour 
{
	public List<GameObject>  Items;
	public List<Text>  Values;

	public void Init(ResourceList list, bool removeUnused = false)
	{
		for (int i = 0; i < (int)ResourceID._Amount; i++)
		{
			int value = list.GetResource((ResourceID) i);

			if (removeUnused && value == 0)
			{
				Items[i].gameObject.SetActive(false);
				continue;
			}

			Items[i].gameObject.SetActive(true); 
			Values[i].text = "" + value;
		}
	}
}
