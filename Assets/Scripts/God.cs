using UnityEngine;
using System.Collections;

public class God : MonoBehaviour
{
    public Animator Animator;
    public GameObject hand1, hand2;

    bool handtogglehack = false;

    public void GodAppear()
    {
        handtogglehack = true;
        Animator.SetTrigger("GodNeutral");
    }

	public void GodAppearAngry()
	{
		handtogglehack = true;
		Animator.SetTrigger("GodAngry");
	}

	public void GodAppearHappy()
	{
		handtogglehack = true;
		Animator.SetTrigger("GodHappy");
	}

    public void GodDisappear()
    {
        handtogglehack = true;
        Animator.SetTrigger("Disappear");
    }

    public void ToggleHands() 
    {
        if (!handtogglehack) return;
        handtogglehack = false;
        hand1.SetActive(!hand1.activeSelf);
        hand2.SetActive(!hand2.activeSelf);
    }
}
