using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighScore : MonoBehaviour
{
    public GameObject HighScoreCanvas;

    public GameObject Backbutton;

    public Text NumberOneText;
    public Text NumberTwoText;
    public Text NumberThreeText;

    public bool BackToMainMenu;

    List<Scores> highScoreList;

    void Awake()
    {
        HighScoreCanvas.SetActive(false);
        Backbutton.SetActive(false);
        highScoreList = new List<Scores>();

        NumberOneText = GetComponent<Text>();
        NumberTwoText = GetComponent<Text>();
        NumberThreeText = GetComponent<Text>();
    }

    public void showHighScore(List<Scores> aList)
    {
        HighScoreCanvas.SetActive(true);
        Backbutton.SetActive(true);

        highScoreList = aList;

        if(highScoreList.Count >= 2)
            highScoreList.Sort();

        if (highScoreList.Count >= 1)
        {
            NumberOneText.text = "1st " + highScoreList[0].score;
        }
        else if (highScoreList.Count >= 2)
        {
            NumberTwoText.text = "2nd " + highScoreList[1].score;
        }
        else if (highScoreList.Count >= 3)
        {
            NumberThreeText.text = "3rd " + highScoreList[2].score;
        }
    }

    public void OnBackPressed()
    {
        HighScoreCanvas.SetActive(false);
        BackToMainMenu = true;
    }
}
