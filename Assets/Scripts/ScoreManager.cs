using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //ScoreManager stores and managers score of 2 players at a time but require work to extend for more than 2 players
    public static ScoreManager Instance { get; private set; }
    public event System.Action<int> OnScoreIncrease;
    [SerializeField] ScoreStats firstPlayerScoreStat = new ScoreStats();
    [SerializeField] ScoreStats secondPlayerScoreStat = new ScoreStats();
    private Dictionary<Turn, ScoreStats> turn_ScoreStats = new Dictionary<Turn, ScoreStats>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        turn_ScoreStats[Turn.Player1] = firstPlayerScoreStat;
        turn_ScoreStats[Turn.Player2] = secondPlayerScoreStat;

    }
    //Can be refactored by defining carrom pieces type and then by state checking from a method, assign the value
    //TODO:Refactor
    public void AddBlackPuck(int amount, Turn currentTurn)
    {
        turn_ScoreStats[currentTurn].blackPucks += amount;
        OnScoreIncrease?.Invoke(ScoreCalculator(currentTurn));
    }


    public void AddWhitePuck(int amount, Turn currentTurn)
    {
        turn_ScoreStats[currentTurn].whitePucks += amount;
        OnScoreIncrease?.Invoke(ScoreCalculator(currentTurn));
    }
    public void AddQueen(int amount, Turn currentTurn)
    {
        turn_ScoreStats[currentTurn].queen += amount;
        OnScoreIncrease?.Invoke(ScoreCalculator(currentTurn));


    }

    public string WinnerNameDecider()
    {
        int score1 = ScoreCalculator(firstPlayerScoreStat);
        int score2 = ScoreCalculator(secondPlayerScoreStat);
        string result;
        if (score1 > score2)
        {
            result = "Player";
        }
        else if (score1 < score2)
        {
            result = "AI";
        }
        else
        {
            result = "No one";
        }
        return result;
    }
    //Method of calculating score through either turn or scoreStats
    private int ScoreCalculator(Turn currentTurn)
    {
        return turn_ScoreStats[currentTurn].queen * 2 + turn_ScoreStats[currentTurn].whitePucks * 1 + turn_ScoreStats[currentTurn].blackPucks * 1;
    }
    private int ScoreCalculator(ScoreStats scoreStats)
    {
        return scoreStats.queen * 2 + scoreStats.whitePucks * 1 + scoreStats.blackPucks * 1;
    }
}

[System.Serializable]
public class ScoreStats
{
    public int blackPucks;
    public int whitePucks;
    public int queen;
}
