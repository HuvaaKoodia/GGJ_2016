using UnityEngine;
using System.Collections;

public class MouseSelection : MonoBehaviour
{
    public RaycastHit hit;
    public Ray ray;

    Villager targetVillager;

    public Timer timerThing;

    // Use this for initialization
    void Awake()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        selection();
    }

    void selection()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && timerThing.isJudgementDay == false)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			//find resource or cake
			if (targetVillager)
			{
				if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Resource")))
				{
					var resource = hit.collider.GetComponent<ResourceView>();
				
					targetVillager.getResource(resource);
					targetVillager.Deselect();
					targetVillager = null;
					return;
				}
				else if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Cake")))
				{
					targetVillager.GoToBake(hit.transform.position);
					targetVillager.Deselect();
					targetVillager = null;
					return;
				}
			}

			//find villager
            if (Physics.Raycast(ray, out hit, 100,  1 << LayerMask.NameToLayer("Villager")))
            {
                targetVillager = hit.transform.GetComponent<Villager>();
				targetVillager.Select();
            }

            /*else if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Ground")))
            {
				if (targetVillager)
				{
					targetVillager.goToPoint(hit.point);
				}
            }*/
        }
    }
}
