using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;

public class GameController : MonoBehaviour
{
	public ResourcePanel CollectedResourcesPanel;
	public ResourcePanel[] RequiredResourceLayerPanels;

	public static GameController I;

	public Villager VillagerPrefab;
	public Transform VillagerStartPosCenter;
	public Transform ResourceDropArea;

    public CutScene theGodCutScene;

    public YearOver YearOverPrefab;
	public CakeView Cake;

	public Timer Timer;

	public static int VillagerAmount = 10, CakesMade = 0;
 
	ResourceList CollectedResources;
	ResourceList[] RequiredResources, RequiredResourcesMax;
	int[] maxResourceAmountsPerLayer;

	int currentBakeLayer = 0, maxAmountRequired;

	public AudioSource lightningSource;

	private List<Villager> Villagers;

	private void Awake()
	{
		I = this;
	}

	private void Start()
	{
		//set basic values
		maxResourceAmountsPerLayer = new int[3];

		maxResourceAmountsPerLayer[0] = 30;
		maxResourceAmountsPerLayer[1] = 15;
		maxResourceAmountsPerLayer[2] = 5;

		CollectedResources = new ResourceList();

		//set up events
		Timer.OnJudgementDayEvent += OnJudgementDay;

		//generate goal
		{
			int layerAmount = 3;
			int maxResourceTypesPerLayer = 3;

			RequiredResources = new ResourceList[layerAmount];
			RequiredResourcesMax = new ResourceList[layerAmount];
			for (int l = 0; l < layerAmount; l++) 
			{
				//create resource layer
				RequiredResources[l] = new ResourceList();
				RequiredResourcesMax [l] = new ResourceList();

				//add resources to layer
				int amountOfResourcesTypes = Random.Range(1, (maxResourceTypesPerLayer - l) + 1);
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
					RequiredResources[l].AddResource(resourceType, amount);
                    RequiredResourcesMax[l].AddResource(resourceType, amount);
					maxAmountRequired += amount;
				}
			}

			//set GUI
			UpdateCollectedResourcesGUI();
			UpdateRequiredResourcesGUI();

			//spawn villagers
			int amountOfVillagers = VillagerAmount;
			Villagers = new List<Villager>();
			for (int i = 0; i < amountOfVillagers; i++) 
			{
				var villager = Instantiate(VillagerPrefab, Helpers.RandomPlanePosition(VillagerStartPosCenter.position, 5f), Quaternion.identity) as Villager;
				villager.OnResourceDroppedEvent += OnResourceGained;
				villager.OnBakeCompleteEvent += OnBakeCompleted;
				villager.ResourceDropPoint = ResourceDropArea;
				villager.Cake = Cake.transform;
				Villagers.Add(villager);
			}
		}
	}

	void OnJudgementDay()
	{
		//stop villagers
		foreach (var villager in Villagers) 
		{
			villager.Stop();
		}

        theGodCutScene.theGodShow();

		int MaxAmountOfResourcesInCake = 0;
		for (int i = 0; i < maxResourceAmountsPerLayer.Length; i++) 
		{
			MaxAmountOfResourcesInCake += maxResourceAmountsPerLayer[i];
		}

		int AmountOfResourcesMissingInCake = 0;

		for (int i = 0; i < RequiredResources.Length; i++) 
		{
			var layer = RequiredResources[i];
			for (int j = 0; j < (int)ResourceID._Amount; j++) 
			{
				AmountOfResourcesMissingInCake += layer.GetResource((ResourceID)j);
			}
		}

		int failPercentage = (int)((AmountOfResourcesMissingInCake / (float) MaxAmountOfResourcesInCake) * 100);
		int deadVillagers = 0;
		if (failPercentage == 0) 
		{
			Debug.Log("perfect cake -> no one dies");
		}
		else if (failPercentage > 0 && failPercentage <= 10) 
		{
			Debug.Log("near perfect cake -> one villager dies");
			deadVillagers = 1;
		}
		else if (failPercentage > 10 && failPercentage <= 50) 
		{
			Debug.Log("ok cake -> 2 - 3 villagers die");
			deadVillagers = Random.Range(2, 3 + 1);
		}
		else if (failPercentage > 50 && failPercentage <= 90) 
		{
			Debug.Log("bad cake -> 3-5 villagers die");
			deadVillagers = Random.Range(3, 5 + 1);
		}
		else if (failPercentage > 90) 
		{
			Debug.Log("terrible cake (or no cake at all!)-> everyone dies");
			deadVillagers = VillagerAmount;
		}

        CakesMade += 1;
		VillagerAmount -= deadVillagers;

		YearOverPrefab.showResult(failPercentage, deadVillagers);
		
		if (deadVillagers > 0)
		{       	
			lightningSource.Play();
		}

		for (int v = 0; v < deadVillagers; v++) 
		{
			Villagers[v].Die();
		}
	}

	void OnResourceGained(ResourceID resource)
	{
		CollectedResources.AddResource(resource, 1);
		UpdateCollectedResourcesGUI();
	}

	bool OnBakeCompleted()
	{
		//check if can bake
		var layer = RequiredResources[currentBakeLayer];

		bool bakeFail = true;
		for (int i = 0; i < (int)ResourceID._Amount; i++) 
		{
			var resource = (ResourceID) i;
			int requiredAmount = layer.GetResource(resource);
			if (requiredAmount > 0)
			{
				//check if resource available
				int collectedAmount = CollectedResources.GetResource(resource);

				if (collectedAmount > 0)
				{
					bakeFail = false;
					CollectedResources.AddResource(resource, -1);
					layer.AddResource(resource, -1);

					UpdateRequiredResourcesGUI();
					UpdateCollectedResourcesGUI();
					break;
				}
			}
		}

		if (bakeFail) return false;

		//check if layer completed
		bool layerCompleted = true;
		for (int i = 0; i < (int)ResourceID._Amount; i++) 
		{
			var resource = (ResourceID) i;
			int resourceAmount = layer.GetResource(resource);
			if (resourceAmount > 0)
			{
				layerCompleted = false;
				break;
			}
		}

		if (layerCompleted)
		{
			//move onto next layer
			currentBakeLayer++;

			//whole cake finished
			if (currentBakeLayer == RequiredResources.Length)
			{
				//celebrations!!
				foreach (var villager in Villagers) 
				{
					villager.Stop();
				}
			}
		}

		//update cake graphics
		int amountRequired =0;
		for (int l = 0; l < RequiredResources.Length; l++) 
		{
			for (int i = 0; i < (int)ResourceID._Amount; i++) 
			{
				var resource = (ResourceID) i;
				amountRequired += RequiredResources[l].GetResource(resource);
			}
		}


		Cake.SetCakeCompletionPercentage(1- (amountRequired / (float) maxAmountRequired));

		return true;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) GotoNextYear();
	}


	public void GotoNextYear()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	void UpdateCollectedResourcesGUI()
	{
		CollectedResourcesPanel.Init(CollectedResources);
	}

	void UpdateRequiredResourcesGUI()
	{
		for (int i = 0; i < RequiredResources.Length; i++) 
		{
			RequiredResourceLayerPanels[i].Init(RequiredResources[i], RequiredResourcesMax[i], true);
		}
	}
}
