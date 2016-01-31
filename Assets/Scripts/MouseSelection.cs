﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseSelection : MonoBehaviour
{
    public RaycastHit hit;
    public Ray ray;

    Villager targetVillager;

    private List<Villager> villagers;

    public Timer timerThing;

    private bool isSelecting = false;
    Vector3 mousePosition1;

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
            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Resource")))
            {
                var resource = hit.collider.GetComponent<ResourceView>();

                foreach (var aVillager in villagers)
                {
                    aVillager.getResource(resource);
                    aVillager.Deselect();
                    commandGiven = true;
                }
             }
             else if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Cake")))
             {
                foreach (var aVillager in villagers)
                {
                    aVillager.GoToBake();
                    aVillager.Deselect();
                    commandGiven = true;
                }
             }

            if (commandGiven)
			{
				villagers.Clear();
				return;
			}
            
			//find villager
            if (Physics.Raycast(ray, out hit, 1000,  1 << LayerMask.NameToLayer("Villager")))
            {

                targetVillager = hit.transform.GetComponent<Villager>();
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.RightControl))
                {
                    villagers.Add(targetVillager);
                    targetVillager.Select();
                }
                else 
                {
                    foreach (var villager in villagers) villager.Deselect();
                    villagers.Clear();
                    villagers.Add(targetVillager);
                    targetVillager.Select();
                }
            }

            else 
            {
                isSelecting = true;
                mousePosition1 = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0) && isSelecting)
        {
            foreach (var villager in villagers) villager.Deselect();
            villagers.Clear();

            isSelecting = false;
            var allVillagers = new List<Villager>(FindObjectsOfType<Villager>());
            var camera = Camera.main;
            var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
            foreach (var villager in allVillagers)
            {
                if (viewportBounds.Contains(camera.WorldToViewportPoint(villager.transform.position)))
                {
                    villagers.Add(villager);
                    villager.Select();
                }
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}
