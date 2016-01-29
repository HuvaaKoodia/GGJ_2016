using UnityEngine;
using System.Collections;

public class MouseSelection : MonoBehaviour
{
    public RaycastHit hit;
    public Ray ray;

    Vector3 newPosition;
    Villager targetVillager;

    public Timer timerThing;

    // Use this for initialization
    void Awake()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        newPosition = transform.position;
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
            if (Physics.Raycast(ray, out hit, 100,  1 << LayerMask.NameToLayer("Villager")))
            {
                targetVillager = hit.transform.GetComponent<Villager>();
            }
            else if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Resource")))
            {
                newPosition = hit.point;
                targetVillager.getResource(newPosition);
            }
            else if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Ground")))
            {
                newPosition = hit.point;
                targetVillager.goToPoint(newPosition);
            }
        }
    }
}
