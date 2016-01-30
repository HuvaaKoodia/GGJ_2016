using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public int timeInSeconds;
    private float timeRemaining;

    public bool isJudgementDay;
    public Text timerText;
	
    // Use this for initialization
    void Start()
    {
        isJudgementDay = false;
		timeRemaining = timeInSeconds + 1;
    }

    // Update is called once per frame
    void Update()
    {
	if (isJudgementDay) return;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining > 0)
        {
            float minutes = Mathf.Floor(timeRemaining / 60);
            float seconds = Mathf.Floor(timeRemaining % 60);
			timerText.text = string.Format("Time left : {0}.{1:00}", minutes, seconds);
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
