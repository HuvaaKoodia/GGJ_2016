using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour
{
    public int VillagerHeigh;

    public int item;

    public int speed;

    public Transform cakeThing;
    Vector3 cakePosition;

    private bool goToBakeCake;

    private bool movement;

    Vector3 NewestPosition;

    void Awake()
    {
        VillagerHeigh = 1;
        item = 0;
        NewestPosition = transform.position;
        speed = 8;

        cakePosition = cakeThing.transform.position;
        goToBakeCake = false;
    }

    void Update()
    {
        if (movement == true)
        {
            this.transform.position += (NewestPosition - transform.position).normalized * Time.deltaTime * speed;
            
            if (Vector3.Distance(this.transform.position, NewestPosition) < Time.deltaTime * speed)
            {
                movement = false;
            }
        }
        else if (goToBakeCake == true && movement == false)
        {
            NewestPosition = cakePosition;
            movement = true;
            item -= 1;

            goToBakeCake = false;
        }
    }

    public void goToPoint(Vector3 newPosition)
    {
        newPosition.y += VillagerHeigh;
        NewestPosition = newPosition;
        movement = true;
    }

    // player should wait for a while, also should not fly to the cake
    public void getResource(Vector3 newPosition)
    {
        newPosition.y += VillagerHeigh;
        NewestPosition = newPosition;
        movement = true;
        item += 1;
        goToBakeCake = true;
    }
}
