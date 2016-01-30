using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseSelection : MonoBehaviour
{
    public RaycastHit hit;
    public Ray ray;

    Villager targetVillager;

    private List<Villager> villagers;

    public Timer timerThing;

    // Use this for initialization
    void Awake()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        villagers = new List<Villager>();
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

            bool commandGiven = false;
            
            //find resource or cake
            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Resource")))
            {
                var resource = hit.collider.GetComponent<ResourceView>();

                foreach (var aVillager in villagers)
                {
                    aVillager.getResource(resource);
                    aVillager.Deselect();
                    commandGiven = true;
                }
             }
             else if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Cake")))
             {
                foreach (var aVillager in villagers)
                {
                    aVillager.GoToBake(hit.transform.position);
                    aVillager.Deselect();
                    commandGiven = true;
                }
             }

            if (commandGiven) villagers.Clear();
            
			//find villager
            if (Physics.Raycast(ray, out hit, 100,  1 << LayerMask.NameToLayer("Villager")))
            {

                targetVillager = hit.transform.GetComponent<Villager>();
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.RightControl))
                {
                    Debug.Log("asdasdasd");
                    villagers.Add(targetVillager);
                    targetVillager.Select();
                }
                else 
                {
                    Debug.Log("###");
                    foreach (var villager in villagers) villager.Deselect();
                    villagers.Clear();
                    villagers.Add(targetVillager);
                    targetVillager.Select();
                }
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
