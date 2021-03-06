﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public ResourcePanel CollectedResourcesPanel;
	public ResourcePanel[] RequiredResourceLayerPanels;
	public static GameController I;
	public Villager VillagerPrefab;
	public Transform VillagerStartPosCenter;
	public Transform ResourceDropArea;
	public Canvas GUICanvas;
	public Text YearText;
	public GameObject UIBasics;
	public CutScene theGodCutScene;
    public YearOver YearOverScreen;
	public CakeView Cake;
	public Timer Timer;
	public MouseSelection MouseSelection;
	public static int VillagerAmount = 0, CakesMade = 0;
 
	public static bool StartInMenu = true;

	public GameObject LightningBoltPrefab;

	ResourceList CollectedResources;
	ResourceList[] RequiredResources, RequiredResourcesMax;
	int[] maxResourceAmountsPerLayer;
	int [,] LayerResourceTypeAmounts;

	int currentBakeLayer = 0, maxAmountRequired;

	public AudioSource lightningSource, Music, Kong;
	private List<Villager> Villagers;

	private void Awake()
	{
		I = this;
	}

	private IEnumerator Start()
	{
		if (StartInMenu)
		{
			VillagerAmount = 10;
			CakesMade = 0;
			StartMenu.SetActive(true);
		}

		Timer.Hourglass.gameObject.SetActive(false);

		Timer.enabled = false;
		MouseSelection.enabled = false;

		//set basic values
		maxResourceAmountsPerLayer = new int[3];
		
		maxResourceAmountsPerLayer[0] = 30;
		maxResourceAmountsPerLayer[1] = 15;
		maxResourceAmountsPerLayer[2] = 5;

		LayerResourceTypeAmounts = new int[5,3];

		LayerResourceTypeAmounts[0,0] = 2;
		LayerResourceTypeAmounts[0,1] = 1;
		LayerResourceTypeAmounts[0,2] = 1;

		LayerResourceTypeAmounts[1,0] = 2;
		LayerResourceTypeAmounts[1,1] = 2;
		LayerResourceTypeAmounts[1,2] = 1;

		LayerResourceTypeAmounts[2,0] = 3;
		LayerResourceTypeAmounts[2,1] = 2;
		LayerResourceTypeAmounts[2,2] = 1;

		LayerResourceTypeAmounts[3,0] = 3;
		LayerResourceTypeAmounts[3,1] = 2;
		LayerResourceTypeAmounts[3,2] = 2;

		LayerResourceTypeAmounts[4,0] = 3;
		LayerResourceTypeAmounts[4,1] = 3;
		LayerResourceTypeAmounts[4,2] = 2;

		CollectedResources = new ResourceList();

		int turnIndex = CakesMade;

		//set up events
		Timer.OnJudgementDayEvent += OnJudgementDay;
		
		//generate goal
		{
			int layerAmount = 3;
			
			RequiredResources = new ResourceList[layerAmount];
			RequiredResourcesMax = new ResourceList[layerAmount];
			for (int l = 0; l < layerAmount; l++) 
			{
				//create resource layer
				RequiredResources[l] = new ResourceList();
				RequiredResourcesMax [l] = new ResourceList();
				
				//add resources to layer
				int amountOfResourcesTypes = LayerResourceTypeAmounts[Mathf.Min(turnIndex, LayerResourceTypeAmounts.GetLength(0) - 1) , l];
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
				var villager = Instantiate(VillagerPrefab, Helpers.RandomPlanePosition(VillagerStartPosCenter.position, 7f), Quaternion.identity) as Villager;
				villager.OnResourceDroppedEvent += OnResourceGained;
				villager.OnBakeCompleteEvent += OnBakeCompleted;
				villager.ResourceDropPoint = ResourceDropArea;
				villager.Cake = Cake.transform;
				Villagers.Add(villager);
			}
		}

		YearText.gameObject.SetActive(false);
		UIBasics.SetActive(false);

		while (StartInMenu)
		{
			yield return null;
		}
		StartMenu.SetActive(false);
		Kong.Play();

		GUICanvas.enabled = false;

		//start cutscenes:
		yield return StartCoroutine(theGodCutScene.godCutSceneCoroutine());

		GUICanvas.enabled = true;
		YearText.text = "Year "+(CakesMade + 1);
		YearText.gameObject.SetActive(true);
		
		yield return new WaitForSeconds(3f);

		Timer.enabled = true;
		//yield return new WaitForSeconds(0.35f);
		
		YearText.gameObject.SetActive(false);
		UIBasics.SetActive(true);
		Timer.Hourglass.gameObject.SetActive(true);
		MouseSelection.enabled = true;
		Music.Play();
	}

	public GameObject StartMenu;

	public void OnStartPressed()
	{
		StartInMenu = false;
	}

	public void OnQuitPressed()
	{
		Application.Quit();
	}

	void OnJudgementDay()
	{
		StartCoroutine(JudgementDayCoroutine());
	}

	IEnumerator JudgementDayCoroutine(int fixedFailPercentage = -1)
	{
		//hide stuff
		UIBasics.SetActive(false);
		Timer.Hourglass.gameObject.SetActive(true);
		Timer.enabled = false;
		Timer.isJudgementDay = true;

		//stop villagers
		foreach (var villager in Villagers) 
		{
			villager.Stop();
		}
		
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
		int cutSceneType = 0;

		if( fixedFailPercentage >= 0) failPercentage = fixedFailPercentage;

		if (failPercentage == 0) 
		{
			Debug.LogError("perfect cake -> no one dies");
			cutSceneType = 1;
			CakesMade += 1;
		}
		else if (failPercentage > 0 && failPercentage <= 10) 
		{
			Debug.LogError("near perfect cake -> one villager dies");
			deadVillagers = 1;
			CakesMade += 1;
		}
		else if (failPercentage > 10 && failPercentage <= 50) 
		{
			Debug.LogError("ok cake -> 2 - 3 villagers die");
			deadVillagers = Random.Range(2, 3 + 1);
			CakesMade += 1;
		}
		else if (failPercentage > 50 && failPercentage <= 90) 
		{
			Debug.LogError("bad cake -> 3-5 villagers die");
			deadVillagers = Random.Range(3, 5 + 1);
			cutSceneType = 2;
			CakesMade += 1;
		}
		else if (failPercentage > 90) 
		{
			Debug.LogError("terrible cake (or no cake at all!)-> everyone dies");
			deadVillagers = VillagerAmount;
			cutSceneType = 2;
		}

		if (deadVillagers > VillagerAmount) deadVillagers = VillagerAmount;

		VillagerAmount -= deadVillagers;

		Kong.Play();
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		Music.Stop();

		//stop music
		/*while (Music.volume > 0)
		{
			Music.volume -= Time.deltaTime * 0.5f;
			yield return null;
		}
		Music.Stop();*/

		yield return new WaitForSeconds(2.5f);
	
		Timer.Hourglass.gameObject.SetActive(false);

		//god cutscene

		//neutral god
		if (cutSceneType == 0) yield return StartCoroutine(theGodCutScene.godHappyCutSceneCoroutine());
		
		//happy god
		else if (cutSceneType == 1) yield return StartCoroutine(theGodCutScene.godHappyCutSceneCoroutine());

		//angry god
		else if (cutSceneType == 2) yield return StartCoroutine(theGodCutScene.godAngryCutSceneCoroutine());

		for (int v = 0; v < deadVillagers; v++) 
		{
			Villagers[v].Die();
		}

		yield return new WaitForSeconds(0.6f);
		
		if (deadVillagers > 0)
		{       	
			lightningSource.Play();
		}

		for (int v = 0; v < deadVillagers; v++) 
		{
			Instantiate(LightningBoltPrefab, Villagers[v].transform.position,Quaternion.identity);
		}
	
		yield return new WaitForSeconds(2.8f);
		
		YearOverScreen.showResult(failPercentage, deadVillagers);

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
				Cake.SetCompletionPercentage(1f);
				//celebrations!!
				OnJudgementDay();
				return true;
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
		
		Cake.SetCompletionPercentage(1- (amountRequired / (float) maxAmountRequired));

		return true;
	}

	void Update()
	{
#if UNITY_EDITOR
		//restart
		if (Input.GetKeyDown(KeyCode.R)) GotoNextYear();

		//judgement day
		if (Input.GetKeyDown(KeyCode.J))
		{
			OnJudgementDay();
		}

		//fixed judgement days
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			StartCoroutine(JudgementDayCoroutine(0));
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			StartCoroutine(JudgementDayCoroutine(10));
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			StartCoroutine(JudgementDayCoroutine(50));
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			StartCoroutine(JudgementDayCoroutine(90));
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			StartCoroutine(JudgementDayCoroutine(100));
		}

		if (Input.GetKeyDown(KeyCode.Y))
		{
			for (int i = 0; i < (int)ResourceID._Amount; i++) 
			{
				CollectedResources.AddResource((ResourceID)i, 1000);
			}
			UpdateCollectedResourcesGUI();
		}
#endif
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
			RequiredResourceLayerPanels[i].CanvasGroup.alpha = (i == currentBakeLayer) ? 1f : 0.42f;
			RequiredResourceLayerPanels[i].Init(RequiredResources[i], RequiredResourcesMax[i], true);
		}
	}
}
