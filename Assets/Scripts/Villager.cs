using UnityEngine;
using System.Collections;
using Constants;

public class Villager : MonoBehaviour
{
    public int speed;

    public Transform cakeThing;

    private bool gotoCake, gatheringResource, movement;

    Vector3 MovementTargetPosition;
	float gatheringTimer;

	public bool CarryingResourceBackToCake{get{return gotoCake;}}

	public delegate void ResourceEvent(ResourceID resource);

	public ResourceEvent DroppedResourceAtCake;

	public bool AutomaticalGather = true;

    void Start()
    {
        MovementTargetPosition = transform.position;
        speed = 8;

        gotoCake = false;
    }

    void Update()
    {
		if (gatheringResource)
		{
			if (gatheringTimer < Time.time)
			{
				//gathering done
				gatheringResource = false;
				gotoCake = true;
				MovementTargetPosition = Helpers.RandomPlanePosition(cakeThing.transform.position, 3f, 3f);
				movement = true;
			}
		}

        if (movement == true)
        {
            this.transform.position += (MovementTargetPosition - transform.position).normalized * Time.deltaTime * speed;
            
            if (Vector3.Distance(this.transform.position, MovementTargetPosition) < Time.deltaTime * speed)
            {
				//reached target
                movement = false;

				if (gotoCake == true)
				{
					//drop resource
					gotoCake = false;
					if (DroppedResourceAtCake != null) DroppedResourceAtCake(currentResource.Resource);
				
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

	ResourceView currentResource;
	
	public void getResource(ResourceView resource)
    {
		currentResource = resource;
		var newPosition = Helpers.RandomPlanePosition(resource.transform.position, 3f, 3f);
		newPosition.y = transform.position.y;
        MovementTargetPosition = newPosition;
        movement = true;
    }
}
