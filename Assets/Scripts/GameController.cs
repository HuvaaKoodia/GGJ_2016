using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;

public class GameController : MonoBehaviour
{
	public ResourcePanel CollectedResourcesPanel;
	public ResourcePanel[] RequiredResourceLayerPanels;

	ResourceList CollectedResources;
	ResourceList[] RequiredResources;
	int[] maxResourceAmountsPerLayer;

	public void Start()
	{
		//set basic values
		maxResourceAmountsPerLayer = new int[3];

		maxResourceAmountsPerLayer[0] = 30;
		maxResourceAmountsPerLayer[1] = 15;
		maxResourceAmountsPerLayer[2] = 5;

		CollectedResources = new ResourceList();

		//generate goal
		{
			int layerAmount = 3;
			int maxResourceTypesPerLayer = 3;

			RequiredResources = new ResourceList[layerAmount];
			for (int l = 0; l < layerAmount; l++) 
			{
				//create resource layer
				var resourceList = new ResourceList();
				RequiredResources[l] = resourceList;

				//add resources to layer
				int amountOfResourcesTypes = Random.Range(1, maxResourceTypesPerLayer + 1);
				int maxResourceAmountPerLayer = maxResourceAmountsPerLayer[l];
				int resourcesToDistribute = maxResourceAmountPerLayer;
				List<ResourceID> alreadyUsedResourceTypes = new List<ResourceID>();

				for (int r = 0; r < amountOfResourcesTypes; r++)
				{
					//generate resource
					ResourceID resourceType;
					do
					{
						resourceType = (ResourceID) Random.Range(0, (int)ResourceID._Amount);
					}
					while(alreadyUsedResourceTypes.Contains(resourceType));
					alreadyUsedResourceTypes.Add(resourceType);

					//add resource amount
					int amount = 0;

					if (r < amountOfResourcesTypes - 1)
					{
						amount = Random.Range(1,  resourcesToDistribute - (amountOfResourcesTypes - 1 - r) + 1);
					}
					else
					{
						amount = resourcesToDistribute;
					}
					resourcesToDistribute -= amount;
					resourceList.AddResource(resourceType, amount);
				}
			}

			//set GUI
			CollectedResourcesPanel.Init(CollectedResources);

			for (int i = 0; i < RequiredResources.Length; i++) 
			{
				RequiredResourceLayerPanels[i].Init(RequiredResources[i]);
			}
		}
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.R)) Application.LoadLevel(Application.loadedLevel);

	}
}
