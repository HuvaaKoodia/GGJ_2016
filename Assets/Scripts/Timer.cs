using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public int timeInSeconds;
    private float timeRemaining, timeStart;

    public bool isJudgementDay;
    public Text timerText;

	public CakeView Hourglass;//Yep!

    // Use this for initialization
    void Start()
    {
        isJudgementDay = false;
		timeStart = timeInSeconds + 1;
		timeRemaining = timeStart;
    }

    // Update is called once per frame
    void Update()
    {
		if (isJudgementDay) return;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining > 0)
        {
			if (timerText)
			{
            float minutes = Mathf.Floor(timeRemaining / 60);
            float seconds = Mathf.Floor(timeRemaining % 60);
			timerText.text = string.Format("Time left : {0}.{1:00}", minutes, seconds);
			}
			if (Hourglass)
			{
				Hourglass.SetCompletionPercentage(1f - (timeRemaining / timeStart));
			}
        }
        else
        {
			StartJudgement();
        }
    }

	public void StartJudgement()
	{
		isJudgementDay = true;
		timerText.gameObject.SetActive(true);
		if (OnJudgementDayEvent != null) OnJudgementDayEvent();
	}

	public System.Action OnJudgementDayEvent;
}
