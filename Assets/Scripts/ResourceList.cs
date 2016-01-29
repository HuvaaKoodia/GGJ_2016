using UnityEngine;
using System.Collections;
using Constants;
using System.Collections.Generic;

public class ResourceList
{
	int[] resources;

	public ResourceList()
	{
		resources = new int[(int)ResourceID._Amount];
	}

	public int GetResource(ResourceID resource)
	{
		return resources[(int)resource];
	}

	public void AddResource(ResourceID resource, int addAmount)
	{
		resources[(int)resource] += addAmount;
	}
	
	public void SetResource(ResourceID resource, int amount)
	{
		resources[(int)resource] = amount;
	}
}
