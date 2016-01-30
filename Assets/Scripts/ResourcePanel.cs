using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Constants;

public class ResourcePanel : MonoBehaviour 
{
	public List<GameObject>  Items;
	public List<Text>  Values;

	public void Init(ResourceList list)
	{
		for (int i = 0; i < (int)ResourceID._Amount; i++)
		{
			int value = list.GetResource((ResourceID) i);
			Values[i].text = "" + value;
		}
	}

	public void Init(ResourceList list, ResourceList listMax, bool removeUnused = false)
	{
		for (int i = 0; i < (int)ResourceID._Amount; i++)
		{
			int value = list.GetResource((ResourceID) i);
			//int max = listMax.GetResource((ResourceID) i);

			if (removeUnused && value == 0)
			{
				Items[i].gameObject.SetActive(false);
				continue;
			}

			Items[i].gameObject.SetActive(true);
			//Values[i].text = "" + (max - value) + "/" + max;
			Values[i].text = "" + value;
		}
	}
}
