using UnityEngine;
using System.Collections;
using Constants;

public class Villager : MonoBehaviour
{
    public int speed = 8;
	public float BakingDelay = 5f;

    public Transform ResourceDropPoint, Cake;
	public Animator Animator;

    private bool gotoResourcePoint, gatheringResource, movement, goToBake, baking;

	Vector3 MovementTargetPosition;
	float gatheringTimer, bakingTimer;
	ResourceView currentResource, nextResourceCommand;

	public delegate void ResourceEvent(ResourceID resource);
	public delegate bool BoolAction();

	public ResourceEvent OnResourceDroppedEvent;
	public BoolAction OnBakeCompleteEvent;

	public bool AutomaticalGather = true, bakeNextCommand = false;

	public GameObject SelectionCircle, BakingHat;

    void Start()
    {
        MovementTargetPosition = transform.position;
        gotoResourcePoint = false;
		CarryResource.gameObject.SetActive(false);

		//randomize starting direction
		Animator.transform.localScale = new Vector3(Random.Range(0,2) == 0 ? -1 : 1, 1, 1);
    }

	public void Die()
	{
		Animator.SetBool("Dead", true);

		BakingHat.SetActive(false);
		CarryResource.gameObject.SetActive(false);

		Animator.SetBool("Carrying", false);
		Animator.SetBool("Walking", false);
		
		dead = true;
	}

	bool dead = false;
	public SpriteRenderer CarryResource;
	public Sprite[] CarrySprites;

    void Update()
    {
		if (dead) return;

		Animator.SetBool("Carrying", gotoResourcePoint);
		Animator.SetBool("Walking", movement);



		if (gatheringResource)
		{
			if (gatheringTimer < Time.time)
			{
				//gathering done
				gatheringResource = false;
				gotoResourcePoint = true;
				MovementTargetPosition = Helpers.RandomPlanePosition(ResourceDropPoint.transform.position, 3f, 3f);
				movement = true;

				CarryResource.gameObject.SetActive(true);
				CarryResource.sprite = CarrySprites[(int)currentResource.Resource];
			}
			return;
		}
		else if (baking)
		{
			if (bakingTimer < Time.time)
			{
				baking = false;
				GoToBake();
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
			var movementDirection = (MovementTargetPosition - transform.position).normalized;
            this.transform.position += movementDirection * Time.deltaTime * speed;
            
			int direction = (int)Mathf.Sign(Vector3.Dot(Camera.main.transform.right, movementDirection));
			Animator.transform.localScale = new Vector3(direction, 1, 1);


            if (Vector3.Distance(this.transform.position, MovementTargetPosition) < Time.deltaTime * speed)
            {
				//reached target
                movement = false;

				if (goToBake)
				{
					BakingHat.SetActive(true);
					baking = true;
					bakingTimer = Time.time + BakingDelay;
				}
				else if (gotoResourcePoint == true)
				{
					//drop resource
					gotoResourcePoint = false;
					CarryResource.gameObject.SetActive(false);
					if (OnResourceDroppedEvent != null) OnResourceDroppedEvent(currentResource.Resource);
				
					if (AutomaticalGather)
					{
						//change command
						if (nextResourceCommand != null)
						{
							bakeNextCommand = false;
							currentResource = nextResourceCommand;
							nextResourceCommand = null;
						}
						else if (bakeNextCommand)
						{
							bakeNextCommand = false;
							nextResourceCommand = null;
							GoToBake();
							return;
						}
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

	public void GoToBake ()
	{
		//interrupt previous actions
		if (gotoResourcePoint) 
		{
			bakeNextCommand = true;
			return;
		}

		gatheringResource= false;
		if (currentResource != null) currentResource = null;

		goToBake = true;
		movement = true;
		MovementTargetPosition = Helpers.RandomPlanePosition(Cake.position, 5f, 5f);
	}
	
	public void getResource(ResourceView resource)
    {
		if (gotoResourcePoint)
		{
			//just change the next resource
			nextResourceCommand = resource;
			return;
		}

		//interrupt previous actions

			baking = false;
			BakingHat.SetActive(false);

		gatheringResource= false;
		goToBake = false;

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

		CarryResource.gameObject.SetActive(false);
		
		Animator.SetBool("Carrying", false);
		Animator.SetBool("Walking", false);
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
