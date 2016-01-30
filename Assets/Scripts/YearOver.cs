﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YearOver : MonoBehaviour
{
    public Text messageText;
    public Text announceText;

    public GameObject YearOverCanvas;

    public GameObject continueButton;
    public GameObject quitButton;

    void Awake()
    {
        YearOverCanvas.SetActive(false);
        continueButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void showResult(int failPercentage,int deadVillager){
        YearOverCanvas.SetActive(true);
        if (failPercentage == 0)
        {
            messageText.text = "Mmm... Delicious";
            announceText.text = "No one Died";
            continueButton.SetActive(true);
        }
        else if (failPercentage > 0 && failPercentage <= 10)
        {
            messageText.text = "Good enough";
            announceText.text = deadVillager + " " + "People Died";
            continueButton.SetActive(true);
        }
        else if (failPercentage > 10 && failPercentage <= 50)
        {
            messageText.text = "Unapprovable";
            announceText.text = deadVillager + " " + "People Died";
            continueButton.SetActive(true);
        }
        else if (failPercentage > 50 && failPercentage <= 90)
        {
            messageText.text = "What filth is this?";
            announceText.text = deadVillager + " " + "People Died";
        }
        if (failPercentage > 90)
        {
            messageText.text = "You are not worthy";
            announceText.text = "Everyone Died";
        }

        quitButton.gameObject.SetActive(false);
        continueButton.SetActive(true);
    }

    public void showGameOverScreen(int cakesMade)
    {
        YearOverCanvas.SetActive(true);
        messageText.text = "Game Over!";
        announceText.text = "You made " + cakesMade + " cakes";
        continueButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(true);
    }

    public void OnContinuePressed()
    {
        if (GameController.VillagerAmount == 0)
        {
            showGameOverScreen(GameController.CakesMade);
        }
        else
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
