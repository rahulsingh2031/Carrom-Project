using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ScoreUI : MonoBehaviour
{
    //Script to handle UI for Score : Simple and maintable only for 2 player only though
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private TextMeshProUGUI player2Text;
    [SerializeField] private Image player1ScoreIndicator;
    [SerializeField] private Image player2ScoreIndicator;

    private Image currentScoreIndicator;
    private void Start()
    {
        currentScoreIndicator = player1ScoreIndicator;
        OnStart();
        ScoreManager.Instance.OnScoreIncrease += ScoreManager_OnScoreIncrease;
        GameManager.Instance.OnTurnSwitch += GameManager_OnTurnSwitch;
    }

    private void OnStart()
    {
        GameManager_OnTurnSwitch(Turn.Player1);
    }
    private void GameManager_OnTurnSwitch(Turn turn)
    {
        switch (turn)
        {
            case Turn.Player1:
                player1ScoreIndicator.gameObject.SetActive(true);
                player2ScoreIndicator.fillAmount = 1f;
                player2ScoreIndicator.gameObject.SetActive(false);
                currentScoreIndicator = player1ScoreIndicator;
                break;

            case Turn.Player2:
                player2ScoreIndicator.gameObject.SetActive(true);
                player1ScoreIndicator.fillAmount = 1f;
                player1ScoreIndicator.gameObject.SetActive(false);
                currentScoreIndicator = player2ScoreIndicator;
                break;
        }
    }
    private void Update()
    {
        currentScoreIndicator.fillAmount = GameManager.Instance.GetStrikerTimerNormalized();
    }
    private void ScoreManager_OnScoreIncrease(int score)
    {
        Turn currentTurn = GameManager.Instance.CurrentTurn;
        switch (currentTurn)
        {

            case Turn.Player1:
                player1Text.text = score.ToString();
                break;
            case Turn.Player2:
                player2Text.text = score.ToString();
                break;
        }
    }
}
