using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Turn
{
    //In Case of AI,Player 2 is AI
    Player1, Player2
}
public class GameManager : MonoBehaviour
{
    public event System.Action OnGameStart;
    public event System.Action OnGameOver;
    public event System.Action<Turn> OnTurnSwitch;
    public static GameManager Instance { get; private set; }

    private enum State
    {
        Starting, Strike, ChangeTurn, GameOver
    }

    public bool currentTurnHasHit = false;


    private float startTimer = 5f;
    [SerializeField] private float startTimerMax;
    private float strikerTimer = 20f;
    [SerializeField] private float strikerTimerMax;

    private float gameTimer;
    [SerializeField] private float gameTimerMax;
    public Turn CurrentTurn { get; private set; }
    private State gameState;
    private Coroutine strikeChangeCoroutine;
    private bool isGameStarted = false, isGameEnded = false;
    private bool shouldSwitch = true;
    private void Awake()
    {

        startTimer = startTimerMax;
        strikerTimer = strikerTimerMax;
        gameTimer = gameTimerMax;
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
        CarromStriker.OnStrike += CarromStriker_OnStrike;
        AIStriker.OnStrike += CarromStriker_OnStrike;
        PocketHandler.OnScore += PocketHandler_OnScore;
    }

    //Using State Machine to change State of game i.e Start State,Strike State,GameOver State
    private void Update()
    {
        if (isGameStarted && !isGameEnded)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                gameTimer = 0f;
                gameState = State.GameOver;
            }
        }
        switch (gameState)
        {
            case State.Starting:
                startTimer -= Time.deltaTime;
                if (startTimer <= 0)
                {
                    gameState = State.Strike;
                    isGameStarted = true;
                    OnGameStart?.Invoke();

                }
                break;

            case State.Strike:
                if (!currentTurnHasHit)
                {
                    strikerTimer -= Time.deltaTime;
                    if (strikerTimer < 0)
                    {
                        strikerTimer = strikerTimerMax;
                        StrikeEvent();

                    }
                }
                else
                {
                    if (strikeChangeCoroutine == null)
                    {
                        float waitTime = 3f;
                        strikeChangeCoroutine = StartCoroutine(WaitToSwitchStrikerCoroutine(waitTime));

                    }
                }
                break;
            case State.ChangeTurn:
                SwitchTurn();
                gameState = State.Strike;
                break;
            case State.GameOver:
                isGameEnded = true;
                OnGameOver?.Invoke();
                break;
        }

    }


    void CarromStriker_OnStrike()
    {

        currentTurnHasHit = true;
    }
    IEnumerator WaitToSwitchStrikerCoroutine(float timeToSwitch)
    {

        yield return new WaitForSeconds(timeToSwitch);
        strikerTimer = strikerTimerMax;
        currentTurnHasHit = false;

        StrikeEvent();
        strikeChangeCoroutine = null;


    }

    private void PocketHandler_OnScore()
    {
        shouldSwitch = false;
    }
    void StrikeEvent()
    {
        gameState = State.ChangeTurn;

        if (CurrentTurn == Turn.Player1)
            CarromStriker.Instance.ResetSide(shouldSwitch);
        else
        {
            AIStriker.Instance.ResetSide(shouldSwitch);
        }
    }
    void SwitchTurn()
    {
        if (!shouldSwitch)
        {
            shouldSwitch = true;
            OnTurnSwitch?.Invoke(CurrentTurn);
            return;
        }
        switch (CurrentTurn)
        {

            case Turn.Player1:
                CurrentTurn = Turn.Player2;
                AIStriker.Instance.Spawn();
                break;
            case Turn.Player2:
                CurrentTurn = Turn.Player1;
                CarromStriker.Instance.Spawn();
                break;

        }
        OnTurnSwitch?.Invoke(CurrentTurn);
    }

    public int GetGameTimeInSeconds() => Mathf.RoundToInt(gameTimer);
    public float GetStrikerTimerNormalized() => strikerTimer / strikerTimerMax;
    public int GetGameTimerMaxInSeconds() => (int)gameTimerMax;
}

