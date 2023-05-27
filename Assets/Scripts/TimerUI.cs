using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    //TODO:it might be better to seperate the timerLogic 

    bool isGameEnded, isGameStarted;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    void Start()
    {
        GameManager.Instance.OnGameStart += GameManager_OnGameStart;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

    }

    private void GameManager_OnGameStart()
    {
        isGameStarted = true;
    }

    private void GameManager_OnGameOver()
    {
        isGameEnded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted && !isGameEnded)
            GetTimeAndSetTimerText();
    }

    void GetTimeAndSetTimerText()
    {
        // totalMinutes = GameManager.Instance.GetGameTimerMaxInSeconds() % 60;
        int timeLeft = GameManager.Instance.GetGameTimeInSeconds();
        int remainingMinutes = timeLeft / 60;
        int remainingSeconds = timeLeft % 60;

        // string rm = remainingMinutes.ToString().PadLeft(2, '0');
        string rs = remainingSeconds.ToString().PadLeft(2, '0');
        timerText.text = $"{remainingMinutes}:{rs}";

    }
}
