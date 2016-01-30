using UnityEngine;
using System.Collections;
using Constants;

public class Villager : MonoBehaviour
{
    public int speed = 8;
	public float BakingDelay = 5f;

    public Transform cakeThing;

    private bool gotoResourcePoint, gatheringResource, movement, goToBake, baking;

	Vector3 MovementTargetPosition, bakePosition;
	float gatheringTimer, bakingTimer;
	ResourceView currentResource;

	public bool CarryingResourceBackToCake{get{return gotoResourcePoint;}}

	public delegate void ResourceEvent(ResourceID resource);
	public delegate bool BoolAction();

	public ResourceEvent OnResourceDroppedEvent;
	public BoolAction OnBakeCompleteEvent;

	public bool AutomaticalGather = true;

	public GameObject SelectionCircle;

    void Start()
    {
        MovementTargetPosition = transform.position;
        gotoResourcePoint = false;
    }

    void Update()
    {
		if (gatheringResource)
		{
			if (gatheringTimer < Time.time)
			{
				//gathering done
				gatheringResource = false;
				gotoResourcePoint = true;
				MovementTargetPosition = Helpers.RandomPlanePosition(cakeThing.transform.position, 3f, 3f);
				movement = true;
			}
			return;
		}
		else if (baking)
		{
			if (bakingTimer < Time.time)
			{
				baking = false;
				GoToBake(bakePosition);
				if (OnBakeCompleteEvent != null)
				{
					bool successfulBake = OnBakeCompleteEvent();

					if (!successfulBake) 
					{
						//look puzzled?
					}
				}
			}
			return;
		}

        if (movement == true)
        {
            this.transform.position += (MovementTargetPosition - transform.position).normalized * Time.deltaTime * speed;
            
            if (Vector3.Distance(this.transform.position, MovementTargetPosition) < Time.deltaTime * speed)
            {
				//reached target
                movement = false;

				if (goToBake)
				{
					baking = true;
					bakingTimer = Time.time + BakingDelay;
				}
				else if (gotoResourcePoint == true)
				{
					//drop resource
					gotoResourcePoint = false;
					if (OnResourceDroppedEvent != null) OnResourceDroppedEvent(currentResource.Resource);
				
					if (AutomaticalGather)
					{
						getResource(currentResource);
					}
					else
					{
						currentResource = null;
					}
				}
				else if (currentResource)
				{
					//start gathering
					gatheringResource = true;
					gatheringTimer = Time.time + currentResource.ResourceGatheringDelay;
				}
            }
        }
    }

    public void goToPoint(Vector3 newPosition)
    {
        newPosition.y = transform.position.y;
        MovementTargetPosition = newPosition;
        movement = true;
    }

	public void GoToBake (Vector3 position)
	{
		//interrupt previous actions
		if (gotoResourcePoint) return;
		if (gatheringResource) gatheringResource= false;
		if (currentResource != null) currentResource = null;

		bakePosition = position;
		goToBake = true;
		movement = true;
		MovementTargetPosition = Helpers.RandomPlanePosition(position, 5f, 5f);
	}
	
	public void getResource(ResourceView resource)
    {
		//interrupt previous actions
		if (gotoResourcePoint) return;
		if (gatheringResource) gatheringResource= false;

		currentResource = resource;
		var newPosition = Helpers.RandomPlanePosition(resource.transform.position, 3f, 3f);
		newPosition.y = transform.position.y;
        MovementTargetPosition = newPosition;
        movement = true;
    }

	public void Stop ()
	{
		movement = false;
		baking = false;
		gatheringResource = false;
	}

	public void Select ()
	{
		SelectionCircle.SetActive(true);
	}

	public void Deselect ()
	{
		SelectionCircle.SetActive(false);
	}

}
