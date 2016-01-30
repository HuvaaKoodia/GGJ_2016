using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public float timeInMinute;

    public float timeRemaining;

    public bool isJudgementDay;

    public Text timerText;

    void Awake()
    {
        timeInMinute = 0;
    }

    // Use this for initialization
    void Start()
    {
        isJudgementDay = false;
        timeRemaining = timeInMinute*60 + 1;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining > 0 && isJudgementDay == false)
        {
            float minutes = Mathf.Floor(timeRemaining / 60);
            float seconds = Mathf.Floor(timeRemaining % 60);
            timerText.text = "Time left : " + minutes.ToString() + "." + seconds.ToString();
        }
        else
        {
            isJudgementDay = true;
            timeRemaining = timeInMinute * 60 + 1;
        }
    }
}
